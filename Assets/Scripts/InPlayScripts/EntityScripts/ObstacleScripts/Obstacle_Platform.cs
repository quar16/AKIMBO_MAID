using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_Platform : Obstacle
{
    public GameObject platformBase;

    public GameObject leftLeg;
    public GameObject rightLeg;

    public GameObject armTop;
    public GameObject armMiddle;
    public GameObject armBottom;


    public override void Init(List<float> customData)
    {
        int platformLength = (int)customData[0];
        bool isHanging = customData[1] == 1;

        for (int i = 0; i <= platformLength; i++)
        {
            this.InstantiateChild(platformBase, Vector3.right * i, Quaternion.identity);
        }

        if (isHanging)
        {
            int height = 6 - 1 - (int)transform.position.y;
            for (int i = 0; i < height; i++)
            {
                GameObject prefab;
                if (i == 0)
                    prefab = armBottom;
                else if (i != height - 1)
                    prefab = armMiddle;
                else
                    prefab = armTop;

                InstantiateArm(prefab, 0, i);
                InstantiateArm(prefab, platformLength, i);
            }
        }
        else
        {
            this.InstantiateChild(leftLeg, Vector3.zero, Quaternion.identity);
            this.InstantiateChild(rightLeg, Vector3.right * platformLength, Quaternion.identity);
        }
    }

    public void InstantiateArm(GameObject prefab, int x, int y)
    {
        this.InstantiateChild(prefab, new Vector3(x, y, 0), Quaternion.identity);
    }
}
