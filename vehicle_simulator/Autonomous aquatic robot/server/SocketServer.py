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

# Activar el modo interactivo de matplotlib
plt.ion()

# Crear el modelo YOLO fuera del bucle principal
model = YOLO('best.pt')

sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

server_address = ('localhost', 5000)
sock.bind(server_address)
sock.listen()
torch.cuda.is_available()

print("Servidor conectado")

while True:
    # Wait for a connection
    connection, client_address = sock.accept()
    
    data = b''
    data = connection.recv(6000000)
    nparr = np.frombuffer(data, np.uint8)
    img_np = cv2.imdecode(nparr, cv2.IMREAD_COLOR)
    print("Se recibió datos")
    results = model.predict(img_np, save=True)
    try:
        # Enviar "Hola mundo" al cliente
        connection.sendall(b"Hola mundo")
        print("Mensaje 'Hola mundo' enviado al cliente")
    except Exception as e:
        print(f"Error al enviar datos al cliente: {e}")
    
    connection.close()
