using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    private bool isTab;
    [SerializeField] private Image UI;
    [SerializeField] private Text Damage;
    [SerializeField] private Text HP;
    [SerializeField] private GameObject Upgrade1_Ob;
    [SerializeField] private GameObject Upgrade2_Ob;
    [SerializeField] private GameObject Content;
    private Button[] buttons;
    int DamageUpgradeCount = 0;
    int HPUpgradeCount = 0;
    //공격력 체력 이동속도 체젠
    //플레이어 관련업글
    //병사 관련업글
    //점령속도
    //병사인구증가   -> 완료
    //병사밸류증가   -> 완료
    //리스폰시간단축
    private void Awake()
    {
        buttons = Content.GetComponentsInChildren<Button>();
       
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!isTab)
            {
                GameManager.instance.Stop();
                UI.gameObject.SetActive(true);
            }
            else
            {
                GameManager.instance.Resume();
                Time.timeScale = 1f;
                UI.gameObject.SetActive(false);
            }
            isTab = !isTab;


        }
    }
    public void UpgradeMaxCountUp()
    {
        if (GameManager.instance.Gold >= 200)
        {
            GameManager.instance.Gold -= 200;
            GameManager.instance.Max_MinionCount = 24;
            buttons[0].interactable = false;
            Debug.Log(GameManager.instance.Max_MinionCount);
        }
        else
        {
            
            Debug.Log("골드가 부족합니다.");
            return;
        }


    }
    public void UpgradeUnitValueUp()
    {
        if (GameManager.instance.Gold >= 200)
        {
            GameManager.instance.Gold -= 200;
            //첫번째 업그레이드 안되있을때 
            if (!GameManager.instance.isPossible_Upgrade_1)
            {
                GameManager.instance.isPossible_Upgrade_1 = true;
                Upgrade1_Ob.SetActive(true);
                buttons[1].interactable = false;
                Debug.Log("1번째 업그레이드 완료");
            }
            else       //첫번째 업그레이드 되있을때 
            if (GameManager.instance.isPossible_Upgrade_1 && !GameManager.instance.isPossible_Upgrade_2)
            {
                GameManager.instance.isPossible_Upgrade_2 = true;
                Upgrade2_Ob.SetActive(true);
                buttons[2].interactable = false;
                Debug.Log("2번째 업그레이드 완료");
            }
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }
    public void UpgradeRespawnTime()
    {
        if (GameManager.instance.Gold >= 200)
        {
            GameManager.instance.Gold -= 200;
            GameManager.instance.respawnTime = 3f;
            buttons[3].interactable = false;
            Debug.Log("리스폰시간 3초로 감소");
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }
    public void UpgradeAtk()
    {
        if (GameManager.instance.Gold >= 200)
        {
            GameManager.instance.Gold -= 200;
            if (DamageUpgradeCount < 5)
            {
                DamageUpgradeCount++;
                int currentUpgrade = DamageUpgradeCount * 5;
                GameManager.instance.Damage += 5f;
                Debug.Log("데미지 5증가");
                Damage.text = string.Format("영웅의 공격력이 5 증가합니다. 현재 {0}% / 최대25", currentUpgrade);
                if (DamageUpgradeCount == 5)
                {
                    buttons[4].interactable = false;
                }
            }
            else
            {
                Debug.Log("업그레이드 최대치에 도달했습니다.");


            }
         
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }
    public void UpgradeHP()
    {
        if (GameManager.instance.Gold >= 200)
        {
            GameManager.instance.Gold -= 200;
            if (HPUpgradeCount < 5)
            {
                HPUpgradeCount++;
                int currentUpgrade = HPUpgradeCount * 10;
                GameManager.instance.Max_Hp += 10f;
                GameManager.instance.Current_HP += 10f;
                Debug.Log("HP10증가");
                if (HPUpgradeCount == 5)
                {
                    buttons[5].interactable = false;
                }
                HP.text = string.Format("영웅의 HP가 10% 상승합니다. 현재 {0}% / 최대50%", currentUpgrade);
            }
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }
    //Astar완성후 다시 작성
    public void UpgradeSpeed()
    {

    }
    public void UpgradeRegeneration()
    {
        GameManager.instance.Regeneration += 0.5f;
        Debug.Log("체력리젠 0.5증가");

    }


}
