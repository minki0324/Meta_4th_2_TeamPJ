using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInter : MonoBehaviour
{
    // 문을 열고 닫는 스크립트

    [SerializeField] private Animator Door_Ani;
    [SerializeField] private BoxCollider boxcol;  // 문 박스콜
    private bool isOpen = false;

    private void OnEnable()
    {
        Door_Ani.SetTrigger("OpenDoor");
        isOpen = true;
        boxcol.enabled = false;
    }

    public IEnumerator OpenDoor_co()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                if(isOpen)
                {
                    Debug.Log("닫어");
                    Door_Ani.SetTrigger("CloseDoor");
                    isOpen = false;
                    boxcol.enabled = true;
                }
                else
                {
                    Debug.Log("열어");
                    Door_Ani.SetTrigger("OpenDoor");
                    isOpen = true;
                    boxcol.enabled = false;
                }
            }
            yield return null;
        }
    }

}
