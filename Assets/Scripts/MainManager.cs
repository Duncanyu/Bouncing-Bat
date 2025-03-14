using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;

    public static int balance = 0;
    public int initialBalance = 0;
    public static int cosmeticEquipped = 13;
    
    private const string BalanceKey = "Balance";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        balance = PlayerPrefs.GetInt(BalanceKey, initialBalance);
    }

    public static void UpdateBalance(int newBalance)
    {
        balance = newBalance;
        PlayerPrefs.SetInt(BalanceKey, balance);
        PlayerPrefs.Save();
    }
}
