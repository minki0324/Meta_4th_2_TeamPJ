using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    // ColorManager
    // 깃발, HUD 색을 팀 색으로 변경하기 위함
    
    public static ColorManager instance = null;

    //  0 : BK / 1 : BL / 2 : DB / 3 : BR / 4 : GR / 5 : GR_BL / 6 : PK / 7 : PP / 8 : RE / 9 : TAN / 10 : WH / 11 : YE
    //  0 : 검정 / 1 : 하늘 / 2 : 파랑 / 3 : 갈색 / 4 : 연두 / 5 : 초록
    //  6 : 핑크 / 7 : 보라 / 8 : 빨강 / 9 : 연갈색 / 10 : 흰색 / 11 : 노랑


    public Flag flag;
    public Color[] Teamcolor; // 팀 Color
    public Material[] Flag_Color; // 깃발 Material
    public Texture2D[] Unit_Texture; 
    public Texture2D[] Building_Texture; 


    private void Awake()    
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RecursiveSearchAndSetUnit(Transform currentTransform, int index)
    {
        foreach (Transform child in currentTransform)
        {
            if (child.gameObject.activeSelf)
            {
                Renderer renderer = child.GetComponent<Renderer>();

                if (renderer != null && child.tag != "Flag")
                {
                    Material material = renderer.material;
                    material.SetTexture("_BaseMap", Unit_Texture[index]);
                }

                // 현재 자식 오브젝트의 하위 자식 오브젝트를 재귀적으로 검사

                RecursiveSearchAndSetUnit(child, index);
            }
        }
    }
    public void RecursiveSearchAndSetBuilding(Transform currentTransform, int index)
    {
        foreach (Transform child in currentTransform)
        {
            if (child.gameObject.activeSelf)
            {
                Renderer renderer = child.GetComponent<Renderer>();

                if (renderer != null && child.tag != "Flag")
                {
                    Material material = renderer.material;
                    material.SetTexture("_BaseMap", Building_Texture[index]);
                }

                // 현재 자식 오브젝트의 하위 자식 오브젝트를 재귀적으로 검사

                RecursiveSearchAndSetBuilding(child, index);
            }
        }
    }
}
