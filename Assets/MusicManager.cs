using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep this GameObject when changing scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }
}
