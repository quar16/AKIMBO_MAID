using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DraggableObject : MonoBehaviour
{
    protected bool isDragging = false;

    [HideInInspector]
    public Vector2 offset;
    [HideInInspector]
    public Vector3 cameraOffset;
    //[HideInInspector]
    public Vector2Int gridIndex;
    [HideInInspector]
    public List<float> customValues;


    public int prefabId;

    public virtual void Init()
    {
        gridIndex = TileGrid.Instance.SnapToGrid(transform);
    }

    protected virtual void OnMouseOver()
    {
        // 우클릭인 경우만 버튼을 표시
        if (Input.GetMouseButtonDown(1))
        {
#if UNITY_EDITOR
            Selection.activeObject = gameObject;
            EditorGUIUtility.PingObject(gameObject);
#endif
            DraggableObjectDeleteButton.ShowButtonGlobal(this);
        }
        else if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            cameraOffset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cameraOffset.z = 0;
        }
    }

    protected virtual void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + cameraOffset;
            mousePosition.z = 0;
            transform.position = mousePosition;
        }
    }

    protected virtual void OnMouseUp()
    {
        if (isDragging)
        {
            isDragging = false;
            gridIndex = TileGrid.Instance.SnapToGrid(transform);
        }
    }

    public virtual List<float> GetCustomValue()
    {
        return null;
    }
}
