using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_MapTrigger : Obstacle
{
    public override void Init(List<float> customData)
    {
        int index = (int)(transform.position.x / 17);

        MapManager.Instance.SpawnSpecialMap((MapIndex)customData[0], index);
    }
}
