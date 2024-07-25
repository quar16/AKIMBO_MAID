using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMover : MonoBehaviour
{
    public GameObject backgroundPrefab;
    public float bgLength;
    public float ratio = 0.7f;

    int bgCount = 3;

    int nowCameraIndex = 0;

    public List<KeyValuePair<int, GameObject>> backGroundDic = new();

    public List<int> removeIndex = new();

    IEnumerator moveBackGround;
    IEnumerator moveBackGroundByPlayer;

    bool isMove = false;

    public void Init()
    {
        isMove = true;

        InitBackGround();

        nowCameraIndex = 0;

        moveBackGround = MoveBackGround();
        moveBackGroundByPlayer = MoveBackGroundByPlayer();
        StartCoroutine(moveBackGround);
        StartCoroutine(moveBackGroundByPlayer);
    }

    public void CleanUp()
    {
        isMove = false;

        StopCoroutine(moveBackGround);
        StopCoroutine(moveBackGroundByPlayer);

        transform.position = Vector3.zero;

        foreach (var v in backGroundDic)
            Destroy(v.Value);

        removeIndex.Clear();
        backGroundDic.Clear();
    }

    public void InitBackGround()
    {
        for (int i = -1; i < bgCount - 1; i++)
        {
            GameObject bg = Instantiate(backgroundPrefab, transform);
            bg.transform.localPosition = bgLength * i * Vector3.right;

            backGroundDic.Add(new KeyValuePair<int, GameObject>(i, bg));
        }
    }

    public IEnumerator MoveBackGround()
    {
        int lastCameraIndex = nowCameraIndex;
        while (isMove)
        {
            nowCameraIndex = (int)((CameraController.Instance.cameraT.position.x - transform.position.x + bgLength * 0.5f) / 17f);
            if (lastCameraIndex != nowCameraIndex)
            {
                int fromIndex = 2 * lastCameraIndex - nowCameraIndex;
                int toIndex = 2 * nowCameraIndex - lastCameraIndex;

                int listIndex = backGroundDic.FindIndex(x => x.Key == fromIndex);

                if (listIndex != -1)
                {
                    GameObject bg = backGroundDic[listIndex].Value;
                    bg.transform.localPosition = bgLength * toIndex * Vector3.right;
                    backGroundDic[listIndex] = new KeyValuePair<int, GameObject>(toIndex, bg);

                    bg.SetActive(!removeIndex.Contains(toIndex));
                }

                lastCameraIndex = nowCameraIndex;
            }
            yield return PlayTime.ScaledNull;
        }
    }


    public IEnumerator MoveBackGroundByPlayer()
    {
        float cameraLastPosX, cameraNowPosX;
        while (isMove)
        {
            cameraLastPosX = CameraController.Instance.cameraT.position.x;
            yield return PlayTime.ScaledNull;
            cameraNowPosX = CameraController.Instance.cameraT.position.x;

            float deltaX = cameraNowPosX - cameraLastPosX;

            transform.position += deltaX * ratio * Vector3.right;
        }
    }
}