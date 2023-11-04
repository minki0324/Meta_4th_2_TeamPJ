using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSet : MonoBehaviour
{
    /*
         UI에서 색깔 지정시 인덱스를 받고 배열(컬러)에있는 인덱스 색깔을 불러옴
        현재 오브젝트 자식객체에 Renderer컴포넌트가 있는지 검사 
        ->컴포넌트가 있을시 Renderer material의 texture를 인덱스에 해당하는 컬러로 변경
        -> 검사하는 객체의 자식객체가 없을때까지 반복.(재귀함수)
    */

    //컬러배열
    [Header("적용할 컬러배열(Texture)")]
    [SerializeField] private Texture2D[] Color_Texture;

    //바꿀오브젝트 ->  혹시몰라서 넣어둠 필요없으면 빼도될듯 
    [Header("색깔을 바꿀 오브젝트")]
    [SerializeField] private Transform target;

    //배열 인덱스
    [Header("적용시킬 컬러배열의 인덱스")]
    [TextArea()] public string Color_Num;
    [SerializeField] public int Color_Index;    

    private void Start()
    {
        //참조안하고 스크립트가지고있는오브젝트한테 적용시킬거면 이거 적용하기
        RecursiveSearchAndSetTexture(transform , Color_Index);
        //if(player.TryGetComponent<Material>(out 

        // player 참조 걸땐 이거사용하기 
    }

    public void RecursiveSearchAndSetTexture(Transform currentTransform, int index)
    {
        foreach (Transform child in currentTransform)
        {
            if (child.gameObject.activeSelf)
            {
                Renderer renderer = child.GetComponent<Renderer>();

                if (renderer != null && child.tag != "Flag")
                {
                    Material material = renderer.material;
                    material.SetTexture("_BaseMap", Color_Texture[index]);
                }

                // 현재 자식 오브젝트의 하위 자식 오브젝트를 재귀적으로 검사

                RecursiveSearchAndSetTexture(child, index);
            }
        }
    }
}
