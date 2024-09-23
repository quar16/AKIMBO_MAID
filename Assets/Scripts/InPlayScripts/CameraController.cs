using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoSingleton<CameraController>
{
    //카메라의 트랜스폼, 오프셋을 적용하는데 사용한다
    public Transform cameraT;
    public Transform shakeT;

    [SerializeField]
    private float cameraSize = 5;
    [SerializeField]
    private Vector2 offset = new Vector2(7.5f, 3);
    [SerializeField]
    private float camTrackingPower = 0.1f;

    private Dictionary<string, NamedCharacter> cameraTragetDic = new();

    void Update()
    {
        cameraT.localPosition = Vector3.Lerp(cameraT.localPosition, offset, camTrackingPower * PlayTime.Scale);
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, cameraSize, camTrackingPower * PlayTime.Scale);

        float weight = 0;
        float targetX = 0;

        foreach (var namedCharacter in cameraTragetDic.Values)
        {
            if (namedCharacter.cameraWeight != 0)
            {
                weight += namedCharacter.cameraWeight;
                targetX += namedCharacter.transform.position.x * namedCharacter.cameraWeight;
            }
        }

        if (weight == 0) return;

        targetX /= weight;

        Vector3 targetPos = transform.position;
        float camPosX = targetPos.x;

        targetPos.x = Mathf.Lerp(camPosX, targetX, camTrackingPower * PlayTime.Scale);

        transform.position = targetPos;
    }

    public void AddNamedCharacter(string characterName, float weight)
    {
        if (!cameraTragetDic.ContainsKey(characterName))
            cameraTragetDic.Add(characterName, NamedCharacter.GetNamedCharacter(characterName));

        cameraTragetDic[characterName].cameraWeight = weight;
    }

    public void RemoveNamedCharacter(string characterName)
    {
        cameraTragetDic.Remove(characterName);
    }

    public void SetCameraTargetWeight(string key, float weight)
    {
        if (cameraTragetDic.ContainsKey(key))
            cameraTragetDic[key].cameraWeight = weight;
        else
            Debug.LogWarning("Key does not exist in targetDictionary");
    }

    public void SetCameraSize(float newSize)
    {
        cameraSize = newSize;
    }

    public void SetCameraOffset(Vector2 newOffset)
    {
        offset = newOffset;
    }

    public void SetCamTrackingPower(float newPower)
    {
        camTrackingPower = newPower;
    }

    public IEnumerator DeadProcessingCamera()
    {
        Time.timeScale = 0.2f;
        camTrackingPower = 0.9f;

        cameraTragetDic.Clear();
        AddNamedCharacter("Player", 1);

        Direction direction = PlayerManager.Instance.playerMoveController.playerDirection;
        float x = 0.5f * (direction == Direction.LEFT ? -1 : 1);
        SetCameraOffset(new Vector2(x, 0.1f));

        SetCameraSize(4);

        CameraShake(1, 2, 0.2f, 5);

        yield return PlayTime.ScaledWaitForSeconds(0.5f);

        Time.timeScale = 1;

        camTrackingPower = 0;

        float time = Time.time;

        var player = NamedCharacter.GetNamedCharacter("Player");

        while (time + 1 > Time.time)
        {
            float t = Time.time - time;

            Camera.main.orthographicSize = 4 * Mathf.Cos(t * 0.5f * Mathf.PI);
            cameraT.localPosition = Vector3.Lerp(cameraT.localPosition, offset, 0.9f);

            float targetX = player.transform.position.x;
            Vector3 targetPos = transform.position;
            float camPosX = targetPos.x;
            targetPos.x = Mathf.Lerp(camPosX, targetX, 0.5f);
            transform.position = targetPos;

            yield return PlayTime.ScaledNull;
        }
        yield return SceneTransitionManager.Instance.CallFadeEffect(FadeTypes.Quick, IO.Out);
    }


    public void CameraShake(float powerX, float powerY, float duration, int gap)
    {
        StartCoroutine(CameraShaking(powerX, powerY, duration, gap));
    }

    IEnumerator CameraShaking(float powerX, float powerY, float duration, int gap)
    {
        duration += Time.time;
        int yDirection = 1;

        while (Time.time < duration)
        {
            float randomX = Random.Range(-1f, 1f) * powerX * 0.1f;
            float randomY = Random.Range(0, 1f) * powerY * 0.1f;

            shakeT.localPosition = new Vector2(randomX, randomY * yDirection);

            yDirection *= -1;

            yield return PlayTime.ScaledWaitForSeconds(gap * 0.016f);
        }
        shakeT.localPosition = Vector3.zero;
    }
}
