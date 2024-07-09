using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableObject_MainPlatform : DraggableObject
{
    public override void Init()
    {
        gridIndex = TileGrid.Instance.SnapToGrid(transform);
        if (customValues == null)
        {
            customValues = new List<float> { 1, 0 };
        }
        else
        {
            sidePlatform.transform.position = transform.position + Vector3.right * customValues[0];
            isHanging = customValues[1] == 1;
        }
        sidePlatform.Init();
    }

    public bool isHanging = false;

    public DraggableObject_SidePlatform sidePlatform;

    //0:ÇÃ·§Æû ±æÀÌ /1:ÃµÀå,¹Ù´Ú ¿¬°á
    public override List<float> GetCustomValue()
    {
        customValues[0] = sidePlatform.gridIndex.x - gridIndex.x;
        customValues[1] = isHanging ? 1 : 0;

        return customValues;
    }
}
