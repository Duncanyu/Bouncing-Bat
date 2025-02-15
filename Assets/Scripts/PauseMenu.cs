using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void ToShop()
    {
        SceneManager.LoadSceneAsync("Shop");
        Debug.Log("To Shop");
    }
}
