using UnityEngine;

public class AngleCheck : MonoBehaviour
{
    public Transform transform1;
    public Transform transform2;
    public float targetAngle = 90f; // ��ǥ ����
    public float tolerance = 10f;   // ��� ������ ���� ����

    void Update()
    {
        Vector3 calcAngle = transform2.position - transform1.position;
        calcAngle.z = 0;


        // �� Ʈ������ ���� ���� ���
        float angle = Vector3.SignedAngle(Vector3.right, calcAngle, Vector3.forward);



    }
}
