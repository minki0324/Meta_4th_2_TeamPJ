using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flag : MonoBehaviour
{
    // 점령
    // 1. 점령 후 깃발 색 변경       

    private SkinnedMeshRenderer skinnedmesh;   
    
    public void Change_Flag_Color(int Teamcolor)
    { 
        skinnedmesh.material = ColorManager.instance.Flag_Color[Teamcolor];        
    }

   



}
