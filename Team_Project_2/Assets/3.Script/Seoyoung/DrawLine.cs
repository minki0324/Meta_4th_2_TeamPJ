using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawLine : Graphic
{
    public Vector2Int gridSize;

    public DrawGrid grid;


    public List<Vector2> points;
    public Vector2 graphPoint;


    float width;
    float height;
    float unitWidth;
    float unitHeight;

    public float thickness = 10f;

    float current_Time = 0f;    //시간 변화용 변수
    int cell_x = 0;
    int cell_y = 0;
    protected override void Start()
    {
        points.Clear();
        points.Add(new Vector2(0, 0));
        //points.Add(new Vector2(cell_x + 1, 3));
        for(int i = 0; i< GameManager.instance.TeamPoint_graph.Count; i++)
        {
            UIVertex vertex = UIVertex.simpleVert;
            if (gameObject.name.ToString() == "Line_Team1")
            {
                graphPoint = new Vector2(grid.gridSize.x + (i + 1), GameManager.instance.TeamPoint_graph[i] / 10f);
                points.Add(graphPoint);

            }
            else if (gameObject.name.ToString() == "Line_Enemy1")
            {
                graphPoint = new Vector2(grid.gridSize.x + (i + 1), GameManager.instance.EnemyPoint_graph_1[i] / 10f);
                points.Add(graphPoint);

            }
            else if (gameObject.name.ToString() == "Line_Enemy2")
            {

                graphPoint = new Vector2(grid.gridSize.x + (i + 1), GameManager.instance.EnemyPoint_graph_2[i] / 10f);
                points.Add(graphPoint);

            }
            else if (gameObject.name.ToString() == "Line_Enemy3")
            {
                graphPoint = new Vector2(grid.gridSize.x + (i + 1), GameManager.instance.EnemyPoint_graph_3[i] / 10f);
                points.Add(graphPoint);
            }

           
        }

        SetVerticesDirty();
    }
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        width = rectTransform.rect.width;
        height = rectTransform.rect.height;

        unitWidth = width / (float)gridSize.x;
        unitHeight = height / (float)gridSize.y;


        if (points[0].x != 0 || points[0].y != 0)
        {
            points[0] = new Vector2(0, 0f);
        }

        if (points.Count < 2)
        {
            return;
        }

        float angle = 0;
        for (int i = 0; i < points.Count; i++)
        {
            Vector2 point = points[i];

            //굵기조절
            if (i < points.Count - 1)
            {
                angle = GetAngle(points[i], points[i + 1]) + 20;
            }

            DrawPoint(point, vh, angle);
        }


        for (int i = 0; i < points.Count - 1; i++)
        {
            int index = i * 2;
            vh.AddTriangle(index + 0, index + 1, index + 3);
            vh.AddTriangle(index + 3, index + 2, index + 0);
        }

    }

    public float GetAngle(Vector2 pos, Vector2 target)
    {
        return (float)(Mathf.Atan2(target.y - pos.y, target.x = pos.x) * (180 / Mathf.PI));
    }


    public void Getcolor(int num, ref UIVertex v)
    {
        for (int i = 0; i < GetColor.instance.teamColors.Length; i++)
        {
            if (num == GetColor.instance.teamColors[i].ColorNum)
            {
                v.color = GetColor.instance.teamColors[i].color_c;
            }
        }
    }


    public Color Get_color(int num)
    {
        for (int i = 0; i < GetColor.instance.teamColors.Length; i++)
        {
            if (num == GetColor.instance.teamColors[i].ColorNum)
            {
                return GetColor.instance.teamColors[i].color_c;
            }

        }
        return Color.black;
    }


    //정점 찾는 함수
    private void DrawPoint(Vector2 point, VertexHelper vh, float angle)
    {
        UIVertex vertex = UIVertex.simpleVert;
        if (gameObject.name.ToString() == "Line_Team1")
        {
            vertex.color = ColorManager.instance.Teamcolor[GameManager.instance.Color_Index];
            vertex.color.a = 150;
        }
        else if (gameObject.name.ToString() == "Line_Enemy1")
        {
            vertex.color = ColorManager.instance.Teamcolor[GameManager.instance.T1_Color];
            vertex.color.a = 150;
        }
        else if (gameObject.name.ToString() == "Line_Enemy2")
        {
            vertex.color = ColorManager.instance.Teamcolor[GameManager.instance.T2_Color];
            vertex.color.a = 150;
        }
        else if (gameObject.name.ToString() == "Line_Enemy3")
        {
            vertex.color = ColorManager.instance.Teamcolor[GameManager.instance.T3_Color];
            vertex.color.a = 150;   
        }


        vertex.position = Quaternion.Euler(0, 0, angle) * new Vector3(-thickness / 2, 0);
        vertex.position += new Vector3(unitWidth * point.x, unitHeight * point.y);
        vh.AddVert(vertex);


        vertex.position = Quaternion.Euler(0, 0, angle) * new Vector3(thickness / 2, 0);
        vertex.position += new Vector3(unitWidth * point.x, unitHeight * point.y);
        vh.AddVert(vertex);
    }

    private void Update()
    {
        if (grid != null)
        {
            if (gridSize != grid.gridSize)
            {
                gridSize = grid.gridSize;
                SetVerticesDirty();
            }
        }

        //if (current_Time + (GameManager.instance.EndTime / 20) <= GameManager.instance.currentTime)
        //{
        //    current_Time += (GameManager.instance.EndTime / 20);

        //    //시간(x), 팀포인트(y)추가
        //    if (gameObject.name.ToString() == "Line_Team1")
        //    {
        //        graphPoint = new Vector2(grid.gridSize.x, GameManager.instance.Teampoint / 100f);
        //        points.Add(graphPoint);

        //    }
        //    else if (gameObject.name.ToString() == "Line_Enemy1")
        //    {
        //        graphPoint = new Vector2(grid.gridSize.x, GameManager.instance.leaders[0].Teampoint / 100f);
        //        points.Add(graphPoint);
        //    }
        //    else if (gameObject.name.ToString() == "Line_Enemy2")
        //    {
        //        graphPoint = new Vector2(grid.gridSize.x, GameManager.instance.leaders[1].Teampoint / 100f);
        //        points.Add(graphPoint);
        //    }
        //    else if (gameObject.name.ToString() == "Line_Enemy3")
        //    {
        //        graphPoint = new Vector2(grid.gridSize.x, GameManager.instance.leaders[2].Teampoint / 100f);
        //        points.Add(graphPoint);
        //    }
        //    SetVerticesDirty();
        //}
    }
}