using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableObject_SidePlatform : DraggableObject
{
    public DraggableObject_MainPlatform mainPlatform;
    public Transform middlePlatform;
    public int gap;

    public override void Init()
    {
        gridIndex = TileGrid.Instance.SnapToGrid(transform);

        SetMiddlePlatform();
    }

    protected override void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            cameraOffset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cameraOffset.z = 0;
        }
    }

    protected override void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + cameraOffset;

            mousePosition.z = 0;
            mousePosition.y = transform.position.y;

            transform.position = mousePosition;

            SetMiddlePlatform();
        }
    }

    protected override void OnMouseUp()
    {
        base.OnMouseUp();
        SetMiddlePlatform();
    }

    void SetMiddlePlatform()
    {
        middlePlatform.position = (transform.position + Vector3.right + mainPlatform.transform.position) * 0.5f;

        float length = (transform.position.x - mainPlatform.transform.position.x - 1);
        middlePlatform.localScale = new Vector3(length + gap, 1, 1);
    }
}
