using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class NarrativeFunction_Stage1_Bar : NarrativeFunction
{
    public Animator elevatorDoor;
    public Animator elevatorFrame;
    public Animator shelf;
    public Animator desk;

    public Animator bartender;

    public GameObject wall;
    public List<GameObject> wallParticles;

    protected override IEnumerator Process(string name)
    {
        switch (name)
        {
            case "PlayerEnter":
                yield return PlayerEnter();
                break;
            case "BartenderShoot":
                yield return BartenderShoot();
                break;
            case "WallBroken":
                yield return WallBroken();
                break;
        }


        yield break;
    }

    public IEnumerator PlayerEnter()
    {
        elevatorDoor.Play("ElevatorDoorOpen");
        elevatorFrame.Play("ElevatorFrameOpen");

        yield return PlayTime.ScaledWaitForSeconds(1);
        elevatorDoor.GetComponent<SpriteRenderer>().sortingOrder = 4;

        elevatorDoor.Play("ElevatorDoorClose");
        elevatorFrame.Play("ElevatorFrameClose");
    }

    public IEnumerator BartenderShoot()
    {
        bartender.Play("Bartender_Attack");
        shelf.Play("ShelfBroken");
        desk.Play("DeskBroken");

        PlayerManager.Instance.Animator.Play("Nar_Slide_Start");

        Transform playerT = PlayerManager.Instance.player.transform;

        Vector3 startV = playerT.transform.position;
        Vector3 endV = startV + Vector3.right * 13;

        float sTime = Time.time;
        float duration = 2f;

        while (sTime + duration > Time.time)
        {
            float t = (Time.time - sTime) / duration;
            float st = Mathf.Sin(t * Mathf.PI * 0.5f);

            playerT.position = Vector3.Lerp(startV, endV, st);

            yield return PlayTime.ScaledNull;
        }

        PlayerManager.Instance.Animator.Play("Nar_Slide_End");

        //player slide

        yield break;
    }

    public IEnumerator WallBroken()
    {
        PlayerManager.Instance.Animator.Play("Nar_Run_Fire");

        wall.SetActive(false);

        foreach (var v in wallParticles)
            StartCoroutine(AnimatePlankParticle(v));

        PlayerManager.Instance.playerMoveController.InputDirection(Direction.RIGHT);
        yield return PlayTime.ScaledWaitForSeconds(1);
        PlayerManager.Instance.Animator.Play("DefaultMove");

        yield break;
    }


    public float gravity = 1;
    public float yPower = 1;
    public float xPower = 1;
    public float rotPower = 1;

    IEnumerator AnimatePlankParticle(GameObject particle)
    {
        particle.gameObject.SetActive(true);

        float ySpeed = Random.Range(1, 4f);
        float xSpeed = Random.Range(7, 10f) - ySpeed;
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
