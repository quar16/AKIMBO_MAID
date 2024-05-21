using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public float bgLength;
    public float fbgLength;

    public float lastFarBackGroundPosX;
    public float lastBackGroundPosX;

    public int targetBackGroundIndex;
    public int targetFarBackGroundIndex;

    public StageDataScriptableObject tempStageData;

    public List<GameObject> backGroundList;
    public List<GameObject> farBackGroundList;

    public Transform playerT;

    public Transform bgParent;
    public Transform fbgParent;

    public Image upperLetterBox;
    public Image lowerLetterBox;

    void Start()
    {
        StartCoroutine(Initiate());
    }


    void Update()
    {

    }

    public IEnumerator Initiate()
    {
        InitBackGround();

        yield return NarrativePlay();

        GameManager.Instance.gameMode = GameMode.RUN;

        StartCoroutine(MoveBackGround());
        StartCoroutine(MoveFarBackGround());
        StartCoroutine(MoveFarBackGround2());

    }

    public void InitBackGround()
    {
        for (int i = 0; i < 3; i++)
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

    public IEnumerator MoveBackGround()
    {
        while (true)
        {
            yield return new WaitUntil(() => playerT.position.x - lastBackGroundPosX >= bgLength * 1.2f);
            backGroundList[targetBackGroundIndex].transform.position += Vector3.right * bgLength * 3;
            lastBackGroundPosX += bgLength;
            targetBackGroundIndex = (targetBackGroundIndex + 1) % 3;
        }
    }


    public IEnumerator MoveFarBackGround()
    {
        while (true)
        {
            yield return new WaitUntil(() => playerT.position.x - lastFarBackGroundPosX >= fbgLength * 1.2f);
            farBackGroundList[targetFarBackGroundIndex].transform.position += Vector3.right * fbgLength * 3;
            lastFarBackGroundPosX += fbgLength;
            targetFarBackGroundIndex = (targetFarBackGroundIndex + 1) % 3;
        }
    }

    public IEnumerator MoveFarBackGround2()
    {
        while (true)
        {
            yield return null;
            lastFarBackGroundPosX += Time.deltaTime;
            fbgParent.transform.position += Vector3.right * Time.deltaTime;
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
        for (int i = 0; i < 30; i++)
        {
            upperLetterBox.rectTransform.anchoredPosition += Vector2.down * 7;
            lowerLetterBox.rectTransform.anchoredPosition += Vector2.up * 7;
            yield return null;
        }
    }
    IEnumerator LetterBoxOut()
    {
        for (int i = 0; i < 30; i++)
        {
            upperLetterBox.rectTransform.anchoredPosition += Vector2.up * 7;
            lowerLetterBox.rectTransform.anchoredPosition += Vector2.down * 7;
            yield return null;
        }
    }

}


/*
 �ܺο� ����� �������� �����͸� �޾ƿͼ� ����
 ���� �ٴ�, ������ ���� �����δ�
 ���̳� ���� ���������� �÷��̾��� ��ġ�� ������� ��ȯ
 ó���� ������ ����, ���������� ���� ȿ���� �߰��Ѵ� / ������ enter�� �ѱ��
 ���� ��ȯ Ʈ���Ű� �۵��ϸ� ������ ������ ����� �̵��� �����
 ����� �ﱳ��� ������

 �������� ������
 ���� ����, ����
 �������� ��ġ
 ������ ��ġ, ����
 
 
 
 */