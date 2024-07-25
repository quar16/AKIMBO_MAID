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

    private Dictionary<CharacterNames, NamedCharacter> namedCharacterDic = new();

    void Update()
    {
        cameraT.localPosition = Vector3.Lerp(cameraT.localPosition, offset, camTrackingPower * PlayTime.Scale);
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, cameraSize, camTrackingPower * PlayTime.Scale);

        float weight = 0;
        float targetX = 0;

        foreach (var namedCharacter in namedCharacterDic.Values)
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

    public void AddNamedCharacter(CharacterNames characterName, float weight)
    {
        if (!namedCharacterDic.ContainsKey(characterName))
            namedCharacterDic.Add(characterName, NamedCharacter.GetNamedCharacter(characterName));

        namedCharacterDic[characterName].cameraWeight = weight;
    }

    public void RemoveNamedCharacter(CharacterNames characterName)
    {
        namedCharacterDic.Remove(characterName);
    }

    public void SetCameraTargetWeight(CharacterNames key, float weight)
    {
        if (namedCharacterDic.ContainsKey(key))
            namedCharacterDic[key].cameraWeight = weight;
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

    public void CleanUp()
    {
        namedCharacterDic.Clear();

        transform.position = new Vector3(-0.95f, 0, -10);

        cameraSize = 5;
        offset = new Vector2(7.5f, 3);
        camTrackingPower = 0.1f;
    }
}
