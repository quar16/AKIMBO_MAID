using System.Collections.Generic;
using UnityEngine;

public class DraggableObject : MonoBehaviour
{
    private bool isDragging = false;
    private TileGrid tileGrid;

    public Vector2 offset;
    public Vector3 cameraOffset;
    public Vector2Int gridIndex;
    public int prefabId;

    public void Init()
    {
        tileGrid = FindObjectOfType<TileGrid>();
        gridIndex = tileGrid.SnapToGrid(transform);
    }

    void OnMouseOver()
    {
        // 우클릭인 경우만 버튼을 표시
        if (Input.GetMouseButtonDown(1))
        {
            DraggableObjectDeleteButton.ShowButtonGlobal(this);
        }
        else if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            cameraOffset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cameraOffset.z = 0;
        }
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + cameraOffset;
            mousePosition.z = 0;
            transform.position = mousePosition;
        }
    }

    void OnMouseUp()
    {
        if (isDragging)
        {
            isDragging = false;
            gridIndex = tileGrid.SnapToGrid(transform);
        }
    }

    public virtual List<float> GetCustomValue()
    {
        return null;
    }
}
