using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class PlayerControl : MonoBehaviour
{
    [Header("References")]
    public Rigidbody2D player;
    public GameObject obstacleSpawner;
    public TextMeshProUGUI gamePause;
    public SpriteRenderer spriteRender;
    public Light2D spotLight;
    public Rigidbody2D rigidbody;
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

    public static int health = 1;
    public static int pointIncreaseRate = 1;
    public static float spotLightRange = 10f;
    public static float spotLightIntensity = .5f;
    public static Color spotLightColor = new Color32(255, 0, 0, 255);
    public static float spotLightOuterAngle = 102f;
    public static float spotlightInnerAngle = 0f;
    public static float gravityMultiplier = 1.75f;
    public static float abilityCooldownTime = 3f;
    public static bool isDead = false;
    public static bool gameGoing = false;
    public static bool spawned = false;
    public int score = 0;

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
        player.simulated = true;
        if (obstacleSpawner != null && !spawned)
            {
                Instantiate(obstacleSpawner, Vector3.zero, Quaternion.identity);
                spawned = true;
                //Spotlight stuff
                spotLight.intensity = spotLightIntensity;
                spotLight.color = spotLightColor;
                spotLight.pointLightInnerRadius = spotLightRange - 2f; spotLight.pointLightOuterRadius = spotLightRange;
                spotLight.pointLightOuterAngle = spotLightOuterAngle; spotLight.pointLightInnerAngle = spotlightInnerAngle;
                rigidbody.gravityScale = gravityMultiplier;
                jumpPower = jumpingPower;
            } else if (!spawned) {
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
            SceneManager.LoadSceneAsync("Level");
            isDead = false;
            spawned = false;
            score = 0;
            gameGoing = false;
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            gameGoing = !gameGoing;
        }
        else if (Input.GetKey(KeyCode.E) && Time.time - timeSinceLastEcholocation >= coolDownTime && !isDead && gameGoing)
        {
            Instantiate(echolocationProjectile, transform.position, transform.rotation);
            timeSinceLastEcholocation = Time.time;
            source.PlayOneShot(batScreech);
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
            health--;
            if (health <= 0)
            {
                Debug.Log("BLAM!");
                isDead = true;

                player.linearVelocity = Vector2.zero;
                player.simulated = false;

                source.PlayOneShot(deathAudio);
                animator.SetTrigger("Player_Death");

                if (deathParticles != null)
                {
                    deathParticles.Play();
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
            Debug.Log("+1");
            source.PlayOneShot(rewardAudio);
            score += pointIncreaseRate;
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
