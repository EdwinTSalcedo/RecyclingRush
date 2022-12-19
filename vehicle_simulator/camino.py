import keyboard
import socket
import time
import base64
import cv2
import numpy as np
datorecibido=0
encodeds=b''
host, port = "127.0.0.2", 2500
sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
sock.connect((host, port))
while True:
    time.sleep(0.5)
    if encodeds==b'':
        sock.sendall("Confirmacion".encode("UTF-8"))
        datorecibido=sock.recv(8000)
        encodeds = base64.b64encode(datorecibido)
    elif encodeds!=b'':
        im_arr = np.fromstring(encodeds, np.uint8)  
        im_bytes = base64.b64decode(im_arr)
        imagenarr = np.frombuffer(im_bytes, dtype=np.uint8)
        img = cv2.imdecode(imagenarr, flags=cv2.IMREAD_COLOR)
        imagen_grande=cv2.resize(img,(800,800))
        cv2.imwrite('imagenes/Camino.png',imagen_grande)
        encodeds=b''
    if keyboard.is_pressed("c"):
        sock.shutdown(socket.SHUT_RDWR)
        sock.close()
        break
