using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DraggableObject_Laser : DraggableObject
{
    bool isFront = true;
    public float chargeTime = 5;
    public float fireTime = 5;
    public int range = 17;

    public List<Sprite> laserImage = new();
    public SpriteRenderer spriteRenderer;

    public GameObject laserRange;

    public override void Init()
    {
        gridIndex = TileGrid.Instance.SnapToGrid(transform);

        if (customValues == null)
        {
            customValues = new List<float> { isFront ? 1 : 0, chargeTime, fireTime, range };
        }
        else
        {
            isFront = customValues[0] == 1;
            chargeTime = customValues[1];
            fireTime = customValues[2];
            range = (int)customValues[3];

            spriteRenderer.sprite = laserImage[isFront ? 0 : 1];
        }
    }

    public override List<float> GetCustomValue()
    {
        customValues[0] = isFront ? 1 : 0;
        customValues[1] = chargeTime;
        customValues[2] = fireTime;
        customValues[3] = range;

        return customValues;
    }

    public void LaserSpin()
    {
        isFront = !isFront;
        spriteRenderer.sprite = laserImage[isFront ? 0 : 1];
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(DraggableObject_Laser))]
public class DraggableObject_Laser_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DraggableObject_Laser laser = (DraggableObject_Laser)target;

        if (GUILayout.Button("Laser Spin"))
            laser.LaserSpin();
    }
}
#endif