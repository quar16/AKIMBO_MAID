using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMover : MonoBehaviour
{
    public GameObject backgroundPrefab;
    public float bgLength;
    public float ratio = 0.7f;

    public int bgCount = 3;
    int halfCount { get { return (int)((bgCount - 1) * 0.5f); } }

    int nowCameraIndex = 0;

    protected List<BackGroundBlock> backGroundList = new();

    List<int> removeIndex = new();

    IEnumerator moveBackGround;
    IEnumerator moveBackGroundByPlayer;

    bool isMove = false;
    public bool debug = false;

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

    public virtual void AddRemoveIndex(int index)
    {
        removeIndex.Add(index);

        foreach (var v in backGroundList)
            if (v.index == index)
                v.block.SetActive(false);
    }

    public void CleanUp()
    {
        isMove = false;

        StopCoroutine(moveBackGround);
        StopCoroutine(moveBackGroundByPlayer);

        transform.position = Vector3.zero;

        foreach (var v in backGroundList)
            Destroy(v.block);

        removeIndex.Clear();
        backGroundList.Clear();
    }

    public void InitBackGround()
    {
        for (int i = -halfCount; i <= halfCount; i++)
        {
            GameObject bg = Instantiate(backgroundPrefab, transform);
            bg.transform.localPosition = bgLength * i * Vector3.right;

            backGroundList.Add(new BackGroundBlock(i, bg));
        }
    }

    public IEnumerator MoveBackGround()
    {
        int lastCameraIndex = nowCameraIndex;
        while (isMove)
        {
            nowCameraIndex = (int)((CameraController.Instance.cameraT.position.x - transform.position.x + bgLength * 0.5f) / bgLength);
            if (lastCameraIndex != nowCameraIndex)
            {
                int fromIndex = lastCameraIndex + (lastCameraIndex - nowCameraIndex) * halfCount;
                int toIndex = fromIndex + (nowCameraIndex - lastCameraIndex) * bgCount;

                if (debug)
                    Debug.Log(lastCameraIndex + "|" + nowCameraIndex + "|" + fromIndex + "|" + toIndex);

                var backGroundBlock = backGroundList.Find(x => x.index == fromIndex);

                if (backGroundBlock != null)
                {
                    BackGroundUpdate(backGroundBlock, toIndex);
                }

                lastCameraIndex = nowCameraIndex;
            }
            yield return PlayTime.ScaledNull;
        }
    }

    public virtual void BackGroundUpdate(BackGroundBlock backGroundBlock, int toIndex)
    {
        backGroundBlock.block.transform.localPosition = bgLength * toIndex * Vector3.right;
        backGroundBlock.index = toIndex;

        backGroundBlock.block.SetActive(!removeIndex.Contains(toIndex));
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

public class BackGroundBlock
{
    public int index;
    public GameObject block;

    public BackGroundBlock(int index, GameObject block)
    {
        this.index = index;
        this.block = block;
    }
}