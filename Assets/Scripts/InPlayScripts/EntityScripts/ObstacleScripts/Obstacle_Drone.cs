using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Obstacle_Drone : Obstacle
{
    public float speed = 1;
    public bool isRoundTrip = true;
    public List<Vector3> travelPoints = new();

    public GameObject explosion;

    public override void Init(List<float> customData)
    {
        speed = customData[0];
        isRoundTrip = customData[1] == 1;

        for (int i = 2; i < customData.Count; i += 2)
        {
            Vector3 v3 = new Vector3(customData[i], customData[i + 1], 0);
            travelPoints.Add(v3);
        }

        StartCoroutine(Traveling());
    }

    IEnumerator Traveling()
    {
        for (int i = 0; i < travelPoints.Count; i++)
        {
            Vector3 start = transform.position;
            Vector3 end = travelPoints[i];

            float maxDistance = Vector3.Distance(start, end);

            float moveTime = maxDistance / speed;

            float startTime = Time.time;

            while (Time.time < startTime + moveTime)
            {
                float t = (Time.time - startTime) / moveTime;

                transform.position = Vector3.Lerp(start, end, t);
                yield return PlayTime.ScaledNull;
            }

            transform.position = end;
        }

        if (isRoundTrip)
            StartCoroutine(Traveling());
    }

    public override void OnZeroHP()
    {
        this.Instantiate(explosion, CenterPoint, Quaternion.identity);
        OppositionEntityManager.Instance.DespawnEntity(this);
    }
}
