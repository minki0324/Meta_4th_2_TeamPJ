using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttoninfo : MonoBehaviour
{

    public Unit_Information unitInfo; // ScriptableObject를 참조하기 위한 필드
    public GameObject infoPanel; // 정보 패널을 참조하기 위한 필드

    private bool isHovering = false;

    private void Start()
    {
        infoPanel.SetActive(false);
    }

    private void Update()
    {
        if (isHovering)
        {
            // 마우스 포인터 위치를 가져옵니다.
            Vector3 mousePosition = Input.mousePosition;

            // 패널을 마우스 포인터 위치로 이동시킵니다.
            infoPanel.transform.position = mousePosition;

            // 패널을 활성화합니다.
            infoPanel.SetActive(true);
        }
        else
        {
            // 패널을 비활성화합니다.
            infoPanel.SetActive(false);
        }
    }

    private void OnMouseEnter()
    {
        isHovering = true;
        ShowUnitInfo(unitInfo);
    }

    private void OnMouseExit()
    {
        isHovering = false;
        HideUnitInfo();
    }

    // 정보를 패널에 표시하는 함수 (구현에 따라 다를 수 있음)
    private void ShowUnitInfo(Unit_Information info)
    {
        // info를 사용하여 정보를 패널에 표시
    }

    // 정보 패널을 비활성화하는 함수 (구현에 따라 다를 수 있음)
    private void HideUnitInfo()
    {
        // 정보 패널을 비활성화
    }
}


