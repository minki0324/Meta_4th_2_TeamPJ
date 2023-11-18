using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OccupationHUD : MonoBehaviour
{
    // ���� HUD
    // 1. ������ ������ �ΰ��� ��ܿ� ������Ȳ ImageColor ����
    // 2. �÷��̾����״� ���� �� Slider�� UI ǥ�õǵ���   

    [SerializeField] GameObject[] Occu_image = null;
    [SerializeField] private Image[] Occu_Img_Color; // ���� ���� �� ��
    public Flag[] FlagArray;  // �÷��� ������Ʈ �迭
    public Slider[] OccuSlider;
    private Color ColorTemp;

    private void Start()
    {
        Occu_image = new GameObject[7];
        Occu_Img_Color = GetComponentsInChildren<Image>();
        OccuSlider = GetComponentsInChildren<Slider>();
       
        for (int i = 0; i < Occu_Img_Color.Length * 0.5f; i++) 
        {
            Occu_Img_Color[(i * 2) + 1].transform.parent.gameObject.SetActive(false);
           
        }
    }    

    public void Occu_Set()
    {
        FlagArray = FindObjectsOfType<Flag>();

        // �÷��׸��� ��ȣ�ο�
        for (int i = 0; i < FlagArray.Length; i++)
        {
            FlagArray[i].Flag_Num = i;
        }

        // ��� ������Ȳ ��ġ����
        for (int i = 0; i < FlagArray.Length; i++)
        {
            Occu_image[i] = Occu_Img_Color[i * 4].transform.parent.gameObject;
            Occu_image[i].transform.localPosition = new Vector3((-50 * FlagArray.Length * 0.5f) + (50 * i), 0, 0);
        }

        // ��� ������Ȳ setActive 
        for (int i = FlagArray.Length; i < 7; i++)
        {
            Occu_image[i] = Occu_Img_Color[i * 4].transform.parent.gameObject;
            Occu_image[i].SetActive(false);
        }

    }

    public void Ply_Slider(int TeamColor, int FlagNum, float Current, float Total)
    {
        ColorTemp = ColorManager.instance.Teamcolor[TeamColor];
        ColorTemp.a = 0.431f;

        Occu_Img_Color[FlagNum * 4 + 2].color = ColorTemp; 
        OccuSlider[FlagNum].value = Current / Total;   // �����̴� ���� ������
    }
 
    public void Ply_OccuHUD(int FlagNum, bool Act)
    {
        // �������� ������ Player �� �� SetActive�� �Ұ��� �ֱ� ���� �޼���
        Occu_Img_Color[FlagNum * 4 + 1].transform.parent.gameObject.SetActive(Act);  // ���� �߾��� HUD
        Occu_Img_Color[FlagNum * 4 + 3].transform.parent.gameObject.SetActive(Act);  // ���� �����̴�
    }

    public void Change_Color(int TeamColor, int FlagNum)
    {        
        ColorTemp = ColorManager.instance.Teamcolor[TeamColor];
        ColorTemp.a = 0.431f;

        Occu_Img_Color[FlagNum * 4].color = ColorTemp; // HUD ���               

        Occu_Img_Color[FlagNum * 4 + 1].color = ColorTemp; // �÷��̾�� �� HUD   

        FlagArray[FlagNum].Change_Flag_Color(TeamColor); // ��� �� ����

    }


}
