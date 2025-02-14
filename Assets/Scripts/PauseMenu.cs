using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    void Shop()
    {
        SceneManager.LoadSceneAsync("Level");
    }
}
