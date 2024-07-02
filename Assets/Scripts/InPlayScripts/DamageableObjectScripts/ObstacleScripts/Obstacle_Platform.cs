using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_Platform : Obstacle
{
    public GameObject leftPlatform;
    public GameObject middlePlatform;
    public GameObject rightPlatform;

    public override void Init(List<float> customData)
    {
        int platformLength = (int)customData[0];
        bool isHanging = customData[1] == 1;

        GameObject lp = Instantiate(leftPlatform, transform);
        lp.transform.localPosition = Vector3.zero;
        lp.SetActive(true);

        for (int i = 1; i < platformLength; i++)
        {
            GameObject mp = Instantiate(middlePlatform, transform);
            mp.transform.localPosition = Vector3.right * i;
            mp.SetActive(true);
        }

        GameObject rp = Instantiate(rightPlatform, transform);
        rp.transform.localPosition = Vector3.right * platformLength;
        rp.SetActive(true);

        if (isHanging)
        {
            //
        }
    }
}
