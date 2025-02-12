using UnityEngine;

public class SeamlessParallax : MonoBehaviour
{
    public float scrollSpeed = 1f;
    
    private float spriteWidth;
    private Transform clone;

    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        spriteWidth = sr.bounds.size.x;
        
        GameObject cloneGO = Instantiate(gameObject, transform.parent);
        clone = cloneGO.transform;
        
        clone.position = new Vector3(transform.position.x + spriteWidth, transform.position.y, transform.position.z);
        
        SeamlessParallax cloneScript = cloneGO.GetComponent<SeamlessParallax>();
        if (cloneScript != null)
        {
            cloneScript.enabled = false;
        }
    }

    void Update()
    {
        transform.position += Vector3.left * scrollSpeed * Time.deltaTime;
        clone.position += Vector3.left * scrollSpeed * Time.deltaTime;

        if (transform.position.x <= -spriteWidth)
        {
            transform.position = new Vector3(clone.position.x + spriteWidth, transform.position.y, transform.position.z);
        }
        if (clone.position.x <= -spriteWidth)
        {
            clone.position = new Vector3(transform.position.x + spriteWidth, clone.position.y, clone.position.z);
        }
    }
}
