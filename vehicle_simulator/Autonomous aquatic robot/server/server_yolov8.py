import socket
import threading
import cv2
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
import cv2
import requests
import select  # Importa la biblioteca select


def clear_console():
    os.system('cls' if os.name == 'nt' else 'clear')
intervalo_limpieza = 60  # 1 minuto en segundos


def obtain_angle_from_bboxes(bboxes, threshold=None):
  if len(bboxes) == 0:
    return "3"

  angles_election = [0, 0, 0, 0, 0, 0, 0]
  CATEGORIES = ["0","1","2","3","4","5","6"]
  for detection in bboxes:
    xmin, ymin, xmax, ymax = detection
    center_x =  (xmin + xmax) // 2
    if threshold:
      if row['confidence'] < threshold:
        continue
    if center_x <= 274:
      angles_election[0] += 1
    elif center_x <= 549:
      angles_election[1] += 1
    elif center_x <= 823:
      angles_election[2] += 1
    elif center_x <= 1098:
      angles_election[3] += 1
    elif center_x <= 1373:
      angles_election[4] += 1
    elif center_x <= 1647:
      angles_election[5] += 1
    else:
      angles_election[6] += 1
  selected_angle = angles_election.index(max(angles_election))
  return str(selected_angle)

def obtain_final_angle(angle1, angle2, w1, w2):
  weighted_angle = int(float(angle1)*w1 + float(angle2)*w2)

  if weighted_angle == 0:
    final_angle = -45
  elif weighted_angle == 1:
    final_angle = -30
  elif weighted_angle == 2:
    final_angle = -15
  elif weighted_angle == 3:
    final_angle = 0
  elif weighted_angle == 4:
    final_angle = 15
  elif weighted_angle == 5:
    final_angle = 30
  else:
    final_angle = 45

  return final_angle
CATEGORIES = ["0","1","2","3","4","5","6"]
# Load models
steering_estimator = torchvision.models.resnet34()
steering_estimator.fc = torch.nn.Linear(in_features=512, out_features=7)
state_dict = torch.load("steering_estimator.pt")
steering_estimator.load_state_dict(state_dict)
steering_estimator.eval()
model = YOLO('best.pt')
contclear=0

inference_queue = []


# Define una función de formato para cada columna
formatters = {
    'column1': '{:,.2f}'.format,
    'column2': '{:,.2f}'.format,
    # Añade más columnas según sea necesario
}    

host = "localhost"
port = 12345
angle=0


def calcular_momentum():
    # Ruta actual del script
    script_directory = os.path.dirname(os.path.realpath(__file__))
    carpeta_momentum = os.path.join(script_directory, "momentum")

    # Obtener la lista de archivos en la carpeta "momentum"
    archivos_en_carpeta = os.listdir(carpeta_momentum)

    # Determinar el valor de "momentum" basado en la cantidad de archivos
    momentum = 0.1 + len(archivos_en_carpeta) //20 * 0.1

    return momentum

while True:
    try:
        # Crea un socket del servidor
        server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        server_socket.bind((host, port))
        server_socket.listen(5)

        print(f"Esperando una conexión en {host}:{port}")

        # Wait for a client to connect
        client_socket, client_address = server_socket.accept()

        print(f"Connection established with {client_address}")
        data = b''  # Initialize an empty byte variable to store received data
        bytes_to_receive = 2073600  # Number of bytes you want to receive in each iteration

        while True:
            if contclear >= 30:
              contclear = 0
              print("hola pew")
              clear_console()
            ready_to_read, _, _ = select.select([client_socket], [], [], 13)  # Espera 2 segundos

            if not ready_to_read:
                # No se recibieron datos del cliente en 2 segundos, intenta reconectar
                print("Reconnecting with the client...")
                server_socket.close()
                break
            try:
                request = b''
                request = client_socket.recv(bytes_to_receive)
            except ConnectionAbortedError:
                print(f"Client at {client_address} disconnected")
                break
            if not request:
                print(f"Client at {client_address} disconnected")
                break
           
            try:
                img_pil = Image.open(io.BytesIO(request))
                img_pil = img_pil.convert('RGB')
                image_pil_resized = img_pil.resize((1920 , 1080))
                compose = [torchvision.transforms.ToTensor(), torchvision.transforms.Normalize(mean=[0.485, 0.456, 0.406], std=[0.229, 0.224, 0.225])]
                transform = torchvision.transforms.Compose(compose)
                image_torch = transform(image_pil_resized)
                outputs = steering_estimator(torch.unsqueeze(image_torch, 0))
                _, preds = torch.max(outputs, 1)
                results = model.predict(source=img_pil)
                detections = results[0].boxes.xyxy.cpu().numpy()
                confidences = results[0].boxes.conf.cpu().numpy()
                angle_resnet = str(preds.cpu().numpy()[0])
                angle_yolo = obtain_angle_from_bboxes(detections)
                w1 = 0.0
                w2 = 1.0
                final_angle = obtain_final_angle(angle_resnet, angle_yolo, w1, w2)
                            
                if len(inference_queue) == 8:
                    inference_queue.pop(7)
                momentum = calcular_momentum()
                next_angle =sum(element * (momentum ** (i)) for i, element in enumerate(inference_queue))
                inference_queue.insert(0, final_angle)
                data=next_angle
                data = round(data, 2)      
                print("momentum:" + str(momentum))       
                contclear= contclear + 1   
                client_socket.send(str(data).encode()) 
            except Exception as e:
                print(f"Error: {e}")
                continue
                # O manejar otros errores generales aquí
            except IOError:
              continue

    except ConnectionResetError:
        # Handle an unexpected client disconnection
        continue

    except KeyboardInterrupt:
        # Handle a keyboard interruption (Ctrl+C) to close the server
        print("Server closed")
        server_socket.close()
        break

    except Exception as e:
        print(f"Error: {e}")
        continue