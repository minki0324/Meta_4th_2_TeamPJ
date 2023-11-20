using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawLine : Graphic
{
    public Vector2Int gridSize;

    public DrawGrid grid;
    

    public List<Vector2> points;

    float width;
    float height;
    float unitWidth;
    float unitHeight;

    public float thickness = 10f;

    float current_Time = 0f;    //�ð� ��ȭ�� ����
    float cell_x = 0;
    float cell_y = 0;

    protected override void Start()
    {
        //points.Clear();
        points.Add(new Vector2(0, 0));
        //points.Add(new Vector2(cell_x + 1, 3));
        SetVerticesDirty();
    }
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        width = rectTransform.rect.width;
        height = rectTransform.rect.height;

        unitWidth = width / (float)gridSize.x;
        unitHeight = height / (float)gridSize.y;

      
        if(points[0].x !=0 || points[0].y != 0 )
        {
            points[0] = new Vector2(0, 0f);
        }

        if (points.Count < 2)
        {
            return;
        }

        float angle = 0;
        for (int i = 0; i < points.Count;  i++)
        {
            Vector2 point = points[i];

            //��������
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
        for(int i =0; i< GetColor.instance.teamColors.Length; i++)
        {
            if(num == GetColor.instance.teamColors[i].ColorNum)
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


    //���� ã�� �Լ�
    private void DrawPoint(Vector2 point, VertexHelper vh, float angle)
    {
        UIVertex vertex = UIVertex.simpleVert;
        if (gameObject.name.ToString() == "Line_Team1")
        {
            vertex.color = ColorManager.instance.Teamcolor[GameManager.instance.Color_Index];
            //Getcolor(GameManager.instance.Color_Index, ref vertex);
            //vertex.color = Get_color(GameManager.instance.Color_Index);
        }
        else if (gameObject.name.ToString() == "Line_Enemy1")
        {
            vertex.color = ColorManager.instance.Teamcolor[GameManager.instance.T1_Color];
            //Getcolor(GameManager.instance.T1_Color, ref vertex);
        }
        else if (gameObject.name.ToString() == "Line_Enemy2")
        {

            vertex.color = ColorManager.instance.Teamcolor[GameManager.instance.T2_Color];
            //   Getcolor(GameManager.instance.T2_Color, ref vertex);
        }
        else if (gameObject.name.ToString() == "Line_Enemy3")
        {
            vertex.color = ColorManager.instance.Teamcolor[GameManager.instance.T3_Color];
            //Getcolor(GameManager.instance.T3_Color, ref vertex);
        }
        //vertex.color = color;

        vertex.position = Quaternion.Euler(0, 0, angle) * new Vector3(-thickness / 2, 0);
        vertex.position += new Vector3(unitWidth * point.x, unitHeight * point.y);
        vh.AddVert(vertex);


        vertex.position = Quaternion.Euler(0, 0, angle) * new Vector3(thickness / 2, 0);
        vertex.position += new Vector3(unitWidth * point.x, unitHeight * point.y);
        vh.AddVert(vertex);
    }

    private void Update()
    {
        if(grid != null)
        {
            if(gridSize != grid.gridSize)
            {
                gridSize = grid.gridSize;
                SetVerticesDirty();
            }
        }


        Draw_TeamPointLine();
    }


    public void Draw_TeamPointLine()
    {
        //�ð�(x), ������Ʈ(y)�߰�
        if (current_Time + 2 <= GameManager.instance.currentTime)
        {

            if (gameObject.name.ToString() == "Line_Team1")
            {
                cell_x += 2;
                cell_y += GameManager.instance.Teampoint;
               
            }
            else if (gameObject.name.ToString() == "Line_Enemy1")
            {

            }
            else if (gameObject.name.ToString() == "Line_Enemy2")
            {

            }
            else if (gameObject.name.ToString() == "Line_Enemy3")
            {

            }

            SetVerticesDirty();

        }
       
    }
}
