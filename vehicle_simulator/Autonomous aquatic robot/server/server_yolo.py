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
import glob

# Define una función de formato para cada columna
formatters = {
    'column1': '{:,.2f}'.format,
    'column2': '{:,.2f}'.format,
    # Añade más columnas según sea necesario
}

def obtain_angle_from_bboxes(bboxes, threshold=None):
    if len(bboxes) == 0:
        return "3"

    angles_election = [0, 0, 0, 0, 0, 0, 0]
    CATEGORIES = ["0","1","2","3","4","5","6"]

    for index, row in bboxes.iterrows():
        center_x = (row['xmin'] + row['xmax']) // 2
        center_y = (row['ymin'] + row['ymax']) // 2

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
duckweed_detector = torch.hub.load('ultralytics/yolov5', 'custom', 'best.pt')

inference_queue = []

host = "localhost"
port = 12345
angle = 0

# Crea un socket del servidor
server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server_socket.bind((host, port))
server_socket.listen(5)

print(f"Esperando una conexión en {host}:{port}...")

while True:
    try:
        # Wait for a client to connect
        client_socket, client_address = server_socket.accept()
    
        print(f"Connection established with {client_address}")
        data = b''  # Initialize an empty byte variable to store received data
        bytes_to_receive = 6220800  # Number of bytes you want to receive in each iteration
        while True:
            # Receive data from the client
            try:
                request = client_socket.recv(bytes_to_receive)
                if not request:
                    print(f"Client at {client_address} disconnected")
                    break  # The client has disconnected, exit the inner loop

                img_pil = Image.open(io.BytesIO(request))
                img_pil = img_pil.convert('RGB')
                image_pil_resized = img_pil.resize((224, 126))
                compose = [torchvision.transforms.ToTensor(), torchvision.transforms.Normalize(mean=[0.485, 0.456, 0.406], std=[0.229, 0.224, 0.225])]
                transform = torchvision.transforms.Compose(compose)
                image_torch = transform(image_pil_resized)
                outputs = steering_estimator(torch.unsqueeze(image_torch, 0))
                _, preds = torch.max(outputs, 1)
                results = duckweed_detector(img_pil)
                bboxes = results.pandas().xyxy[0]
                # Angle predicted by steering angle estimator
                angle_resnet = str(preds.cpu().numpy()[0])
                # Angle predicted by duckweed detector
                angle_yolo = obtain_angle_from_bboxes(bboxes)
                w1 = 0.0
                w2 = 1.0
                final_angle = obtain_final_angle(angle_resnet, angle_yolo, w1, w2)
                
                if len(inference_queue) == 8:
                    inference_queue.pop(7)
                #momentum = 0.2
                #next_angle = sum(element * (momentum ** (i)) for i, element in enumerate(inference_queue))
                #inference_queue.insert(0, final_angle)
                data = final_angle
                data = round(data, 2)                
                print(data)
                #print(str(angle_yolo) + " " + str(angle_resnet))
                client_socket.send(str(data).encode()) 
                final = 0
                
                # Liberar recursos de imagen y memoria de tensores de PyTorch
                del img_pil
                del image_pil_resized
                del image_torch
                del outputs
                del preds
                del results
                del bboxes

            except Exception as e:
                print(f"Error processing image: {e}")

    except ConnectionResetError:
        # Handle an unexpected client disconnection
        print(f"Client at {client_address} disconnected unexpectedly")

    except KeyboardInterrupt:
        # Handle a keyboard interruption (Ctrl+C) to close the server
        print("Server closed")
        server_socket.close()
        break

    except Exception as e:
        print(f"Error: {e}")

