import numpy as np
import cv2 as cv
import matplotlib.pyplot as plt

# Read both images and convert to grayscale
img1 = cv.imread('L.png', cv.IMREAD_GRAYSCALE)
img2 = cv.imread('R.png', cv.IMREAD_GRAYSCALE)

# ------------------------------------------------------------
# PREPROCESSING

# Compare unprocessed images
fig, axes = plt.subplots(1, 2, figsize=(15, 10))
axes[0].imshow(img1, cmap="gray")
axes[1].imshow(img2, cmap="gray")
axes[0].axhline(250)
axes[1].axhline(250)
axes[0].axhline(450)
axes[1].axhline(450)



# 1. Detect keypoints and their descriptors
# Based on: https://docs.opencv.org/master/dc/dc3/tutorial_py_matcher.html

# Initiate SIFT detector
sift = cv.SIFT_create()
# find the keypoints and descriptors with SIFT
kp1, des1 = sift.detectAndCompute(img1, None)
kp2, des2 = sift.detectAndCompute(img2, None)


# Visualize keypoints
imgSift = cv.drawKeypoints(
    img1, kp1, None, flags=cv.DRAW_MATCHES_FLAGS_DRAW_RICH_KEYPOINTS)

# Match keypoints in both images
# Based on: https://docs.opencv.org/master/dc/dc3/tutorial_py_matcher.html
FLANN_INDEX_KDTREE = 1
index_params = dict(algorithm=FLANN_INDEX_KDTREE, trees=5)
search_params = dict(checks=50)   # or pass empty dictionary
flann = cv.FlannBasedMatcher(index_params, search_params)
matches = flann.knnMatch(des1, des2, k=2)

# Keep good matches: calculate distinctive image features
# Lowe, D.G. Distinctive Image Features from Scale-Invariant Keypoints. International Journal of Computer Vision 60, 91–110 (2004). https://doi.org/10.1023/B:VISI.0000029664.99615.94
# https://www.cs.ubc.ca/~lowe/papers/ijcv04.pdf
matchesMask = [[0, 0] for i in range(len(matches))]
good = []
pts1 = []
pts2 = []

for i, (m, n) in enumerate(matches):
    if m.distance < 0.7*n.distance:
        # Keep this keypoint pair
        matchesMask[i] = [1, 0]
        good.append(m)
        pts2.append(kp2[m.trainIdx].pt)
        pts1.append(kp1[m.queryIdx].pt)



# Draw the keypoint matches between both pictures
# Still based on: https://docs.opencv.org/master/dc/dc3/tutorial_py_matcher.html
draw_params = dict(matchColor=(0, 255, 0),
                   singlePointColor=(255, 0, 0),
                   matchesMask=matchesMask[300:500],
                   flags=cv.DrawMatchesFlags_DEFAULT)

keypoint_matches = cv.drawMatchesKnn(
    img1, kp1, img2, kp2, matches[300:500], None, **draw_params)

# ------------------------------------------------------------
# STEREO RECTIFICATION

# Calculate the fundamental matrix for the cameras
# https://docs.opencv.org/master/da/de9/tutorial_py_epipolar_geometry.html
pts1 = np.int32(pts1)
pts2 = np.int32(pts2)
fundamental_matrix, inliers = cv.findFundamentalMat(pts1, pts2, cv.FM_RANSAC)

# We select only inlier points
pts1 = pts1[inliers.ravel() == 1]
pts2 = pts2[inliers.ravel() == 1]


# Stereo rectification (uncalibrated variant)
# Adapted from: https://stackoverflow.com/a/62607343
h1, w1 = img1.shape
h2, w2 = img2.shape
_, H1, H2 = cv.stereoRectifyUncalibrated(
    np.float32(pts1), np.float32(pts2), fundamental_matrix, imgSize=(w1, h1)
)



# Undistort (rectify) the images and save them
# Adapted from: https://stackoverflow.com/a/62607343
img1_rectified = cv.warpPerspective(img1, H1, (w1, h1))
img2_rectified = cv.warpPerspective(img2, H2, (w2, h2))



# Draw the rectified images
fig, axes = plt.subplots(1, 2, figsize=(15, 10))
axes[0].imshow(img1_rectified, cmap="gray")
axes[1].imshow(img2_rectified, cmap="gray")
axes[0].axhline(250)
axes[1].axhline(250)
axes[0].axhline(450)
axes[1].axhline(450)




# ------------------------------------------------------------
# CALCULATE DISPARITY (DEPTH MAP)
# Adapted from: https://github.com/opencv/opencv/blob/master/samples/python/stereo_match.py
# and: https://docs.opencv.org/master/dd/d53/tutorial_py_depthmap.html

# StereoSGBM Parameter explanations:
# https://docs.opencv.org/4.5.0/d2/d85/classcv_1_1StereoSGBM.html

# Matched block size. It must be an odd number >=1 . Normally, it should be somewhere in the 3..11 range.
block_size = 11
min_disp = -128
max_disp = 128
# Maximum disparity minus minimum disparity. The value is always greater than zero.
# In the current implementation, this parameter must be divisible by 16.
num_disp = max_disp - min_disp
# Margin in percentage by which the best (minimum) computed cost function value should "win" the second best value to consider the found match correct.
# Normally, a value within the 5-15 range is good enough
uniquenessRatio = 5
# Maximum size of smooth disparity regions to consider their noise speckles and invalidate.
# Set it to 0 to disable speckle filtering. Otherwise, set it somewhere in the 50-200 range.
speckleWindowSize = 200
# Maximum disparity variation within each connected component.
# If you do speckle filtering, set the parameter to a positive value, it will be implicitly multiplied by 16.
# Normally, 1 or 2 is good enough.
speckleRange = 2
disp12MaxDiff = 0

stereo = cv.StereoSGBM_create(
    minDisparity=min_disp,
    numDisparities=num_disp,
    blockSize=block_size,
    uniquenessRatio=uniquenessRatio,
    speckleWindowSize=speckleWindowSize,
    speckleRange=speckleRange,
    disp12MaxDiff=disp12MaxDiff,
    P1=8 * 1 * block_size * block_size,
    P2=32 * 1 * block_size * block_size,
)
disparity_SGBM = stereo.compute(img1_rectified , img2_rectified )

# Normalize the values to a range from 0..255 for a grayscale image
disparity_SGBM = cv.normalize(disparity_SGBM, disparity_SGBM, alpha=255,
                              beta=0, norm_type=cv.NORM_MINMAX)
disparity_SGBM = np.uint8(disparity_SGBM)

border_size = 140

# Crop the disparity map
disparity_SGBM_cropped = disparity_SGBM[:, border_size:-border_size]
plt.imshow(img1_rectified)
plt.colorbar()
plt.show()
thresh = 220  # Ajusta este valor según tus necesidades

# Aplicar el umbral a la imagen
_, disparity_SGBM_cropped = cv.threshold(disparity_SGBM_cropped, thresh, 255, cv.THRESH_BINARY)

cv.imshow("Disparity", disparity_SGBM_cropped)
cv.imwrite("disparity_SGBM_norm.png", disparity_SGBM_cropped)






# Calcular el promedio de disparidad para cada columna
promedio_columnas = np.mean(disparity_SGBM_cropped, axis=0)

# Imprimir el vector de promedios
print(promedio_columnas)

# Especifica el nombre del archivo en el que deseas guardar los promedios
nombre_archivo = "promedios_disparidad.txt"

# Guardar el vector de promedios en un archivo de texto
np.savetxt(nombre_archivo, promedio_columnas)

print(f"Los promedios de disparidad se han guardado en el archivo '{nombre_archivo}'.")

cv.waitKey()








# Convertir a imágenes de escala de grises
    img1 = cv.cvtColor(img1, cv.COLOR_BGR2GRAY)
    img2 = cv.cvtColor(img2, cv.COLOR_BGR2GRAY)

    # Detectar puntos clave y calcular descriptores
    sift = cv.SIFT_create()
    kp1, des1 = sift.detectAndCompute(img1, None)
    kp2, des2 = sift.detectAndCompute(img2, None)

    # Realizar emparejamiento de puntos
    flann = cv.FlannBasedMatcher(dict(algorithm=1, trees=5), dict(checks=50))
    matches = flann.knnMatch(des1, des2, k=2)

    # Filtrar coincidencias buenas
    good = []
    for m, n in matches:
        if m.distance < 0.7 * n.distance:
            good.append(m)

    # Obtener puntos correspondientes
    pts1 = np.float32([kp1[m.queryIdx].pt for m in good]).reshape(-1, 1, 2)
    pts2 = np.float32([kp2[m.trainIdx].pt for m in good]).reshape(-1, 1, 2)

    # Calcular la matriz fundamental
    fundamental_matrix, inliers = cv.findFundamentalMat(pts1, pts2, cv.FM_RANSAC)

    # Filtrar los puntos de acuerdo a los inliers
    pts1 = pts1[inliers.ravel() == 1]
    pts2 = pts2[inliers.ravel() == 1]

    # Calcular la rectificación estéreo
    _, H1, H2 = cv.stereoRectifyUncalibrated(
        pts1, pts2, fundamental_matrix, imgSize=(img1.shape[1], img1.shape[0])
    )

    # Rectificar las imágenes
    img1_rectified = cv.warpPerspective(img1, H1, (img1.shape[1], img1.shape[0]))
    img2_rectified = cv.warpPerspective(img2, H2, (img2.shape[1], img2.shape[0]))

    # Calcular la disparidad
    stereo = cv.StereoSGBM_create(
        minDisparity=-128,
        numDisparities=256,
        blockSize=11,
        uniquenessRatio=5,
        speckleWindowSize=200,
        speckleRange=2,
        disp12MaxDiff=0,
        P1=8 * 1 * 11 * 11,
        P2=32 * 1 * 11 * 11
    )
    disparity_SGBM = stereo.compute(img1_rectified, img2_rectified)
    disparity_SGBM = cv.normalize(disparity_SGBM, disparity_SGBM, alpha=255, beta=0, norm_type=cv.NORM_MINMAX)
    disparity_SGBM = np.uint8(disparity_SGBM)

    # Recortar la disparidad y realizar el procesamiento para obtener el resultado
    border_size = 140
    disparity_SGBM_cropped = disparity_SGBM[:, border_size:-border_size]
    thresh = 220
    _, disparity_SGBM_cropped = cv.threshold(disparity_SGBM_cropped, thresh, 255, cv.THRESH_BINARY)

    # Calcular el resultado basado en la disparidad
    promedio_columnas = np.mean(disparity_SGBM_cropped, axis=0)
    group_size = len(promedio_columnas) // 3
    averaged_promedios = []

    for i in range(0, len(promedio_columnas), group_size):
        group = promedio_columnas[i:i + group_size]
        average = sum(group) / len(group)
        averaged_promedios.append(average)

    min_value = min(averaged_promedios)
    min_indices = [i for i, value in enumerate(averaged_promedios) if value == min_value]

    if len(min_indices) == 3:
        selected_index = 1
    else:
        selected_index = random.choice(min_indices)

    resultado = -3 if selected_index == 0 else (0 if selected_index == 1 else 3)
    return str(resultado)