using UnityEngine;

public class EcholocationRay : MonoBehaviour
{
    public float speed = 20f;
    public float lifetime = 2f;
    
    public GameObject lightPrefab;
    public AudioSource source;
    public AudioClip soundHit;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("hazard"))
        {
            Debug.Log("Echolocation ray hit: " + collision.gameObject.name);
            source.PlayOneShot(soundHit);
            
            Vector2 hitPoint = collision.ClosestPoint(transform.position);
            
            if (lightPrefab != null)
            {
                GameObject lightInstance = Instantiate(lightPrefab, hitPoint, Quaternion.identity);
                lightInstance.transform.SetParent(collision.transform);
            }
            
            Destroy(gameObject);
        }
    }
}
