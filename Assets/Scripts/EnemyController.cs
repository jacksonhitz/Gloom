using UnityEngine;
using System.Collections;

public enum EnemyState
{
    Patrol,
    Chase
}

public class EnemyController : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    private int currentPatrolIndex;
    private Transform targetPoint;
    private Rigidbody2D rb;

    public float fovAngle = 45f;
    public Transform fovPoint;
    public float range = 8;

    private GameObject playerObject;
    public Player player;
    public bool spotted;

    private EnemyState currentState;

    private void Start()
    {
        currentPatrolIndex = 0;
        targetPoint = patrolPoints[currentPatrolIndex];
        rb = GetComponent<Rigidbody2D>();
        playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.GetComponent<Player>();

        currentState = EnemyState.Patrol;
    }

    private void Update()
    {
        if (WithinRadius())
        {
           
        }

        switch (currentState)
        {
            case EnemyState.Patrol:
                if (spotted)
                {
                    currentState = EnemyState.Chase;
                    StopAllCoroutines(); // Stop any ongoing coroutines
                    StartCoroutine(ChasePlayer());
                }
                else
                {
                    Patrol();
                }
                break;
            case EnemyState.Chase:
                if (!spotted)
                {
                    currentState = EnemyState.Patrol;
                    StopAllCoroutines(); // Stop the ChasePlayer coroutine
                }
                break;
        }

        InLight();
    }

    private void Patrol()
    {
        Vector2 movementDirection = (targetPoint.position - transform.position).normalized;
        rb.transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);

        if (rb.transform.position == targetPoint.position)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            targetPoint = patrolPoints[currentPatrolIndex];
        }

        if (movementDirection != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle - 90f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    private IEnumerator ChasePlayer()
    {
        float startTime = Time.time;
        bool isChasing = true;

        while (isChasing)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            rb.transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);

            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle - 90f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (!WithinRadius() && Time.time - startTime >= 5f)
            {
                isChasing = false;
            }

            yield return null;
        }

        spotted = false;
        currentState = EnemyState.Patrol;
    }

    public void InLight()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                Vector2 directionToCollider = collider.transform.position - transform.position;
                float angleToCollider = Vector2.Angle(transform.up, directionToCollider);

                if (angleToCollider <= fovAngle / 2f)
                {
                    // Fire a raycast to check for obstacles between the FOV point and the player
                    RaycastHit2D hit = Physics2D.Raycast(fovPoint.position, directionToCollider, range);
                    if (hit.collider != null && hit.collider.CompareTag("Player"))
                    {
                        Debug.Log("Player detected: " + collider.name);
                        // Add your desired logic for handling the detected player collider here
                        spotted = true;
                    }
                }
            }
        }
    }

    public bool WithinRadius()
    {
        float distance = Vector2.Distance(player.transform.position, transform.position);
        return distance <= range;
    }
}
