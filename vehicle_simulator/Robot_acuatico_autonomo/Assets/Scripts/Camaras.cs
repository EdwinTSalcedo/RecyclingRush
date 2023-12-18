using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camaras : MonoBehaviour
{
    public GameObject canvas;
    //Estas son las variables  donde se cargan las distintas camaras que se utilizaran para ver al recogedor de basura
    public Camera Espectador;
    public Camera Camino;
    public Camera Escena;
    //Esta variable se encargara de almacenar las variables de cada camara
    private Camera[] Cams;
    //Esta variable se encarga de almacenar a la camara seleccionada  
    private Camera currentCamera;
    //En esta variable se almacena el orden actual de la camara
    public int currentCameraIndex=0;
    //Se realizar estas operaciones al iniciar el programa
    void OnEnable()
    {
        //Se almacenan las camaras
        Cams=new Camera[]{Camino,Espectador, Escena};
        //Se pone la camara por defecto
        currentCamera=Camino;
        Espectador.enabled=false;
        //Se hace el cambio de camara
        Cambio();
    }
    //Se realizan las operaciones en cada frame que hay
    public void CameraNext()
    {
        currentCameraIndex++;
        if(currentCameraIndex>2)
        {
            currentCameraIndex=0;
        }
        Cambio();
    }
    void Cambio()
    {
        currentCamera.enabled=false;
        currentCamera=Cams[currentCameraIndex];
        currentCamera.enabled=true;
    }
}
