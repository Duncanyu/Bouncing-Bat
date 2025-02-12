using UnityEngine;

public class ObstaclesMovement : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime; //Only way I know how to move it as of right now
    }
}
