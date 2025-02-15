using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    public void LeaveShop(){
        SceneManager.LoadScene("Level");
        Debug.Log("Leaving Shop");
    }
}
