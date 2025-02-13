using TMPro;
using UnityEngine;

public class AdjustScore : MonoBehaviour
{
    public PlayerControl playerController;
    public TextMeshProUGUI tmp;
    void Start(){
        tmp = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        tmp.text = playerController.score.ToString();
    }
}
