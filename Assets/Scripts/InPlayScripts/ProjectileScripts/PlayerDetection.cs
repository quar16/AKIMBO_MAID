using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerDetection : MonoBehaviour
{
    [HideInInspector]
    public Enemy enemy;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerDetected();
        }
    }

    void OnPlayerDetected()
    {
        enemy.DetectPlayer();
    }
}
