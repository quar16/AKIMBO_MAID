using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableObject_NarrativeTrigger : DraggableObject
{
    public int narrativeIndex;
    public GameMode afterGameMode;

    public override void Init()
    {
        gridIndex = TileGrid.Instance.SnapToGrid(transform);
        if (customValues == null)
        {
            customValues = new List<float> { 0, 0 };
        }
        else
        {
            narrativeIndex = (int)customValues[0];
            afterGameMode = (GameMode)customValues[1];
        }
    }

    public override List<float> GetCustomValue()
    {
        customValues[0] = narrativeIndex;
        customValues[1] = (float)afterGameMode;

        return customValues;
    }
}
