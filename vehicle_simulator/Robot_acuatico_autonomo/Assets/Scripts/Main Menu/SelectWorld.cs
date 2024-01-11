using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SelectWorld : MonoBehaviour
{
    public void CambiarEscena(int numberScene)
    {
        SceneManager.LoadScene("LoadofScene");
        LoadingScreenController.choice = numberScene;
    }
    public void PrincipalEscena()
    {
        SceneManager.LoadScene("Mainscene");
    }

}
