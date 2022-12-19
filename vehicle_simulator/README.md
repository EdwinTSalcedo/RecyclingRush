# Recogedor_de_basura_acuatico
 Simulador de un recogedor de basura autonomo acuatico en un ambiente similar a lago titicaca
https://drive.google.com/drive/folders/1suFUE9_UGYr8RlhIpPuyhrnLaKwBofjd?usp=sharing
En el link se tiene los codigos encargadas para conectarse con Unity. La division se debe a una mejor comprension de como se componen, es posible el juntarlo en uno solo para vincular las imagenes tomadas con la entrada del teclado.

REQUISITOS:
-Version de Unity necesario 2020.3.30f1.
-Descargar el archivo de "requirements.txt" del link anterior, para descargar las librerias necesarias para los archivos de python.
Pasos
-Descargar los archivos de python del link anterior y alojarlos en una carpeta, dentro de la misma, crear una subcarpeta llamada "imagenes" para depositar las imagenes generadas por el codigo.
-Inicializar el simulador de Unity, (Se recomienda activarlo en calidad muy baja)
-Ya activado, se tiene que ejecutar los archivos Python en el ambiente donde esten las librerias disponibles.
-el archivo "movpython.py" se encarga de controlar el movimiento del bote.
-el archivo "Camino,py" se encarga de enviar las imagenes del camino del bote.
-el archivo "Detector.py" se encarga de enviar imagenes de los objetos que estan en el area dde la boca del recegedor.

Por ahora, aun no se puede activar y desactivar los archivos python mientras el simulador de Unity este activado. De desactivar los archivos de Python, se necesita reiniciar el simulador de Unity para que se pueda activar de nuevo los archivos Python.

Los puertos para cada servidor de Unity son los siguientes:

Movimiento del bote, puerto: 2100
Camara del camino, puerto: 2500
Camara del detector, puerto: 2530

Es necesario descargar otra vez los archivos python para añadir los cambios realizados.
