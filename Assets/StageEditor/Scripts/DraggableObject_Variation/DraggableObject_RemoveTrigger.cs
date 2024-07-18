using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableObject_RemoveTrigger : DraggableObject
{
    public Transform removeArea;

    public override void Init()
    {
        gridIndex = TileGrid.Instance.SnapToGrid(transform);
        SetRemoveArea();
    }

    protected override void OnMouseDrag()
    {
        base.OnMouseDrag();
        SetRemoveArea();
    }

    protected override void OnMouseUp()
    {
        base.OnMouseUp();
        SetRemoveArea();
    }

    public void SetRemoveArea()
    {
        removeArea.localPosition = new Vector3(8.5f - transform.position.x % 17, 2 - transform.position.y, 0);
    }
}
