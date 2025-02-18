using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void ToShop()
    {
        PlayerControl pc = FindObjectOfType<PlayerControl>();
        if(pc != null)
        {
            MainManager.UpdateBalance(MainManager.balance + pc.score);
            pc.score = 0;
            Debug.Log("Updated balance from PlayerControl: " + MainManager.balance);
        }
        else
        {
            Debug.LogWarning("PlayerControl not found! Balance remains: " + MainManager.balance);
        }

        SceneManager.LoadSceneAsync("Shop");
        Debug.Log("To Shop");
    }
}
