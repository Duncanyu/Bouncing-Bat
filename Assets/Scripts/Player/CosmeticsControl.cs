using UnityEngine;

public class CosmeticsControl : MonoBehaviour
{
    public int cosmeticIDNumber = 13;
    public static int cosmeticID = 13; // 13 = none, 12 = laser, 11 = cloud, 10 = weight, 9 = CIA listening kit, 8 = drill, 7 = mono, 6 = miners helm, 5 = mil helm, 4 = biker helm, 3 = crown, 2 = burger, 1 = cowboy, 0 = tophat
    public Sprite[] cosmeticSprites;
    private SpriteRenderer spriteRender;

    private int currentCosmeticID = 13;

    void Start(){
        cosmeticID = MainManager.cosmeticEquipped;
        currentCosmeticID = cosmeticID;
        spriteRender = GetComponent<SpriteRenderer>();
        UpdateCosmetic();
    }

    void Update(){
        if(currentCosmeticID != MainManager.cosmeticEquipped){
            currentCosmeticID = MainManager.cosmeticEquipped;
            cosmeticID = currentCosmeticID;
            UpdateCosmetic();
        }
    }

    void UpdateCosmetic(){
        if(cosmeticID == 0){
            spriteRender.sprite = cosmeticSprites[0];
        } else if(cosmeticID == 1){
            spriteRender.sprite = cosmeticSprites[1];
        } else if(cosmeticID == 2){
            spriteRender.sprite = cosmeticSprites[2];
        } else if(cosmeticID == 3){
            spriteRender.sprite = cosmeticSprites[3];
            PlayerControl.pointIncreaseRate = 3;
        } else if(cosmeticID == 4){
            spriteRender.sprite = cosmeticSprites[4];
            PlayerControl.health += 1;
        } else if(cosmeticID == 5){
            spriteRender.sprite = cosmeticSprites[5];
            PlayerControl.health += 3;
        } else if(cosmeticID == 6){
            spriteRender.sprite = cosmeticSprites[6];
            PlayerControl.spotLightColor = new Color32(255, 255, 255, 255);
            PlayerControl.spotLightRange = 18f;
            PlayerControl.spotLightIntensity = 0.7f;
            PlayerControl.spotlightInnerAngle = 2.8f;
            PlayerControl.spotLightOuterAngle = 3f;
        } else if(cosmeticID == 7){
            spriteRender.sprite = cosmeticSprites[7];
            PlayerControl.pointIncreaseRate = 2;
        } else if(cosmeticID == 8){
            spriteRender.sprite = cosmeticSprites[8];
        } else if(cosmeticID == 9){
            spriteRender.sprite = cosmeticSprites[9];
            LightController.radius = 14f;
            LightController.intensity = 2.5f;
            PlayerControl.abilityCooldownTime = 1.2f;
        } else if(cosmeticID == 10){
            spriteRender.sprite = cosmeticSprites[10];
            PlayerControl.gravityMultiplier = 3.05f;
        } else if(cosmeticID == 11){
            spriteRender.sprite = cosmeticSprites[11];
            PlayerControl.jumpingPower = 5;
            PlayerControl.gravityMultiplier = 1f;
        } else if(cosmeticID == 12){
            spriteRender.sprite = cosmeticSprites[12];
            PlayerControl.spotlightInnerAngle = 0f;
            PlayerControl.spotLightOuterAngle = 80.5f;
            PlayerControl.spotLightColor = new Color32(255, 0, 0, 255);
            PlayerControl.spotLightIntensity = 0.7f;
            PlayerControl.spotLightRange = 15f;
        } else if(cosmeticID == 13){
            spriteRender.sprite = cosmeticSprites[13];
        }
    }

    public void DropCosmetic(){
        Debug.Log("Dropping Cosmetic");
        transform.parent = null;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if(rb == null){
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        rb.gravityScale = 1f;

        Vector2 force = new Vector2(Random.Range(-200f, 200f), Random.Range(200f, 400f));
        rb.AddForce(force);
        rb.angularVelocity = Random.Range(-360f, 360f);
    }
}
