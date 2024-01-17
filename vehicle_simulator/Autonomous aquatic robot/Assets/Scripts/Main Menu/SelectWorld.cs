using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectWorld : MonoBehaviour
{
    public void ChangeScene(int sceneNumber)
    {
        SceneManager.LoadScene("LoadofScene");
        LoadingScreenController.choice = sceneNumber;
    }

    public void LoadMainScene() => SceneManager.LoadScene("Mainscene");
}
