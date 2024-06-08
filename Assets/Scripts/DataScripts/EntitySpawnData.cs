using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EntitySpawnData
{
    public Vector2Int gridIndex; // 타일맵의 좌표값
    public Vector2 offset; // 오프셋 값

    public int prefabId; // 엔티티의 프리팹 ID
    public List<float> customValues; // 개수 미정의 수치값들
}
