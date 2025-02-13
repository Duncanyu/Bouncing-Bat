using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    [Header("References")]
    public Rigidbody2D player;
    public GameObject obstacleSpawner;
    public TextMeshProUGUI gamePause;
    private Collider2D playerCollider;

    [Header("Audio")]
    public AudioClip bounceAudio;
    public AudioClip deathAudio;
    public AudioClip rewardAudio;
    public AudioClip batScreech;
    private AudioSource source;

    [Header("Sprites & Animations")]
    public Animator animator;

    [Header("Movements")]
    public int jumpPower = 10;
    public float minRotation = -60f;
    public float maxRotation = 45f;
    public float angleMultiplier = 2f;

    [Header("Echolocation")]
    public GameObject echolocationProjectile;
    public float coolDownTime;
    private float timeSinceLastEcholocation = -Mathf.Infinity;

    [Header("Particles")]
    public ParticleSystem deathParticles;

    public static bool isDead = false;
    public static bool gameGoing = false;
    public static bool spawned = false;

    public int score = 0;

    void Start()
    {
        playerCollider = GetComponent<CircleCollider2D>();
        animator.SetTrigger("Idle");
        source = GetComponent<AudioSource>();
        player.simulated = false;
        gameGoing = true;
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetMouseButtonDown(0)) && !isDead){
                player.simulated = true;
                gameGoing = true;
                
                if (obstacleSpawner != null && !spawned)
                {
                    Instantiate(obstacleSpawner, Vector3.zero, Quaternion.identity);
                    spawned = true;
                }
                else if(!spawned)
                {
                    new GameObject("Spawned Object");
                    spawned = true;
                }
            
            player.linearVelocity = new Vector2(player.linearVelocity.x, jumpPower);
            source.PlayOneShot(bounceAudio);
            animator.SetTrigger("Flap");
        } else if(Input.GetKeyDown(KeyCode.Space) && isDead){
            SceneManager.LoadSceneAsync("Level");
            isDead = false;
            spawned = false;
            score = 0;
            gameGoing = false;
        } else if (Input.GetKeyDown(KeyCode.P)) {
            gameGoing = !gameGoing;
        } else if (Input.GetKey(KeyCode.E) && Time.time - timeSinceLastEcholocation >= coolDownTime && !isDead && gameGoing) {
            Instantiate(echolocationProjectile, transform.position, transform.rotation);
            timeSinceLastEcholocation = Time.time;
            source.PlayOneShot(batScreech);
        }

        float yVelocity = player.linearVelocity.y;
        float targetAngle = Mathf.Clamp(yVelocity * angleMultiplier, minRotation, maxRotation);
        transform.rotation = Quaternion.Euler(0, 0, targetAngle);

        if (!gameGoing){
            gamePause.text = "GAME PAUSED";
            player.simulated = false;
        } else if (isDead){
            gamePause.text = "YOU LOST, PRESS SPACE TO RETRY";
            player.simulated = false;
        }else if (!isDead){
            gamePause.text = "";
            player.simulated = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("hazard"))
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
        else if (collision.CompareTag("reward"))
        {
            Debug.Log("+1");
            source.PlayOneShot(rewardAudio);
            score++;
        }
    }
}
