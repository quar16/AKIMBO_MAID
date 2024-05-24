using UnityEngine;

public class AngleCheck : MonoBehaviour
{
    public Transform transform1;
    public Transform transform2;
    public float targetAngle = 90f; // 목표 각도
    public float tolerance = 10f;   // 허용 가능한 각도 범위

    void Update()
    {
        Vector3 calcAngle = transform2.position - transform1.position;
        calcAngle.z = 0;


        // 두 트랜스폼 간의 각도 계산
        float angle = Vector3.SignedAngle(Vector3.right, calcAngle, Vector3.forward);



    }
}
