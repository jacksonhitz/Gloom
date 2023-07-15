using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GameObject targetObject;
    public float killRadius = 3f;

    private Rigidbody2D rb;
    private bool sneaking;

    public float radius;

    public Echo echo;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            moveSpeed = 1f;
            sneaking = true;
        }
        else
        {
            moveSpeed = 5f;
            sneaking = false;

            if (rb.velocity.magnitude > 0)
            {
                echo.Called();
            }
        }

        if (Input.GetKey(KeyCode.Space))
        {
            Kill(transform.position, killRadius);
        }

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        rb.velocity = movement * moveSpeed;
    }

    private void Kill(Vector2 center, float killRadius)
    {
        Collider2D[] overlappingColliders = Physics2D.OverlapCircleAll(center, killRadius);

        foreach (Collider2D collider in overlappingColliders)
        {
            EnemyController enemy = collider.GetComponent<EnemyController>();
            if (enemy != null && enemy.gameObject != null)
            {
                Destroy(enemy.gameObject);
            }
        }
    }
}
