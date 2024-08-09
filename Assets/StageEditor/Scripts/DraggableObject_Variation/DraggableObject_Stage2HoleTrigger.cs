using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockStateD { None, Broken }

public class DraggableObject_Stage2HoleTrigger : DraggableObject
{
    [SerializeField]
    BlockStateD blockState;

    public override void Init()
    {
        gridIndex = TileGrid.Instance.SnapToGrid(transform);

        if (customValues == null)
        {
            customValues = new List<float> { 0 };
        }
        else
        {
            blockState = (BlockStateD)customValues[0];
        }
    }

    public override List<float> GetCustomValue()
    {
        customValues[0] = (float)blockState;

        return customValues;
    }

}
