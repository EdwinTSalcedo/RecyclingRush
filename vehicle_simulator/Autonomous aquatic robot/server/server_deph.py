import socket
import cv2 as cv
import numpy as np

# Crea variables para almacenar las imágenes
img_izq = None
img_der = None
img_cen = None

server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server.bind(('127.0.0.1', 12345))
server.listen(1)

bicolor_count = 0
color_count = 0
img_counter = 0

while True:
    connection, address = server.accept()
    data = b''  # Inicializa un búfer de datos vacío

    # Lee los datos de la imagen en fragmentos y busca un marcador para delimitar cada imagen
    while True:
        chunk = connection.recv(1024)
        if not chunk:
            break
        data += chunk

        # Busca un marcador para determinar el final de una imagen
        if b'END_OF_IMAGE' in data:
            img_data = data.split(b'END_OF_IMAGE')
            for img_chunk in img_data:
                if len(img_chunk) == 0:
                    continue

                # Convierte los datos en una imagen
                nparr = np.frombuffer(img_chunk, np.uint8)
                img_np = cv.imdecode(nparr, cv.IMREAD_COLOR)

                # Comprueba si la imagen es en color o en blanco y negro
                if img_np is not None:
                    img_counter += 1
                    if img_counter % 3 == 0:
                        filename = f'color{1}.png'

                    else:
                        img_np = cv.cvtColor(img_np, cv.COLOR_BGR2GRAY)
                        filename = f'bicolor{bicolor_count % 2 + 1}.png'
                        bicolor_count += 1

                    cv.imwrite(filename, img_np)

            data = b''  # Reinicia el búfer de datos



    
