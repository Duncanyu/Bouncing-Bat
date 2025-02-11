using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public Rigidbody2D player_rigidbody;

    public int jump_power = 10;

    void Start()
    {
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            player_rigidbody.velocity = new Vector2(player_rigidbody.velocity.x, jump_power);
        }
    }
}
