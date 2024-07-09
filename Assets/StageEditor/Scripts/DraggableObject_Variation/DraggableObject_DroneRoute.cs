using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class DraggableObject_DroneRoute : DraggableObject
{
    public DraggableObject_Drone drone;

    public Transform connectPoint;
    public LineRenderer connectLine;

    public int index;

    public override void Init()
    {
        gridIndex = TileGrid.Instance.SnapToGrid(transform);

        connectLine.positionCount = 2;
        CallConnectLineSet();
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
        base.OnMouseDrag();
        CallConnectLineSet();
    }

    protected override void OnMouseUp()
    {
        base.OnMouseUp();
        CallConnectLineSet();
    }

    void CallConnectLineSet()
    {
        drone.CallConnectLineSet(index);
    }

    public void SetConnectLine()
    {
        connectLine.SetPosition(1, connectPoint.position - transform.position);
    }
}
