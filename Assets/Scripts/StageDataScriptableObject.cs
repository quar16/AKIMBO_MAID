using UnityEngine;

[CreateAssetMenu(fileName = "Stage - 1", menuName = "Stage Data")]
public class StageDataScriptableObject : ScriptableObject
{
    public GameObject floorSprite;
    public GameObject wallSprite;
    public GameObject backgroundSprite;

    public TextAsset stageDataJson; // 인스펙터에서 연결할 JSON 파일
}
