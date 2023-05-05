using System.Collections;
using UnityEngine;

public class Visibility : MonoBehaviour
{
    private Transform player;
    public float Timer;

    public Transform GetPlayerAxis => player;

    public bool isVisible = false;
    public SpriteRenderer spriteRenderer;
    CircleCollider2D circleCollider;

    bool vision()
    {
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        Vector2 playerPosition = new Vector2(player.position.x, player.position.y);
        Vector2 direction = playerPosition - position;
        Vector2 spreadOnSides = new Vector2(direction.y, -direction.x).normalized * circleCollider.radius;
        LayerMask mask = LayerMask.GetMask("Enemy", "Player", "Bullet");
        if (Physics2D.Raycast(position, direction, direction.magnitude, ~mask.value) &&
             Physics2D.Raycast(position + spreadOnSides, direction - spreadOnSides, (direction - spreadOnSides).magnitude, ~mask.value) &&
             Physics2D.Raycast(position - spreadOnSides, direction + spreadOnSides, (direction + spreadOnSides).magnitude, ~mask.value))
        {
            return false;
        }
        return true;
    }

    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        Timer = 0f;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>(); 
    }

    void FixedUpdate()
    {
        if (vision())
        {
            if (isVisible) { Timer += Time.deltaTime; }
            else { Timer = 0f; }
            isVisible = true;
            //spriteRenderer.enabled = true;
        }
        else
        {
            isVisible = false;
            //spriteRenderer.enabled = false;
        }
    }
}
