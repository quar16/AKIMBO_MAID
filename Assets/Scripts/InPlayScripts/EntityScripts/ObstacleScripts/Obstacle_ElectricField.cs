using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_ElectricField : Obstacle
{
    public GameObject electricEffect;

    public GameObject leftLeg;
    public GameObject rightLeg;

    public GameObject leftArm;
    public GameObject rightArm;

    public DamageArea damageArea;

    public override void Init(List<float> customData)
    {
        int platformLength = (int)customData[0];
        bool isHanging = customData[1] == 1;

        for (int i = 0; i <= platformLength; i++)
        {
            this.InstantiateChild(electricEffect, Vector3.right * i, Quaternion.identity);
        }

        if (isHanging)
        {
            this.InstantiateChild(leftArm, Vector3.zero, Quaternion.identity);
            this.InstantiateChild(rightArm, Vector3.right * platformLength, Quaternion.identity);
        }
        else
        {
            this.InstantiateChild(leftLeg, Vector3.zero, Quaternion.identity);
            this.InstantiateChild(rightLeg, Vector3.right * platformLength, Quaternion.identity);
        }

        damageArea.Init(new Vector3(platformLength * 0.5f, 0.6f, 0), new Vector2(platformLength, 0.1f));
    }
}
