using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoSingleton<StageManager>
{
    public float bgLength;
    public float fbgLength;

    public float lastFarBackGroundPosX;
    public float lastBackGroundPosX;

    int bgCount = 5;

    public int targetBackGroundIndex;
    public int targetFarBackGroundIndex;

    public StageDataScriptableObject tempStageData;

    public List<GameObject> backGroundList;
    public List<GameObject> farBackGroundList;

    public Transform bgParent;
    public Transform fbgParent;

    public Image upperLetterBox;
    public Image lowerLetterBox;

    IEnumerator moveBackGround;

    private void Start()
    {
        SceneInit(tempStageData, 0);
    }

    public void SceneInit(StageDataScriptableObject stageData, int startPoint = 0)
    {
        tempStageData = stageData;
        StartCoroutine(Initiate());
        OppositionEntityManager.Instance.StartEnemySpawnRoutine(stageData.entities);
    }

    public IEnumerator Initiate()
    {
        InitBackGround();

        yield return NarrativePlay();

        GameManager.Instance.gameMode = GameMode.RUN;
        moveBackGround = MoveBackGround();
        StartCoroutine(moveBackGround);
        StartCoroutine(MoveFarBackGround());
        StartCoroutine(MoveFarBackGroundByPlayer());

    }

    public void InitBackGround()
    {
        for (int i = 0; i < bgCount; i++)
        {
            GameObject bg = new GameObject("bg - " + i);
            GameObject fbg = new GameObject("fbg - " + i);
            Instantiate(tempStageData.floorSprite, Vector3.right * i * bgLength, Quaternion.identity, bg.transform);
            Instantiate(tempStageData.wallSprite, Vector3.right * i * bgLength, Quaternion.identity, bg.transform);
            Instantiate(tempStageData.backgroundSprite, Vector3.right * i * fbgLength, Quaternion.identity, fbg.transform);

            bg.transform.parent = bgParent;
            fbg.transform.parent = fbgParent;

            backGroundList.Add(bg);
            farBackGroundList.Add(fbg);
        }
    }

    public void CallBossRoom()
    {
        StopCoroutine(moveBackGround);
        GameManager.Instance.gameMode = GameMode.BOSS;
        CameraController.Instance.UpdateCameraTarget("Player", false);
    }

    public IEnumerator MoveBackGround()
    {
        while (true)
        {
            // 오른쪽으로 이동할 때
            if (CameraController.Instance.cameraT.position.x - lastBackGroundPosX >= bgLength)
            {
                backGroundList[targetBackGroundIndex].transform.position += Vector3.right * bgLength * bgCount;
                lastBackGroundPosX += bgLength;
                targetBackGroundIndex = (targetBackGroundIndex + 1) % bgCount;
            }
            // 왼쪽으로 이동할 때
            else if (CameraController.Instance.cameraT.position.x - lastBackGroundPosX <= -bgLength)
            {
                backGroundList[(targetBackGroundIndex - 1 + bgCount) % bgCount].transform.position -= Vector3.right * bgLength * bgCount;
                lastBackGroundPosX -= bgLength;
                targetBackGroundIndex = (targetBackGroundIndex - 1 + bgCount) % bgCount;
            }

            yield return null;
        }
    }

    public IEnumerator MoveFarBackGround()
    {
        while (true)
        {
            // 오른쪽으로 이동할 때
            if (CameraController.Instance.cameraT.position.x - lastFarBackGroundPosX >= fbgLength)
            {
                farBackGroundList[targetFarBackGroundIndex].transform.position += Vector3.right * fbgLength * bgCount;
                lastFarBackGroundPosX += fbgLength;
                targetFarBackGroundIndex = (targetFarBackGroundIndex + 1) % bgCount;
            }
            // 왼쪽으로 이동할 때
            else if (CameraController.Instance.cameraT.position.x - lastFarBackGroundPosX <= -fbgLength)
            {
                farBackGroundList[(targetFarBackGroundIndex - 1 + bgCount) % bgCount].transform.position -= Vector3.right * fbgLength * bgCount;
                lastFarBackGroundPosX -= fbgLength;
                targetFarBackGroundIndex = (targetFarBackGroundIndex - 1 + bgCount) % bgCount;
            }
            yield return null;
        }
    }


    public IEnumerator MoveFarBackGroundByPlayer()
    {
        float cameraLastPosX, cameraNowPosX;
        float ratio = 0.7f;
        while (true)
        {
            cameraLastPosX = CameraController.Instance.cameraT.position.x;
            yield return null;
            cameraNowPosX = CameraController.Instance.cameraT.position.x;

            float deltaX = cameraNowPosX - cameraLastPosX;

            lastFarBackGroundPosX += deltaX * ratio;

            fbgParent.transform.position += deltaX * ratio * Vector3.right;
        }
    }


    public IEnumerator NarrativePlay()
    {
        yield return LetterBoxIn();

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
        yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Return));

        yield return LetterBoxOut();
    }

    IEnumerator LetterBoxIn()
    {
        for (int i = 0; i <= 30; i++)
        {
            upperLetterBox.rectTransform.anchoredPosition = Vector2.up * Mathf.Lerp(0, 200, 1 - i / 30f);
            lowerLetterBox.rectTransform.anchoredPosition = Vector2.down * Mathf.Lerp(0, 200, 1 - i / 30f);
            yield return null;
        }
    }
    IEnumerator LetterBoxOut()
    {
        for (int i = 0; i <= 30; i++)
        {
            upperLetterBox.rectTransform.anchoredPosition = Vector2.up * Mathf.Lerp(0, 200, i / 30f);
            lowerLetterBox.rectTransform.anchoredPosition = Vector2.down * Mathf.Lerp(0, 200, i / 30f);
            yield return null;
        }
    }

}


/*
 외부에 저장된 스테이지 데이터를 받아와서 구현
 배경과 바닥, 원경은 따로 움직인다
 적이나 보스 스테이지는 플레이어의 위치를 기반으로 소환
 처음과 보스전 시작, 보스전에는 연출 효과를 추가한다 / 지금은 enter로 넘기기
 보스 소환 트리거가 작동하면 원경을 제외한 배경의 이동은 멈춘다
 배경은 삼교대로 돌린다

 스테이지 데이터
 연출 구간, 내용
 스테이지 배치
 보스전 위치, 정보
 
 
 
 */