using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBarText : TextManager
{
    // HP Text와 Slider를 관리
    // 현재 체력, 전체 체력, 체력재생

    private float Total_HP = 150f;  // 임시 최대체력
    [Range(0f,150f)]
    public float Current_HP = 150f; // 임시 현재체력

    private float Regeneration = 0.5f;   // 임시 체력재생

    private Image SliderImg;
    public Gradient gradient;

    [SerializeField] private Slider HPBar;

    private void Awake()
    {
        SliderImg = HPBar.fillRect.GetComponent<Image>();
    }
    private void Update()
    {
        HPBar.value = Current_HP / Total_HP;

        Textarray[0].color = gradient.Evaluate(Current_HP / Total_HP);
        SliderImg.color = gradient.Evaluate(Current_HP / Total_HP); // 현재체력 비례 체력바 색변경
        Textarray[0].text = $"{(int)Current_HP}<color=FFFFFF>/{(int)Total_HP}</color>";   // 현재체력 / 총 체력
        Textarray[1].text = string.Format("+{0:0.00}", Regeneration);

    }


}
