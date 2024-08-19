using System.Collections;
using UnityEngine;

public class TempScript : MonoBehaviour
{
    private Collider2D myCollider;

    private void Start()
    {
        myCollider = GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        Collider2D[] overlappingColliders = Physics2D.OverlapBoxAll(myCollider.bounds.center, myCollider.bounds.size, 0f);

        foreach (Collider2D collider in overlappingColliders)
        {
            if (collider != myCollider) // 자기 자신과의 충돌은 무시
            {
                // 이 부분에서 OnTriggerStay2D와 동일한 로직을 수동으로 호출
                HandleTriggerStay(collider);
            }
        }
    }

    void HandleTriggerStay(Collider2D other)
    {
        // 여기에 OnTriggerStay2D와 유사한 로직을 작성
        Debug.Log("Trigger Stay with: " + other.name);
    }
}
