using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Echo : MonoBehaviour
{
    public GameObject obj;
    public CircleCollider2D colliderObj;

    public float radius = 3f;
    public float scale = 0.1f;

    private bool playing;
    private Collider2D[] colliders;

    public void Called()
    {
        if (!playing)
        {
            StartCoroutine(Expand());
            Debug.Log("test1");
        }
    }

    public IEnumerator Expand()
    {
        Debug.Log("test2");

        playing = true;

        while (transform.localScale.x <= radius)
        {
            transform.localScale += new Vector3(scale, scale, 0f) * Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.25f);

        transform.localScale = Vector3.zero;

        playing = false;
    }

    void Update()
    {
        colliderObj.radius = Mathf.Max(transform.localScale.x, transform.localScale.y);

        float currentRadius = colliderObj.bounds.extents.magnitude;

        Collider2D[] overlappingColliders = Physics2D.OverlapCircleAll(colliderObj.bounds.center, currentRadius);

        Debug.Log(currentRadius);

        foreach (Collider2D overlappingCollider in overlappingColliders)
        {
            if (overlappingCollider.CompareTag("Enemy"))
            {
                EnemyController enemyController = overlappingCollider.GetComponent<EnemyController>();
                enemyController.spotted = true;
                Debug.Log("Test");
            }
        }
    }
}
