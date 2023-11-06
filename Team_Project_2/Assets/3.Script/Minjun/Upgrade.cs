using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    private bool isTab;
    [SerializeField]private Image UI;
    //공격력 체력 이동속도 체젠
    //플레이어 관련업글
    //병사 관련업글
    //점령속도
    //병사인구증가   -> 완료
    //병사밸류증가   -> 완료
    //리스폰시간단축


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
        GameManager.instance.Max_MinionCount = 24;
        Debug.Log(GameManager.instance.Max_MinionCount);
    }
    public void UpgradeUnitValueUp()
    {
        //첫번째 업그레이드 안되있을때 
        if (!GameManager.instance.isPossible_Upgrade_1) { 
        GameManager.instance.isPossible_Upgrade_1 = true;
        Debug.Log("1번째 업그레이드 완료");
        }
        //첫번째 업그레이드 되있을때 
        if (GameManager.instance.isPossible_Upgrade_1 && !GameManager.instance.isPossible_Upgrade_2)
        {
            GameManager.instance.isPossible_Upgrade_2 = true;
            Debug.Log("2번째 업그레이드 완료");
        }
    }
    public void UpgradeRespawnTime()
    {
        GameManager.instance.respawnTime = 3f;
        Debug.Log("리스폰시간 3초로 감소");
    }
    public void UpgradeAtk()
    {
        int UpgradeCount = 0;
        UpgradeCount++;
        GameManager.instance.Damage += 5f;
        Debug.Log("데미지 5증가");
    }
    public void UpgradeHP()
    {
        int UpgradeCount = 0;
        UpgradeCount++;
        GameManager.instance.Max_Hp += 10f;
        GameManager.instance.Current_HP += 10f;
        Debug.Log("HP10증가");

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
