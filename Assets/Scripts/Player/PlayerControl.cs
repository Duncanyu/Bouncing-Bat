using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [Header("Rigidbody & Collidors")]
    public Rigidbody2D player;
    private Collider2D playerCollider;

    [Header("Audio")]
    public AudioClip bounceAudio;
    public AudioClip deathAudio;
    public AudioClip rewardAudio;
    private AudioSource source;

    [Header("Sprites & Animations")]
    public Animator animator;

    [Header("Movements")]
    public int jumpPower = 10;
    public float minRotation = -60f;
    public float maxRotation = 45f;
    public float angleMultiplier = 2f;

    private bool isDead = false;

    public int score = 0;

    void Start()
    {
        playerCollider = GetComponent<CircleCollider2D>();

        animator.SetTrigger("Idle");

        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetMouseButtonDown(0)) && !isDead){
            player.linearVelocity = new UnityEngine.Vector2(player.linearVelocity.x, jumpPower);
            source.PlayOneShot(bounceAudio);
            animator.SetTrigger("Flap");
            // StartCoroutine(Flap());
        }

        float yVelocity = player.linearVelocity.y;
        float targetAngle = Mathf.Clamp(yVelocity * angleMultiplier, minRotation, maxRotation);
        transform.rotation = UnityEngine.Quaternion.Euler(0, 0, targetAngle);
    }

    // IEnumerator Flap(){
    //     animator.SetTrigger("Flap");

    //     yield return new WaitForSeconds(1.75f);
    //     animator.SetTrigger("Idle");
    // }

    void OnTriggerEnter2D(Collider2D collision){
        if (collision.CompareTag("hazard")){
            Debug.Log("BLAM!");
            isDead = true;

            player.linearVelocity = UnityEngine.Vector2.zero;
            player.simulated = false;

            source.PlayOneShot(deathAudio);
            animator.SetTrigger("Player_Death");
        } else if (collision.CompareTag("reward")) {
            Debug.Log("+1");
            source.PlayOneShot(rewardAudio);
            score ++;
        }
    }
}
