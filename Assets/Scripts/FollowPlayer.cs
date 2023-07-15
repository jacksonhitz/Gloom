using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public float followSpeed = 20f;
    public float shiftX;
    public float shiftY;

    private void Update()
    {
        if (player != null)
        {
            Vector3 targetPosition = new Vector3(player.position.x + shiftX, player.position.y + shiftY, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }
}
