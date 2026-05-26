using UnityEngine;
using UnityEngine.SceneManagement; 

public class MainMenuManager : MonoBehaviour
{

    public void PlayGame()
    {

        SceneManager.LoadScene("GameScene");
    }

    public void LoadStressTest()
    {
        SceneManager.LoadScene("StressTestScene");
    }

    public void QuitGame()
    {
        Debug.Log("Гра закривається!"); 
        Application.Quit(); 
    }
}