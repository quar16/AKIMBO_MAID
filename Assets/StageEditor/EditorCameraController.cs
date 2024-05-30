using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EditorCameraController : MonoBehaviour 
{
    private Vector3 lastMousePosition;
    private bool isDragging;

    public float dragSpeed = 2f; // ȭ�� �巡�� �ӵ�
    public float minCameraX = -10f; // ī�޶� �̵� ������ �ּ� x ��ġ
    public float maxCameraX = 10f; // ī�޶� �̵� ������ �ִ� x ��ġ
    public Scrollbar scrollbar; // ������ ��ũ�ѹ�

    private void Start()
    {
        // ��ũ�ѹ� ���� ����� ������ ī�޶� ��ġ�� ������Ʈ
        scrollbar.onValueChanged.AddListener(OnScrollbarValueChanged);
    }

    private void Update()
    {
        // ���콺 ���� ��ư�� ���� ���
        if (Input.GetMouseButtonDown(0) && !IsPointerOverUIObject() && !IsPointerOverCollider())
        {
            lastMousePosition = Input.mousePosition;
            isDragging = true;
        }
        // ���콺 ���� ��ư�� ���� ���
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        // �巡�� ���� ���
        if (isDragging)
        {
            // ���� ���콺 ��ġ�� ���� ���콺 ��ġ�� ���̸� ����Ͽ� ī�޶� �̵��� ���
            Vector3 delta = Input.mousePosition - lastMousePosition;
            float moveX = delta.x * dragSpeed * Time.deltaTime;

            // ī�޶��� x ��ġ�� �̵����� ���� ����
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x - moveX, minCameraX, maxCameraX),
                transform.position.y,
                transform.position.z
            );

            // ��ũ�ѹ� ��ġ ������Ʈ
            UpdateScrollbarPosition();

            // ���콺 ��ġ ����
            lastMousePosition = Input.mousePosition;
        }
    }

    // ��ũ�ѹ� ��ġ�� ī�޶� �̵��� ���� ������Ʈ
    private void UpdateScrollbarPosition()
    {
        // ī�޶� ��ġ�� 0�� 1 ������ ������ ��ȯ
        float normalizedCameraX = Mathf.InverseLerp(minCameraX, maxCameraX, transform.position.x);
        // ��ũ�ѹ� ��ġ ����
        scrollbar.value = normalizedCameraX;
    }

    // ��ũ�ѹ� ���� ����� �� ī�޶� ��ġ�� ������Ʈ
    private void OnScrollbarValueChanged(float value)
    {
        // ��ũ�ѹ� ���� 0�� 1 ������ ������ ���� ī�޶� ��ġ�� ��ȯ
        float cameraX = Mathf.Lerp(minCameraX, maxCameraX, value);
        transform.position = new Vector3(cameraX, transform.position.y, transform.position.z);
    }

    // ���콺 �����Ͱ� UI ��� ���� �ִ��� Ȯ��
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }

    // ���콺 �����Ͱ� 2D �ݶ��̴� ���� �ִ��� Ȯ��
    private bool IsPointerOverCollider()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return Physics2D.OverlapPoint(mousePosition) != null;
    }
}
