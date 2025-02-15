using TMPro;
using UnityEngine;

public class AdjustBalance : MonoBehaviour
{
    public TextMeshProUGUI textMesh;

    void Update(){
        textMesh.text = $"Balance: {MainManager.balance}";
    }
}
