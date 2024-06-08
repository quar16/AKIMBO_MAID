using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EntitySpawnData
{
    public Vector2Int gridIndex; // Ÿ�ϸ��� ��ǥ��
    public Vector2 offset; // ������ ��

    public int prefabId; // ��ƼƼ�� ������ ID
    public List<float> customValues; // ���� ������ ��ġ����
}
