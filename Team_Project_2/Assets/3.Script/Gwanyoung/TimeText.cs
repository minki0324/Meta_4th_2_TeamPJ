using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeText : TextManager
{
    /*
    1. 왼쪽에는 현재시간
    2. 오른쪽에는 게임설정시간
     */

    private void Awake()
    {
        Textarray[1].text = $"30:00"; // 나중에 설정시간 변수명 넣기   
    }
    void Update()
    {
        Textarray[0].text = string.Format("{0}:{1:00}", ((int)GameManager.instance.currentTime / 60), 
            ((int)GameManager.instance.currentTime) % 60); // 분 / 초
        
    }
}
