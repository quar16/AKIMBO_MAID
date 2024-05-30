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

        Vector2 localPoint = screenPoint - new Vector2(Screen.width, Screen.height) * 0.5f + Vector2.one * 308;

        button.SetActive(true);
        showedFrame = true;
        rectTransform.anchoredPosition = localPoint;
    }

    public void DeleteDraggableObject()
    {
        Destroy(target.gameObject);
        button.SetActive(false);
    }
}
