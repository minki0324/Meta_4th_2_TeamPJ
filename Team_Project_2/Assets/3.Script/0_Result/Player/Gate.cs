using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    // 문을 열고 닫는 스크립트


    private Animator Gate_Ani;  
    [SerializeField] private Collider Gate_Col;  // Gate 물리 Collider
    private bool isOpen = false;
    WaitForSeconds DoorCool = new WaitForSeconds(2f);
    private List<int> Units = new List<int>();


    private void Awake()
    {
        Gate_Ani = GetComponentInParent<Animator>();
        Gate_Col = GetComponentInParent<Collider>();
        Gate_Ani.SetTrigger("OpenDoor");
        isOpen = true;
        Gate_Col.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }



    // 게이트 상호작용
    public IEnumerator Gate_Interaction()
    {
        if (!isOpen) // 문이 닫혀있을 때
        {
            Debug.Log("문 열림");
            Gate_Ani.SetTrigger("OpenDoor");
            yield return DoorCool;
            isOpen = true;
            Gate_Col.enabled = false;
        }
        else // 문이 열려있을 때
        {
            Debug.Log("문 닫힘");
            Gate_Ani.SetTrigger("CloseDoor");
            yield return DoorCool;
            isOpen = false;
            Gate_Col.enabled = true;
        }
    }
}
