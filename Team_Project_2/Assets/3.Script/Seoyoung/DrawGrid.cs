using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawGrid : Graphic
{
    public Vector2Int gridSize = new Vector2Int(1, 1);  //x,y�� ����Ʈ ���� ���� �� �ֿ� ��.��!
    public float thickness = 10f;

    float width;
    float height;
    float cellWidth;
    float cellHeight;

    float current_Time = 0f;    //�ð� ��ȭ�� ����
    protected override void Start()
    {
        gridSize.x = 1;

        float MaxY = 0;

        for(int i = 0; i <GameManager.instance.TeamPoint_graph.Count; i++)
        {
            if(GameManager.instance.TeamPoint_graph[i] >= MaxY)
            {
                MaxY = GameManager.instance.TeamPoint_graph[i];
            }
            if (GameManager.instance.EnemyPoint_graph_1[i] >= MaxY)
            {
                MaxY = GameManager.instance.EnemyPoint_graph_1[i];
            }
            if (GameManager.instance.EnemyPoint_graph_2[i] >= MaxY)
            {
                MaxY = GameManager.instance.EnemyPoint_graph_1[i];
            }
            if (GameManager.instance.EnemyPoint_graph_3[i] >= MaxY)
            {
                MaxY = GameManager.instance.EnemyPoint_graph_1[i];
            }
        }


        gridSize.y = ((int)MaxY / 10) + 50;
    }

    private void Update()
    {
        //endTime/20�� ���� ������ x�� (�ð�) �� ���� �ø�
        //GameManager.instance.EndTime / 20
        //if(!GameManager.instance.isGameEnd)
        //{
         
        //}
        if (current_Time + (GameManager.instance.EndTime / 20) <= GameManager.instance.currentTime)
        {
            Debug.Log("����");
            current_Time += (GameManager.instance.EndTime / 20);
            gridSize.x += 1;

            SetVerticesDirty();
        }

    }

    //������ �ʴ� ���� �׷��� ��ġ ó���ϰ������ ���Ǵ� �Լ�
    //VertexHelper : UI �Ž� ���� ���� Ŭ����
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        base.OnPopulateMesh(vh);
        vh.Clear();     //�ʱ�ȭ

        width = rectTransform.rect.width;
        height = rectTransform.rect.height;

        cellWidth = width / (float)gridSize.x;
        cellHeight = height / (float)gridSize.y;

        int count = 0;

        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                drawCell(x, y, count, vh);
                count++;
            }
        }

    }


    private void drawCell(int x, int y, int index, VertexHelper vh)
    {
        float xPos = cellWidth * x;
        float yPos = cellHeight * y;

        //AddVert(UIVertex v) : ��Ʈ���� ���� ������ �߰�
        //AddTrighangle(int index0, int idex1, int index2) : ���ۿ� �ﰢ�� �߰�

        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;
        
        vertex.position = new Vector3(xPos, yPos);
        vh.AddVert(vertex);

        vertex.position = new Vector3(xPos, yPos + cellHeight);
        vh.AddVert(vertex);

        vertex.position = new Vector3(xPos + cellWidth, yPos + cellHeight);
        vh.AddVert(vertex);

        vertex.position = new Vector3(xPos + cellWidth, yPos);
        vh.AddVert(vertex);


        //vh.AddTriangle(0, 1, 2);
        //vh.AddTriangle(2, 3, 0);


        float widthSqr = thickness * thickness;
        float distanceSqr = widthSqr / 2f;
        float distance = Mathf.Sqrt(distanceSqr);

        vertex.position = new Vector3(xPos + distance, yPos + distance);
        vh.AddVert(vertex);

        vertex.position = new Vector3(xPos + distance, yPos + (cellHeight - distance));
        vh.AddVert(vertex);

        vertex.position = new Vector3(xPos + (cellWidth - distance), yPos + (cellHeight - distance));
        vh.AddVert(vertex);

        vertex.position = new Vector3(xPos + (cellWidth - distance), yPos + distance);
        vh.AddVert(vertex);

        int offset = index * 8;

        //left edge
        vh.AddTriangle(offset + 0, offset + 1, offset + 5);
        vh.AddTriangle(offset + 5, offset + 4, offset + 0);

        //top edge
        vh.AddTriangle(offset + 1, offset + 2, offset + 6);
        vh.AddTriangle(offset + 6, offset + 5, offset + 1);

        //right edge
        vh.AddTriangle(offset + 2, offset + 3, offset + 7);
        vh.AddTriangle(offset + 7, offset + 6, offset + 2);

        //bottom edge
        vh.AddTriangle(offset + 3, offset + 0, offset + 4);
        vh.AddTriangle(offset + 4, offset + 7, offset + 3);

    }
}
