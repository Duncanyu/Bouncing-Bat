using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target and Follow Settings")]
    public Transform target;
    public float followRatio = 0.02f;
    public float lerpSpeed = 5f;

    [Header("Clamp Settings")]
    public Vector2 minClamp;
    public Vector2 maxClamp;

    private Vector3 offset;

    void Start()
    {
        if (target != null)
        {
            offset = transform.position - target.position;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;
        Vector3 desiredPos = transform.position;
        desiredPos.x += (target.position.x - (transform.position.x - offset.x)) * followRatio;
        desiredPos.y += (target.position.y - (transform.position.y - offset.y)) * followRatio;
        desiredPos.z = transform.position.z;  

        desiredPos.x = Mathf.Clamp(desiredPos.x, minClamp.x, maxClamp.x);
        desiredPos.y = Mathf.Clamp(desiredPos.y, minClamp.y, maxClamp.y);

        transform.position = Vector3.Lerp(transform.position, desiredPos, Time.deltaTime * lerpSpeed);
    }
}
