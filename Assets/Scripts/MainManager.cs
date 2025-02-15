using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;

    public static int balance = 0;
    public int initialBalance = 0;
    public static int cosmeticEquipped = 13;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        if (balance == 0)
        {
            balance = initialBalance;
        }
    }

    public void LeaveShop()
    {
        SceneManager.LoadScene("Level");
    }

    public void ToShop()
    {
        PlayerControl pc = FindObjectOfType<PlayerControl>();
        if(pc != null)
        {
            balance += pc.score;
            pc.score = 0;
        }
        SceneManager.LoadScene("Shop");
    }
}
