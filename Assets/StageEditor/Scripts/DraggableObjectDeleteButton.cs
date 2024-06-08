using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableObjectDeleteButton : MonoBehaviour
{
    static DraggableObjectDeleteButton instance;
    RectTransform rectTransform { get { return GetComponent<RectTransform>(); } }

    public RectTransform canvasRectTransform;
    public GameObject button;

    DraggableObject target;

    bool showedFrame = false;

    private void Awake()
    {
        instance = this;
    }

    public void Update()
    {
        if (!showedFrame && button.activeSelf && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)))
        {
            Vector2 clickPosition = Input.mousePosition;

            if (!RectTransformUtility.RectangleContainsScreenPoint(button.GetComponent<RectTransform>(), clickPosition))
            {
                button.SetActive(false);
            }
        }

        if (showedFrame)
            showedFrame = false;
    }

    public static void ShowButtonGlobal(DraggableObject _target)
    {
        instance.ShowButton(_target);
    }

    public void ShowButton(DraggableObject _target)
    {
        target = _target;

        Vector2 screenPoint = Camera.main.WorldToScreenPoint(_target.transform.position);

        // 스크린 좌표를 캔버스 로컬 좌표로 변환
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform.parent as RectTransform, screenPoint, null, out localPoint);
        
        // 필요한 경우 위치 조정 (여기서는 (154, 154)만큼 이동)
        localPoint += Vector2.one * 154;

        button.SetActive(true);
        showedFrame = true;
        rectTransform.anchoredPosition = localPoint;
    }

    public void DeleteDraggableObject()
    {
        PrefabLoader.Instance.UnspawnEntity(target);
        button.SetActive(false);
    }
}
