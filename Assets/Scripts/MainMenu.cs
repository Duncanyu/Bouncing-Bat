using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Playgame()
    {
        SceneManager.LoadSceneAsync("Level");
    }
    public void GoToControls(){
        SceneManager.LoadScene("Instructions");
    }
}
