import socket
import threading
import cv2 as cv
import numpy as np
import matplotlib.pyplot as plt
import torch
import torchvision
from PIL import Image
import io
import requests
import time
import os
import glob
import ultralytics
ultralytics.checks()
from collections import deque
from ultralytics import YOLO
import requests
import select  # Importa la biblioteca select
import random
import gc
contant = 0
cont = 0
firstconect = False
client_socket = None
client_address = None
server_socket = None
initserver = False
def timer():
    global contant
    global cont
    global server_socket
    global client_socket
    global initserver
    global firstconect  # Agrega esta línea

    # Este es el bucle que se repetirá cada 3 segundos
    while True:
        time.sleep(120)
        print(cont)
        print(contant)
        if(firstconect == True):
            if contant+1  <= cont:
                
                print("El servidor está conectado")
                contant = cont
            else:
                print("Reconnecting with the client...")
                server_socket.close()
                client_socket.close()
                firstconect = False

    
        

# Esta es la creación del thread con la función timer
t = threading.Thread(target=timer)
# Este es el inicio del thread
t.start()

# Useful functions
def clear_console():
    os.system('cls' if os.name == 'nt' else 'clear')

intervalo_limpieza = 60  # 1 minuto en segundos
def obtain_angle_from_stereo(img1, img2):
    try:
        alto, ancho = img1.shape
        # Calcula la mitad del alto de las imágenes
        mitad_alto = 4 * alto // 6
        mitad_bajo = 6 * alto // 6

        # Recorta las imágenes para tomar solo la mitad inferior
        img1 = img1[mitad_alto:mitad_bajo, :]
        img2 = img2[mitad_alto:mitad_bajo, :]
        cv.imwrite("imagen_original.jpg", img1)
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
        
        disparity_SGBM = stereo.compute(img1, img2)
        disparity_SGBM = cv.normalize(disparity_SGBM, disparity_SGBM, alpha=255, beta=0, norm_type=cv.NORM_MINMAX)
        disparity_SGBM = np.uint8(disparity_SGBM)

        # Mostrar la figura con los dos subplots
    
        
        # Recortar la disparidad y realizar el procesamiento para obtener el resultado
        border_size = 150
        disparity_SGBM_cropped = disparity_SGBM[:, border_size:-border_size]
        thresh = 240
        _, disparity_SGBM_cropped = cv.threshold(disparity_SGBM_cropped, thresh, 255, cv.THRESH_BINARY)
        cv.imwrite("imagen_stereo.jpg", disparity_SGBM_cropped)
        # Calcular el resultado basado en la disparidad
        promedio_columnas = np.mean(disparity_SGBM_cropped, axis=0)
        
        group_size = len(promedio_columnas) // 7
        averaged_promedios = []
        
        for i in range(0, len(promedio_columnas), group_size):
            group = promedio_columnas[i:i + group_size]
            average = sum(group) / len(group)
            averaged_promedios.append(average)
        #for i in range(len(averaged_promedios)):
        #    if averaged_promedios[i] < 40:
        #        averaged_promedios[i] = 0
        todos_son_cero = all(valor == 0 for valor in averaged_promedios)
        min_value = min(averaged_promedios)
        print(averaged_promedios)
        if(todos_son_cero):
            min_position = 3
        else:
            min_position = averaged_promedios.index(min_value)
        plt.subplot(1, 2, 2) 
        
        return min_position
        
    except Exception as e:
        return 3
def obtain_angle_from_bboxes(bboxes, threshold=None):
    if len(bboxes) == 0:
        return "3"
    
    angles_election = [0, 0, 0, 0, 0, 0, 0]
    CATEGORIES = ["0", "1", "2", "3", "4", "5", "6"]
    
    for detection in bboxes:
        xmin, ymin, xmax, ymax = detection
        center_x = (xmin + xmax) // 2
        
        if threshold:
            if detection['confidence'] < threshold:
                continue
        
        if center_x <= 80:
            angles_election[0] += 1
        elif center_x <= 160:
            angles_election[1] += 1
        elif center_x <= 320:
            angles_election[2] += 1
        elif center_x <= 480:
            angles_election[3] += 1
        elif center_x <= 640:
            angles_election[4] += 1
        elif center_x <= 800:
            angles_election[5] += 1
        else:
            angles_election[6] += 1
    selected_angle = angles_election.index(max(angles_election))
    return selected_angle

def obtain_final_angle(angle1, angle2, w1, w2):
    weighted_angle = (angle1 * w1 + angle2 * w2)
    
    rounded_weighted_angle = round(weighted_angle)
    if(w1 == 0 and w2 == 0):
        rounded_weighted_angle = 3
    angle_map = {0: -45, 1: -30, 2: -15, 3: 0, 4: 15, 5: 30, 6: 45}
    final_angle = angle_map.get(rounded_weighted_angle, 0)
    print("angle1: " + str(angle1))
    print("angle2: " + str(angle2))
    print("final_angle" + str(final_angle))
    return final_angle


# Model initialization
CATEGORIES = ["0", "1", "2", "3", "4", "5", "6"]
model = YOLO('best.pt')
img_counter = 0
inference_queue = []
inference_queue = deque(maxlen=10) 
img_pil =[]
img1 = []
img2 = []
angle_stereo = 0
angle_yolo = 0
# Connection parameters
host = "localhost"
port = 1024
contclear = 0

nextContTime = time.time() +10.0
firstconect == False
# Main loop
while True:
    try:
        server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        server_socket.bind((host, port))
        server_socket.listen(5)
        print(f"Waiting for a connection on {host}:{port}")

        client_socket, client_address = server_socket.accept()
        print(f"Connection established with {client_address}")
        firstconect == True
        data = b''
        bytes_to_receive = 1024

        while True:
            if contclear >= 30:
                contclear = 0
                clear_console()
            chunk = client_socket.recv(bytes_to_receive)
            initserver = False
            if not chunk:
                break
            data += chunk

            if b'END_OF_IMAGE' in data:
                img_data = data.split(b'END_OF_IMAGE')

                for img_chunk in img_data:
                    if len(img_chunk) == 0:
                        continue

                    try:
                        nparr = np.frombuffer(img_chunk, np.uint8)
                        
                        if img_pil is not None:
                            if img_counter == 2:
                                img_pil = cv.imdecode(nparr, cv.IMREAD_COLOR)
                                results = model.predict(source=img_pil)
                                detections = results[0].boxes.xyxy.cpu().numpy()
                                if len(detections) == 0:
                                    angle_yolo = 3
                                else:
                                    angle_yolo = obtain_angle_from_bboxes(detections)
                                confidences = results[0].boxes.conf.cpu().numpy()
                                angle_stereo = obtain_angle_from_stereo(img1, img2)
                                w1 = 0.5
                                w2 = 0.5
                                if(angle_stereo == 3):
                                    w1 = 0
                                    w2 = 1
                                    if(angle_yolo == 3):
                                        w1 = 0
                                        w2 = 0
                                elif(angle_yolo == 3):
                                    w1 = 1
                                    w2 = 0
                                final_angle = obtain_final_angle(angle_stereo, angle_yolo, w1, w2)
                                
                                momentum = 0.3
                                inference_queue.insert(0, final_angle)
                                next_angle = sum(element * (momentum ** i) for i, element in enumerate(inference_queue))
                                if(len(inference_queue) > 9):
                                    inference_queue.pop()
                                data = round(next_angle, 2)
                                print("w1: " + str(w1))
                                print("w2: " + str(w2))
                                print("next_angle: " + str(next_angle))
                                client_socket.send(str(data).encode())
                                img_counter = 0
                                img1 = []
                                img2 = []
                                img_pil =[]
                                contclear += 1
                                cont += 1
                                final_angle = 0
                            else:
                                if img_counter == 0:
                                    img1 = cv.imdecode(nparr, cv.IMREAD_GRAYSCALE)
                                elif img_counter == 1:
                                    img2 = cv.imdecode(nparr, cv.IMREAD_GRAYSCALE)

                                    
                                img_counter += 1

                        else:
                            continue

                    except Exception as e:
                        print(f"Error: {e}")
                        continue

                    except IOError:
                        continue

                data = b''
                img_chunk = []

    except ConnectionResetError:
        continue

    except KeyboardInterrupt:
        print("Server closed")
        server_socket.close()
        break

    except Exception as e:
        print(f"Error: {e}")
        continue
    print("Limpiando memoria")
    gc.collect()