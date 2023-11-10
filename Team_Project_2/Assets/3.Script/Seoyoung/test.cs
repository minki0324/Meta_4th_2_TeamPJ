using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{

    public Material night_M;
    public Material evening_M;
    public Material afternoon_M;

   

    

    Color color;
    //public Skybox skybox;

    float duration = 3; //3초마다
    float smoothness = 0.1f; //부드러움? 색깔 변화 텀


    private void Awake()
    {

        RenderSettings.skybox = night_M;
        
    }
    private void Update()
    {

        

    }

   




    private IEnumerator Update_SkyBox_Night()
    {


        float progress = 0; //매개변수 대용

        float increment = smoothness / duration; //적용할 변경수준


        while (progress < 1)
        {
            Debug.Log(progress);
            color = Color.Lerp(afternoon_M.color, night_M.color, progress);
            RenderSettings.skybox.color = color;

            progress += increment;
            yield return new WaitForSeconds(smoothness);        //컬렉션 데이터를 하나씩 리턴
        }

    }
}
