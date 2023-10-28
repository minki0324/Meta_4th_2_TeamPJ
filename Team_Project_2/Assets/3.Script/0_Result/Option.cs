using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : TextManager
{
    [SerializeField] private Slider[] slider;

    private void Update()
    {
        slider[0].value = 0 / 2f;
        slider[1].value = 0 / 2f;
        Textarray[0].text = "1.00";  // 마우스 감도 변수
        Textarray[1].text = "1.00";  // 마스터 음량 변수        
    }

}
