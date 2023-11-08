using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Occupation : MonoBehaviour
{
    // 점령
    // 1. 점령 후 깃발 색 변경
    // 2. 점령 후 포인트가 속도 변경
    // 3. 주변 유닛 수에 따른 점령 슬라이더 변경


    [Header("깃발")]
    [SerializeField] SkinnedMeshRenderer skinnedmesh;

    [Header("색 변경")]
    [SerializeField] private Material[] Flag_Color; // 깃발 색바꿀 Marterial
    [SerializeField] private Image[] Occu_Back; // 점령 중인 팀 색
    [SerializeField] private ColorSet color;
    [SerializeField] private Transform player;

    public Slider OccuValue; // 점령 게이지

    private float Num_Soldier = 1.03f; // 사람 수에 따른 배율
    public float occu_Speed = 15f; // 점령 속도
    private float Total_Gauge = 100f; // 전체 점령 게이지
    private float Current_Gauge = 0;  // 현재 점령 게이지

    public bool isOccupating = false; // 점령 중인지
    [SerializeField] private bool isOccupied = false; // 점령이 끝났는지

    private Ply_Controller ply_Con;
    private void Awake()
    {
        Occu_Back = GetComponentsInChildren<Image>();
        ply_Con = FindObjectOfType<Ply_Controller>();
        for (int i = 0; i < Occu_Back.Length * 0.5f; i++) 
        {
            Occu_Back[i * 2 + 1].transform.parent.gameObject.SetActive(false);

        }
    }

    private void Update()
    {
        if (OccuValue.value >= 1 && !isOccupied) 
        {
            Change_Color();
            GameManager.instance.Occupied_Area++;
            isOccupied = true;
        }
      
    }

    public void ObjEnable(bool act)
    {
        Occu_Back[1].transform.parent.gameObject.SetActive(act);
        OccuValue.gameObject.SetActive(act);
        isOccupating = act;
    }
    
      
    

    private void Change_Color()
    {
        // 나중에 컬러별로 수정
        skinnedmesh.material = Flag_Color[GameManager.instance.Color_Index];
        color.RecursiveSearchAndSetTexture(player, GameManager.instance.Color_Index);
        Occu_Back[0].color = new Color32(255, 0, 0, 110);
        Occu_Back[1].color = new Color32(255, 0, 0, 110);
    }
    


    public IEnumerator Occu_co()
    {

        // 점령 중
        while (isOccupating && Current_Gauge <= 100f)
        {
            Current_Gauge += Time.deltaTime * occu_Speed * Num_Soldier * (ply_Con.Current_MinionCount + 1); // 나중에 인원수에 따른 배율 넣어야해용
            Debug.Log(Current_Gauge);
            OccuValue.value = Current_Gauge / Total_Gauge;
            yield return null;
        }

    }
    public IEnumerator UnOccu_co()
    {
        //OccuValue.gameObject.SetActive(false);
        yield return new WaitForSeconds(3.0f);
        while (!isOccupied && !isOccupating && Current_Gauge >= 0f)
        {
            Current_Gauge -= Time.deltaTime * occu_Speed;
            OccuValue.value = Current_Gauge / Total_Gauge;



            yield return null;
        }
    }



}
