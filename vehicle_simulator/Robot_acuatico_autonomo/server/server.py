import socket
import threading
import cv2
import numpy as np
import torch
import requests
import time
torch.cuda.is_available()
device = torch.device('cuda' if torch.cuda.is_available() else 'cpu')
# model = torch.hub.load('ultralytics/yolov5s', 'yolov5s') 
model = torch.hub.load('ultralytics/yolov5', 'custom', 'best.pt')  
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
while True:
    # Crea un socket del servidor
    server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server_socket.bind((host, port))
    server_socket.listen(5)

    print(f"Esperando una conexión en {host}:{port}...")
    try:
        # Wait for a client to connect
        client_socket, client_address = server_socket.accept()
    
        print(f"Connection established with {client_address}")
        data = b''  # Initialize an empty byte variable to store received data
        bytes_to_receive = 2073600  # Number of bytes you want to receive in each iteration
        while True:
            # Receive data from the client
            try:
                request = client_socket.recv(bytes_to_receive)
            except ConnectionAbortedError:
                # Handle disconnection due to client abort
                break
            if not request:
                break  # The client has disconnected, exit the inner loop
            
            try:
                
                results = model(image)
                rangos = [0, 0, 0, 0, 0, 0, 0]  # Inicializa la lista de rangos con ceros
                # Add an extra column with a value of 5 to the results
                element = results.pandas().xyxy[0]
                for index, row in element.iterrows():
                    puntoMedioX = (row['xmin'] + row['xmax']) / 2
                    confidence = row['confidence']
                    if 0 <= puntoMedioX <= 274:
                        rangos[0] += confidence
                    elif 275 <= puntoMedioX <= 549:
                        rangos[1] += confidence
                    elif 550 <= puntoMedioX <= 823:
                        rangos[2] += confidence
                    elif 824 <= puntoMedioX <= 1098:
                        rangos[3] += confidence
                    elif 1099 <= puntoMedioX <= 1373:
                        rangos[4] += confidence
                    elif 1374 <= puntoMedioX <= 1647:
                        rangos[5] += confidence
                    elif 1648 <= puntoMedioX <= 1920:
                        rangos[6] += confidence
                rangoConMasPuntos = rangos.index(max(rangos))
                if rangoConMasPuntos == 0:
                    angulo=-45
                elif rangoConMasPuntos == 1:
                    angulo=-30
                elif rangoConMasPuntos == 2:
                    angulo=-15
                elif rangoConMasPuntos == 3:
                    angulo=0
                elif rangoConMasPuntos == 4:
                    angulo=15
                elif rangoConMasPuntos == 5:
                    angulo=30
                elif rangoConMasPuntos == 6:
                    angulo=45
                else:
                    print("Ningún rango tiene puntos medios.")
                
                # Si la lista tiene más de 10q elementos, elimina el más antiguo
                if len(inference_queue) == 8:
                    inference_queue.pop(7)
                sumangle = angulo + sum(element * (0.4 ** (i + 1)) for i, element in enumerate(inference_queue))
                inference_queue.insert(0, sumangle)
                data = str(sumangle)
                client_socket.send(data.encode()) 
            except Exception as e:
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
        continue