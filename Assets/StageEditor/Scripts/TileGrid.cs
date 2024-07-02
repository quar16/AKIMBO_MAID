using UnityEngine;

public class TileGrid : MonoSingleton<TileGrid>
{
    public Vector2Int gridSize = new Vector2Int(10, 10);

    public float tileSize = 1.0f;
    public static float TileSize;

    public Color gridColor = Color.gray;
    public float lineWidth = 0.1f;

    public static Vector3 GetTilePosByGridIndex(Vector2Int v2i)
    {
        Vector3 v3 = new Vector3(v2i.x * TileSize, v2i.y * TileSize, 0);

        return v3;
    }

    public Vector2Int SnapToGrid(Transform _transform)
    {
        Vector3 newPosition = _transform.position;
        Vector2Int v2i = new()
        {
            x = (int)Mathf.Round(newPosition.x / tileSize),
            y = (int)Mathf.Round(newPosition.y / tileSize)
        };

        // 허용된 범위를 벗어나는지 확인
        v2i.x = Mathf.Clamp(v2i.x, 0, gridSize.x - 1);
        v2i.y = Mathf.Clamp(v2i.y, 0, gridSize.y - 1);

        // 그리드에 맞게 위치 조정
        _transform.position = new Vector3(v2i.x * tileSize, v2i.y * tileSize, 0);

        // 인덱스 반환
        return v2i;
    }

    void Start()
    {
        TileSize = tileSize;
        DrawGrid();
    }

    void DrawGrid()
    {
        GameObject gridLines = new GameObject("GridLines");

        for (float x = 0; x <= gridSize.x; x += tileSize)
        {
            CreateLine(gridLines.transform, new Vector3(x, 0, 0), new Vector3(x, gridSize.y * tileSize, 0));
        }

        for (float y = 0; y <= gridSize.y; y += tileSize)
        {
            CreateLine(gridLines.transform, new Vector3(0, y, 0), new Vector3(gridSize.x * tileSize, y, 0));
        }
    }

    void CreateLine(Transform parent, Vector3 start, Vector3 end)
    {
        GameObject lineObj = new GameObject("Line");
        lineObj.transform.parent = parent;
        LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();

        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = gridColor;
        lineRenderer.endColor = gridColor;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }
}
