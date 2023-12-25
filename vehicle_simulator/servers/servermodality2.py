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
from ultralytics import YOLO
from collections import deque
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

    # Este es el bucle que se repetirá cada 20 segundos
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

def obtain_final_angle(angle1):
    rounded_weighted_angle = round(angle1)
    
    angle_map = {0: -45, 1: -30, 2: -15, 3: 0, 4: 15, 5: 30, 6: 45}
    final_angle = angle_map.get(rounded_weighted_angle, 0)
    return final_angle


# Model initialization
CATEGORIES = ["0", "1", "2", "3", "4", "5", "6"]
model = YOLO('best.pt')
img_counter = 0
inference_queue = []
inference_queue = deque(maxlen=10) 
img_pil =[]
angle_stereo = 0
angle_yolo = 0
# Connection parameters
host = "localhost"
port = 1024
contclear = 0

nextContTime = time.time() +10.0
# Main loop
firstconect = False
while True:
    try:
        server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        server_socket.bind((host, port))
        server_socket.listen(5)
        print(f"Waiting for a connection on {host}:{port}")

        client_socket, client_address = server_socket.accept()
        print(f"Connection established with {client_address}")
        firstconect = True
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
                        img_pil = Image.open(io.BytesIO(data))
                        img_pil = img_pil.convert('RGB')
                        results = model.predict(source=img_pil)
                        detections = results[0].boxes.xyxy.cpu().numpy()
                        if len(detections) == 0:
                            angle_yolo = 3
                        else:
                            angle_yolo = obtain_angle_from_bboxes(detections)
                        final_angle = obtain_final_angle(angle_yolo)
                        momentum = 0.3
                        inference_queue.insert(0, final_angle)
                        next_angle = sum(element * (momentum ** i) for i, element in enumerate(inference_queue))
                        if(len(inference_queue) > 9):
                            inference_queue.pop()
                        if(angle_yolo == 3):
                            next_angle = 0
                        data = round(next_angle, 2)
                        cont += 1
                        print("next_angle: " + str(next_angle))
                        client_socket.send(str(data).encode())
                        img_pil =[]
                        final_angle = 0
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