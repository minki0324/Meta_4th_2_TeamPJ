using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierText : TextManager
{       
    // 현재 병사관리

    private void Update()
    {
        Textarray[0].text = "0"; // 현재 병사 무슨병사인지 모르겠어요 ,, 기수인가 ? 아무튼 그거 변수넣기 ..
        Textarray[1].text = "00"; // 현재 병사수 변수 넣어야해용
        
    }
}
