using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmployText : TextManager
{
    
     //고용할 기물들의 State, InputKey, Cost 표기
    
    private void Start()
    {
        // 모든 텍스트 나중에 변수로 바꿔주세용
        Textarray[0].text = $"♥80 /20\n고용 키: <color=#FF3E3E>1</color>\n비용: <color=#B7AF3D>15</color>"; // 민병
        Textarray[1].text = $"♥90 /20\n고용 키: <color=#FF3E3E>2</color>\n비용: <color=#B7AF3D>16</color>"; // 창병
        Textarray[2].text = $"♥100 /25\n고용 키: <color=#FF3E3E>3</color>\n비용: <color=#B7AF3D>20</color>"; // 군단
        Textarray[3].text = $"♥80 /20\n고용 키: <color=#FF3E3E>0</color>\n비용: <color=#B7AF3D>20</color>"; // 방어군
    }
}
