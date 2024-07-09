using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DraggableObject_Drone : DraggableObject
{
    public float speed = 1;
    public bool isRoundTrip = true;

    public DraggableObject_DroneRoute droneRoutePrefab;

    List<DraggableObject_DroneRoute> droneRouteList = new();

    public override void Init()
    {
        gridIndex = TileGrid.Instance.SnapToGrid(transform);
        if (customValues == null)
        {
            customValues = new List<float> { speed, isRoundTrip ? 1 : 0 };
        }
        else
        {
            speed = customValues[0];
            isRoundTrip = customValues[1] == 1;

            for (int i = 2; i < customValues.Count; i += 2)
            {
                AddRoute();

                Vector3 routePoint = Vector3.right * customValues[i] + Vector3.up * customValues[i + 1];

                droneRouteList[i / 2 - 1].transform.position = routePoint;
            }

            foreach (var droneRoute in droneRouteList)
                droneRoute.Init();
        }
    }

    //0:스피드 / 1:왕복여부 / 2~드론 경로 좌표들
    public override List<float> GetCustomValue()
    {
        customValues[0] = speed;
        customValues[1] = isRoundTrip ? 1 : 0;

        for (int i = 0; i < droneRouteList.Count; i++)
        {
            droneRouteList[i].gridIndex = TileGrid.Instance.SnapToGrid(droneRouteList[i].transform);
            customValues[(i + 1) * 2] = droneRouteList[i].gridIndex.x;
            customValues[(i + 1) * 2 + 1] = droneRouteList[i].gridIndex.y;
        }

        return customValues;
    }

    public void AddRoute()
    {
        DraggableObject_DroneRoute droneRoute = Instantiate(droneRoutePrefab, transform);

        if (droneRouteList.Count == 0)
        {
            droneRoute.connectPoint = transform;

        }
        else
        {
            Transform lastRoute = droneRouteList[^1].transform;

            droneRoute.connectPoint = lastRoute;
            droneRoute.transform.position = lastRoute.position;
        }

        droneRoute.drone = this;
        droneRoute.index = droneRouteList.Count;

        droneRouteList.Add(droneRoute);
    }

    public void AddRouteWithCustomValue()
    {
        AddRoute();
        customValues.Add(0);
        customValues.Add(0);
    }

    public void RemoveRoute()
    {
        if (droneRouteList.Count != 0)
        {
            GameObject lastRoute = transform.GetChild(transform.childCount - 1).gameObject;
            Destroy(lastRoute);
            droneRouteList.RemoveAt(droneRouteList.Count - 1);
            customValues.RemoveAt(customValues.Count - 1);
            customValues.RemoveAt(customValues.Count - 1);
        }
    }

    public void CallConnectLineSet(int index)
    {
        droneRouteList[index].SetConnectLine();

        if (droneRouteList.Count > index + 1)
            droneRouteList[index + 1].SetConnectLine();
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(DraggableObject_Drone))]
public class DraggableObject_Drone_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DraggableObject_Drone spawner = (DraggableObject_Drone)target;
        if (GUILayout.Button("Add Route"))
        {
            spawner.AddRouteWithCustomValue();
        }
        if (GUILayout.Button("Remove Route"))
        {
            spawner.RemoveRoute();
        }
    }
}
#endif