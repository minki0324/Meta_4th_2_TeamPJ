using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    // 문을 열고 닫는 스크립트


    private List<GameObject> Soldiers;
    private Animator Gate_Ani;  
    private Collider Gate_Col;  // Gate 물리 Collider
    private bool isOpen = false;

    private void Awake()
    {
        Gate_Ani = GetComponent<Animator>();
        Gate_Col = GetComponent<Collider>();
        Gate_Ani.SetTrigger("OpenDoor");
        isOpen = true;
        Gate_Col.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        Soldiers.Add(other.gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        Soldiers.Remove(other.gameObject);
    }
    private void Update()
    {
        if(Soldiers.Count.Equals(0) && isOpen)
        {
            StartCoroutine(Gate_Interaction());
        }
    }

    // 게이트 상호작용
    public IEnumerator Gate_Interaction()
    {
        if (!isOpen) // 문이 닫혀있을 때
        {
            Debug.Log("문 열림");
            Gate_Ani.SetTrigger("OpenDoor");
            isOpen = true;
            Gate_Col.enabled = false;
        }
        else // 문이 열려있을 때
        {
            Debug.Log("문 닫힘");
            Gate_Ani.SetTrigger("CloseDoor");
            isOpen = false;
            Gate_Col.enabled = true;
        }
        yield return null;
        
    }
}
