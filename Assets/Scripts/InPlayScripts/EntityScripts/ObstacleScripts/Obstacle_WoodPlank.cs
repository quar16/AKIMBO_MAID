using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Obstacle_WoodPlank : Obstacle
{
    public List<Sprite> spriteList;
    public SpriteRenderer plankImage;
    public List<GameObject> plankParticles;

    public override void GetDamage(int damage)
    {
        ChangeHP(-damage);
        plankImage.sprite = spriteList[nowHP];
    }

    public override void OnZeroHP()
    {
        foreach (var v in plankParticles)
        {
            StartCoroutine(AnimatePlankParticle(v));
        }
    }


    public float gravity = 1;
    public float yPower = 1;
    public float xPower = 1;
    public float rotPower = 1;

    IEnumerator AnimatePlankParticle(GameObject particle)
    {
        particle.gameObject.SetActive(true);

        float ySpeed = Random.Range(1, 4f);
        float xSpeed = Random.Range(1, 3f) - ySpeed;
        float rotSpeed = Random.Range(100, 200) * (Random.Range(0, 2) * 2 - 1) * rotPower;


        while (particle.transform.position.y > -100)
        {
            particle.transform.position += new Vector3(xSpeed * xPower, ySpeed * yPower, 0) * PlayTime.Scale;
            ySpeed -= Time.deltaTime * gravity;
            particle.transform.eulerAngles += PlayTime.Scale * rotSpeed * Vector3.forward;
            yield return PlayTime.ScaledNull;
        }
        Destroy(particle);
    }
}
