using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamPointText : TextManager
{
  
    // 1. 팀마다 포인트를 Slider와 Text로 시각화
    // 2. Slider는 본인 팀의 포인트 / 포인트가 제일 높은 팀의 포인트          


    private void Update()
    {
        Textarray[1].text = $"{(int)GameManager.instance.currentTime * 2}";
        Textarray[0].text = $"{(int)GameManager.instance.currentTime * 1}";
        Textarray[2].text = $"{(int)GameManager.instance.currentTime * 3}";
        Textarray[3].text = $"{(int)GameManager.instance.currentTime * 4}";

    }


}
