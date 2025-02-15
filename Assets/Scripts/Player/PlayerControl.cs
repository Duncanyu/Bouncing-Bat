using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class PlayerControl : MonoBehaviour
{
    [Header("References")]
    public Rigidbody2D player;
    public GameObject obstacleSpawner;
    public TextMeshProUGUI gamePause;
    public SpriteRenderer spriteRender;
    public Light2D spotLight;
    public Rigidbody2D rigidbody;
    public CosmeticsControl cosmeticsControl; // Must be assigned in the Inspector
    private Collider2D playerCollider;

    [Header("Audio")]
    public AudioClip bounceAudio;
    public AudioClip deathAudio;
    public AudioClip rewardAudio;
    public AudioClip batScreech;
    public AudioClip hurtAudio;
    private AudioSource source;

    [Header("Sprites & Animations")]
    public Animator animator;

    [Header("Movements")]
    public int jumpPower = 10;
    public static int jumpingPower = 10;
    public float minRotation = -60f;
    public float maxRotation = 45f;
    public float angleMultiplier = 2f;

    [Header("Echolocation")]
    public GameObject echolocationProjectile;
    public float coolDownTime;
    private float timeSinceLastEcholocation = -Mathf.Infinity;

    [Header("Particles")]
    public ParticleSystem deathParticles;

    [Header("Damage Cooldown")]
    public float damageCooldown = 1f;
    private float lastDamageTime = -Mathf.Infinity;

    public static int health = 1;
    public static int pointIncreaseRate = 1;
    public static float spotLightRange = 10f;
    public static float spotLightIntensity = 0.5f;
    public static Color spotLightColor = new Color32(255, 0, 0, 255);
    public static float spotLightOuterAngle = 102f;
    public static float spotlightInnerAngle = 0f;
    public static float gravityMultiplier = 1.75f;
    public static float abilityCooldownTime = 3f;
    public static int abilityID = 1;
    public static bool isDead = false;
    public static bool gameGoing = false;
    public static bool spawned = false;
    public int score = 0;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        isDead = false;
        spawned = false;
        health = 1;
        score = 0;
    }

    void Start()
    {
        playerCollider = GetComponent<CircleCollider2D>();
        animator.SetTrigger("Idle");
        source = GetComponent<AudioSource>();
        spriteRender = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();

        player.simulated = false;
        coolDownTime = abilityCooldownTime;
        gameGoing = true;
    }

    void Update()
    {
        if (gameGoing)
            player.simulated = true;

        if (obstacleSpawner != null && !spawned)
        {
            Instantiate(obstacleSpawner, Vector3.zero, Quaternion.identity);
            spawned = true;
            spotLight.intensity = spotLightIntensity;
            spotLight.color = spotLightColor;
            spotLight.pointLightInnerRadius = spotLightRange - 2f;
            spotLight.pointLightOuterRadius = spotLightRange;
            spotLight.pointLightOuterAngle = spotLightOuterAngle;
            spotLight.pointLightInnerAngle = spotlightInnerAngle;
            rigidbody.gravityScale = gravityMultiplier;
            jumpPower = jumpingPower;
        }
        else if (!spawned)
        {
            return;
        }
        
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetMouseButtonDown(0)) && !isDead)
        {
            player.linearVelocity = new Vector2(player.linearVelocity.x, jumpPower);
            source.PlayOneShot(bounceAudio);
            animator.SetTrigger("Flap");
        }
        else if (Input.GetKeyDown(KeyCode.Space) && isDead)
        {
            isDead = false;
            spawned = false;
            score = 0;
            gameGoing = false;
            SceneManager.LoadSceneAsync("Level");
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            gameGoing = !gameGoing;
        }
        else if (Input.GetKey(KeyCode.E) && Time.time - timeSinceLastEcholocation >= coolDownTime && !isDead && gameGoing)
        {
            if (abilityID == 1)
            {
                Instantiate(echolocationProjectile, transform.position, transform.rotation);
                timeSinceLastEcholocation = Time.time;
                source.PlayOneShot(batScreech);
            }
            else if (abilityID == 2)
            {
            }
        }

        float yVelocity = player.linearVelocity.y;
        float targetAngle = Mathf.Clamp(yVelocity * angleMultiplier, minRotation, maxRotation);
        transform.rotation = Quaternion.Euler(0, 0, targetAngle);

        if (!gameGoing)
        {
            gamePause.text = "GAME PAUSED";
            player.simulated = false;
        }
        else if (isDead)
        {
            gamePause.text = "YOU LOST, PRESS SPACE TO RETRY";
            player.simulated = false;
            spawned = false;
        }
        else if (!isDead)
        {
            gamePause.text = "";
            player.simulated = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("hazard"))
        {
            if (Time.time - lastDamageTime < damageCooldown)
            {
                Debug.Log("Damage cooldown active. No additional damage taken.");
                return;
            }
            lastDamageTime = Time.time;

            health--;
            Debug.Log("Hazard hit: Health is now " + health);
            if (health <= 0)
            {
                Debug.Log("Player died.");
                isDead = true;
                player.linearVelocity = Vector2.zero;
                player.simulated = false;
                source.PlayOneShot(deathAudio);
                animator.SetTrigger("Player_Death");
                if (deathParticles != null)
                {
                    deathParticles.Play();
                }
                cosmeticsControl.DropCosmetic();
            }
            else if (health == 1 && (MainManager.cosmeticEquipped == 4 || MainManager.cosmeticEquipped == 5))
            {
                Debug.Log("Condition met to drop cosmetic.");
                if (cosmeticsControl != null)
                {
                    Debug.Log("Calling DropCosmetic()");
                    cosmeticsControl.DropCosmetic();
                }
                else
                {
                    Debug.Log("cosmeticsControl reference is null!");
                }
            }
            else
            {
                source.PlayOneShot(hurtAudio);
                StartCoroutine(FlashDamage());
            }
        }
        else if (collision.CompareTag("reward"))
        {
            Debug.Log("+1 reward");
            source.PlayOneShot(rewardAudio);
            score += pointIncreaseRate;
        }
        else if (collision.CompareTag("boundary"))
        {
            Debug.Log("Boundary hit.");
            isDead = true;
            player.linearVelocity = Vector2.zero;
            player.simulated = false;
            source.PlayOneShot(deathAudio);
            animator.SetTrigger("Player_Death");
            MainManager.balance += score;
            score = 0;
            if (deathParticles != null)
            {
                deathParticles.Play();
            }
            cosmeticsControl.DropCosmetic();
        }
    }

    IEnumerator FlashDamage()
    {
        int flashCount = 5;
        float flashDuration = 0.2f;
        Color originalColor = spriteRender.color;
        for (int i = 0; i < flashCount; i++)
        {
            spriteRender.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
            yield return new WaitForSeconds(flashDuration);
            spriteRender.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
            yield return new WaitForSeconds(flashDuration);
        }
        spriteRender.color = originalColor;
    }
}