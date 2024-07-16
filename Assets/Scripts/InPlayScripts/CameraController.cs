using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoSingleton<CameraController>
{
    //카메라의 트랜스폼, 오프셋을 적용하는데 사용한다
    public Transform cameraT;

    Dictionary<string, NamedCharacter> namedCharacterDic = new();
    public float cameraSize = 5;
    public Vector2 offset = new Vector2(7.5f, 3);
    float camTrackingPower = 0.1f;

    void Update()
    {
        cameraT.localPosition = Vector3.Lerp(cameraT.localPosition, offset, camTrackingPower);
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, cameraSize, camTrackingPower);

        float weight = 0;
        float targetX = 0;

        foreach (var namedCharacter in namedCharacterDic.Values)
        {
            if (namedCharacter.targeted && namedCharacter.cameraWeight != 0)
            {
                weight += namedCharacter.cameraWeight;
                targetX += namedCharacter.transform.position.x * namedCharacter.cameraWeight;
            }
        }

        if (weight == 0) return;

        targetX /= weight;

        Vector3 targetPos = transform.position;
        float camPosX = targetPos.x;

        targetPos.x = Mathf.Lerp(camPosX, targetX, camTrackingPower);

        transform.position = targetPos;
    }

    public void AddNamedCharacter(NamedCharacter namedCharacter)
    {
        namedCharacterDic.Add(namedCharacter.NarrativeName, namedCharacter);
    }

    public void RemoveNamedCharacter(NamedCharacter namedCharacter)
    {
        namedCharacterDic.Remove(namedCharacter.NarrativeName);
    }

    public void UpdateCameraTarget(string key, float weight, bool targeted)
    {
        UpdateCameraTarget(key, weight);
        UpdateCameraTarget(key, targeted);
    }

    public void UpdateCameraTarget(string key, float weight)
    {
        if (namedCharacterDic.ContainsKey(key))
            namedCharacterDic[key].cameraWeight = weight;
        else
            Debug.LogWarning("Key does not exist in targetDictionary");
    }

    public void UpdateCameraTarget(string key, bool targeted)
    {
        if (namedCharacterDic.ContainsKey(key))
            namedCharacterDic[key].targeted = targeted;
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


}
