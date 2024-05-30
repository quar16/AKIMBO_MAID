using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EditorCameraController : MonoBehaviour 
{
    private Vector3 lastMousePosition;
    private bool isDragging;

    public float dragSpeed = 2f; // 화면 드래그 속도
    public float minCameraX = -10f; // 카메라 이동 가능한 최소 x 위치
    public float maxCameraX = 10f; // 카메라 이동 가능한 최대 x 위치
    public Scrollbar scrollbar; // 연동할 스크롤바

    private void Start()
    {
        // 스크롤바 값이 변경될 때마다 카메라 위치를 업데이트
        scrollbar.onValueChanged.AddListener(OnScrollbarValueChanged);
    }

    private void Update()
    {
        // 마우스 왼쪽 버튼이 눌린 경우
        if (Input.GetMouseButtonDown(0) && !IsPointerOverUIObject() && !IsPointerOverCollider())
        {
            lastMousePosition = Input.mousePosition;
            isDragging = true;
        }
        // 마우스 왼쪽 버튼이 떼진 경우
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        // 드래그 중인 경우
        if (isDragging)
        {
            // 현재 마우스 위치와 이전 마우스 위치의 차이를 계산하여 카메라 이동량 계산
            Vector3 delta = Input.mousePosition - lastMousePosition;
            float moveX = delta.x * dragSpeed * Time.deltaTime;

            // 카메라의 x 위치를 이동량에 따라 조절
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x - moveX, minCameraX, maxCameraX),
                transform.position.y,
                transform.position.z
            );

            // 스크롤바 위치 업데이트
            UpdateScrollbarPosition();

            // 마우스 위치 갱신
            lastMousePosition = Input.mousePosition;
        }
    }

    // 스크롤바 위치를 카메라 이동에 따라 업데이트
    private void UpdateScrollbarPosition()
    {
        // 카메라 위치를 0과 1 사이의 값으로 변환
        float normalizedCameraX = Mathf.InverseLerp(minCameraX, maxCameraX, transform.position.x);
        // 스크롤바 위치 설정
        scrollbar.value = normalizedCameraX;
    }

    // 스크롤바 값이 변경될 때 카메라 위치를 업데이트
    private void OnScrollbarValueChanged(float value)
    {
        // 스크롤바 값을 0과 1 사이의 값에서 실제 카메라 위치로 변환
        float cameraX = Mathf.Lerp(minCameraX, maxCameraX, value);
        transform.position = new Vector3(cameraX, transform.position.y, transform.position.z);
    }

    // 마우스 포인터가 UI 요소 위에 있는지 확인
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }

    // 마우스 포인터가 2D 콜라이더 위에 있는지 확인
    private bool IsPointerOverCollider()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return Physics2D.OverlapPoint(mousePosition) != null;
    }
}
