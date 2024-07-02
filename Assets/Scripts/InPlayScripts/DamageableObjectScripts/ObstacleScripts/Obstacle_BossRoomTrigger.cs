using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_BossRoomTrigger : Obstacle
{
    public void Start()
    {
        StageManager.Instance.CallBossRoom();
    }
}
