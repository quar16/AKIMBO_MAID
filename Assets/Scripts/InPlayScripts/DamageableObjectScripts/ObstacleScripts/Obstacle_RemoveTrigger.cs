using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_RemoveTrigger : Obstacle
{
    void Start()
    {
        int index = (int)(transform.position.x % 17);

        MapManager.Instance.AddRemoveIndex(index);
    }
}
