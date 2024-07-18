using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapIndex { stage1_bar, hiddenStage, stage1_Boss, stage2_Boss }

public class DraggableObject_MapTrigger : DraggableObject
{
    public MapIndex mapIndex;
    public int yOffset;

    public Transform mapArea;

    public override void Init()
    {
        gridIndex = TileGrid.Instance.SnapToGrid(transform);
        if (customValues == null)
        {
            customValues = new List<float> { 0, 0 };
        }
        else
        {
            mapIndex = (MapIndex)customValues[0];
            yOffset = (int)customValues[1];
        }
        SetMapArea();
    }

    protected override void OnMouseDrag()
    {
        base.OnMouseDrag();
        SetMapArea();
    }

    protected override void OnMouseUp()
    {
        base.OnMouseUp();
        SetMapArea();
    }

    public void SetMapArea()
    {
        mapArea.localPosition = new Vector3(8.5f - transform.position.x % 17, 2 - transform.position.y, 0);
    }

    public override List<float> GetCustomValue()
    {
        customValues[0] = (float)mapIndex;
        customValues[1] = yOffset;

        return customValues;
    }
}
