using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldText : TextManager
{
    // ÇöÀç°ñµå HUD

    private void Update()
    {
        Textarray[0].text = $"°ñµå: {(int)GameManager.instance.Gold}";
    }

}
