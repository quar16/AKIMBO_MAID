using UnityEngine;

public class TileGrid : MonoBehaviour
{
    public Vector2 gridSize = new Vector2(10, 10);
    public float tileSize = 1.0f;
    public Color gridColor = Color.gray;
    public float lineWidth = 0.1f;

    public Vector2 SnapToGrid(Vector2 originalPosition)
    {
        Vector3 newPosition = originalPosition;

        float gridX = Mathf.Round(newPosition.x / tileSize);
        float gridY = Mathf.Round(newPosition.y / tileSize);

        // ���� ������ ������� Ȯ��
        gridX = Mathf.Clamp(gridX, 0, gridSize.x - 1);
        gridY = Mathf.Clamp(gridY, 0, gridSize.y - 1);

        // �׸��忡 �°� ��ġ ����
        newPosition.x = gridX * tileSize;
        newPosition.y = gridY * tileSize;

        // ������Ʈ�� ��ġ ��ȯ
        return newPosition;
    }

    void Start()
    {
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
