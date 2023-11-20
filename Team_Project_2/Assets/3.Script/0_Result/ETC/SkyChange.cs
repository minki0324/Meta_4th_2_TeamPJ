using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyChange : MonoBehaviour
{

    float day_currentRange = 0;
    float night_currentRange = 0;
    
    [SerializeField]
    Material DayToSunset_skybox;

    [SerializeField]
    Material SunsetToNihgt_skybox;

    private void Start()
    {
        RenderSettings.skybox = DayToSunset_skybox;
        DayToSunset_skybox.SetFloat("_Blend", 0);
        SunsetToNihgt_skybox.SetFloat("_Blend", 0);
        StartCoroutine(ChangeSky());

    }

    private IEnumerator ChangeSky()
    {
        // 게임 시간 20초 일때
        //코루틴 호출 주기 1초 => (겜시간/2) / 10; =>1
        //값 증가량 15초동안 0->1로 가야함

        // 게임 시간 30초 일때
        //코루틴 호출 주기 1초 -> (겜시간/2) / 10; =>1.5
        //값 증가량

        float co_time =  (GameManager.instance.EndTime/2) / 100;
       
     
        while (true)
        {
            
            if (GameManager.instance.currentTime <= GameManager.instance.EndTime / 2)
            {
                day_currentRange += 0.01f;
                DayToSunset_skybox.SetFloat("_Blend", day_currentRange);              
            }
            else
            {
                night_currentRange += 0.01f;
                RenderSettings.skybox = SunsetToNihgt_skybox;
                SunsetToNihgt_skybox.SetFloat("_Blend", night_currentRange);
            }

        

            if (co_time == GameManager.instance.EndTime)
            {
                Debug.Log("끝남");
                yield break;
            }
           
            yield return new WaitForSeconds(co_time);
        }
       
    }
}
