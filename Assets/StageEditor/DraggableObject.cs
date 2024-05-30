using UnityEngine;

public class DraggableObject : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    private TileGrid tileGrid;

    void Start()
    {
        tileGrid = FindObjectOfType<TileGrid>();
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
            offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            offset.z = 0;
        }
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            mousePosition.z = 0;
            transform.position = mousePosition;
        }
    }

    void OnMouseUp()
    {
        if (isDragging)
        {
            isDragging = false;
            Vector2 snappedPosition = tileGrid.SnapToGrid(transform.position);
            transform.position = new Vector3(snappedPosition.x, snappedPosition.y, transform.position.z);
        }
    }
}
