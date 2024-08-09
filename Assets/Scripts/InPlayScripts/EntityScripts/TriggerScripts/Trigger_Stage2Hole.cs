using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Stage2Hole : Obstacle
{
    BlockStateD blockState;

    public override void Init(List<float> customData)
    {
        blockState = (BlockStateD)customData[0];
        int index = (int)(transform.position.x - 8);

        MapManager.Instance.AddRemoveIndex_Stage2(index, blockState);
    }
}
