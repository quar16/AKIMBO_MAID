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

    List<KeyValuePair<int, GameObject>> backGroundDic = new();

    public List<int> removeIndex = new();

    IEnumerator moveBackGround;

    private void Start()
    {
        Initiate();
    }

    public void Initiate()
    {
        InitBackGround();

        nowCameraIndex = 0;

        moveBackGround = MoveBackGround();
        StartCoroutine(moveBackGround);
        StartCoroutine(MoveBackGroundByPlayer());
    }

    public void InitBackGround()
    {
        for (int i = -1; i < bgCount - 1; i++)
        {
            GameObject bg = Instantiate(backgroundPrefab, bgLength * i * Vector3.right, Quaternion.identity, transform);

            backGroundDic.Add(new KeyValuePair<int, GameObject>(i, bg));
        }
    }

    public IEnumerator MoveBackGround()
    {
        int lastCameraIndex = nowCameraIndex;
        while (true)
        {
            nowCameraIndex = (int)((CameraController.Instance.cameraT.position.x - transform.position.x + bgLength * 0.5f) / 17f);
            if (lastCameraIndex != nowCameraIndex)
            {
                int fromIndex = 2 * lastCameraIndex - nowCameraIndex;
                int toIndex = 2 * nowCameraIndex - lastCameraIndex;

                int listIndex = backGroundDic.FindIndex(x => x.Key == fromIndex);

                GameObject bg = backGroundDic[listIndex].Value;
                bg.transform.localPosition = bgLength * toIndex * Vector3.right;
                backGroundDic[listIndex] = new KeyValuePair<int, GameObject>(toIndex, bg);

                bg.SetActive(!removeIndex.Contains(toIndex));

                lastCameraIndex = nowCameraIndex;
            }
            yield return null;
        }
    }


    public IEnumerator MoveBackGroundByPlayer()
    {
        float cameraLastPosX, cameraNowPosX;
        while (true)
        {
            cameraLastPosX = CameraController.Instance.cameraT.position.x;
            yield return null;
            cameraNowPosX = CameraController.Instance.cameraT.position.x;

            float deltaX = cameraNowPosX - cameraLastPosX;

            transform.position += deltaX * ratio * Vector3.right;
        }
    }
}
