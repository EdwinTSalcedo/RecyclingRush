import cv2
import numpy as np

# Leer la imagen de profundidad
depthmap = cv2.imread('depthmap.png', cv2.IMREAD_UNCHANGED)

# Punto de referencia (x, y)
ref_point = (50, 50)

# Crear un vector vacío para almacenar las distancias
distance_vector = []

# Recorrer cada pixel en la imagen de profundidad
for y in range(depthmap.shape[0]):
    for x in range(depthmap.shape[1]):
        # Calcular la distancia euclidiana desde el punto de referencia
        distance = np.sqrt((x - ref_point[0])**2 + (y - ref_point[1])**2)
        # Añadir la distancia al vector
        distance_vector.append(distance)

# Convertir la lista a un array de numpy
distance_vector = np.array(distance_vector)

print(distance_vector)
