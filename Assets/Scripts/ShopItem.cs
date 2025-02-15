using TMPro;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    [Header("Color Settings")]
    public Color normalColor = Color.gray;
    public Color hoverColor = Color.white;
    public Color purchasedColor = Color.white;

    [Header("References")]
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemCost;
    public TextMeshProUGUI itemDescription;
    private SpriteRenderer spriteRenderer;
    public AudioSource source;
    public AudioClip purchaseSound;
    public AudioClip happyMouse;
    public AudioClip hoverSound;
    public AudioClip equipSound;
    public int ID;
    public int cost;
    public string itemNameStr = "Enter item name here.";
    public string itemDescriptionStr = "Enter item description here.";

    public bool isPurchased = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = normalColor;
        if (GetComponent<AudioSource>() != null)
            source = GetComponent<AudioSource>();
        else
            source = gameObject.AddComponent<AudioSource>();

        if (PlayerPrefs.GetInt("ShopItemPurchased_" + ID, 0) == 1)
        {
            isPurchased = true;
            spriteRenderer.color = purchasedColor;
        }
    }

    void OnMouseEnter()
    {
        if (!isPurchased)
        {
            spriteRenderer.color = hoverColor;
            source.PlayOneShot(hoverSound);
        }
        itemCost.text = cost.ToString();
        itemName.text = itemNameStr;
        itemDescription.text = itemDescriptionStr;
    }

    void OnMouseExit()
    {
        if (!isPurchased)
        {
            spriteRenderer.color = normalColor;
        }
    }

    void OnMouseDown()
    {
        if (!isPurchased && MainManager.balance >= cost)
        {
            isPurchased = true;
            spriteRenderer.color = purchasedColor;
            source.PlayOneShot(happyMouse);
            source.PlayOneShot(purchaseSound);
            MainManager.balance -= cost;
            PlayerPrefs.SetInt("ShopItemPurchased_" + ID, 1);
            PlayerPrefs.Save();
        }
        else if (isPurchased)
        {
            source.PlayOneShot(equipSound);
            MainManager.cosmeticEquipped = ID;
        }
        else
        {
            Debug.Log("Not enough balance.");
        }
    }
}
