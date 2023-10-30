using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ply_Inter : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject OpenDoorui;
    private void FixedUpdate()
    {
        gameObject.transform.position = Player.transform.position;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Door"))
        {
            OpenDoorui.SetActive(true);
            if (Input.GetKeyDown(KeyCode.H))
            {
                Destroy(other.gameObject); // 임시로 제거 집에가서 파티클 찾아보고 바꿀게용
            }
        }
        else
        {
            OpenDoorui.SetActive(false);
        }
    }
}
