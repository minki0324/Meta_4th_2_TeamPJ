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
        // ���� �ð� 20�� �϶�
        //�ڷ�ƾ ȣ�� �ֱ� 1�� => (�׽ð�/2) / 10; =>1
        //�� ������ 15�ʵ��� 0->1�� ������

        // ���� �ð� 30�� �϶�
        //�ڷ�ƾ ȣ�� �ֱ� 1�� -> (�׽ð�/2) / 10; =>1.5
        //�� ������

        float co_time =  (GameManager.instance.EndTime/2) / 100;
       
     
        while (true)
        {
            Debug.Log("?");
            
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
                Debug.Log("����");
                yield break;
            }
           
            yield return new WaitForSeconds(co_time);
        }
       
    }
}
