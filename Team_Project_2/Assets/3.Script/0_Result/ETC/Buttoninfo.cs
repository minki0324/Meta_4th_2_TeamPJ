using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Buttoninfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Unit_Information unitInfo;
    public GameObject infoPanel;
    public Text[] texts;

 



    //public float offsetX = f; // 왼쪽과 오른쪽으로의 오프셋
    //public float offsetY = 0f; // 위쪽과 아래쪽으로의 오프셋
    private void Start()
    {
        infoPanel.SetActive(false);
        texts = infoPanel.GetComponentsInChildren<Text>();
    }

  

    public void OnPointerEnter(PointerEventData eventData)
    {

        infoPanel.SetActive(true);

        ShowUnitInfo(unitInfo);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoPanel.SetActive(false);
        HideUnitInfo();
    }

    private void Update()
    {
        // Update 메서드에서는 추가적인 로직이 필요 없습니다.

        if (infoPanel.activeSelf) 
        {
            Vector3 mousePosition = Input.mousePosition;
            infoPanel.transform.position = mousePosition;
        }
    }

    private void ShowUnitInfo(Unit_Information info)
    {
        // info를 사용하여 정보를 패널에 표시
        //0 : 이름 
        //1 : 정보
        //2 : HP
        //3 : 공격력
        //4 : 비용
        //5 : 사거리

        texts[0].text = info.unitName;
        texts[2].text = $"{info.maxHP}\n{info.damage}\n{info.cost}\n{info.attackRange}";
        texts[3].text = $"{info.description}";
    }

    private void HideUnitInfo()
    {
        // 정보 패널을 비활성화
    }
   
}
