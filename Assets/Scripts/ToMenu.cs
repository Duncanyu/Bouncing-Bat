using UnityEngine;
using UnityEngine.SceneManagement;

public class ToMenu : MonoBehaviour
{
    public void GoToMenu(){
        SceneManager.LoadScene("Start menu");
    }
}
