using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkyBox_Controller : MonoBehaviour
{

    public Material night_M;
    public Material evening_M;
    public Material afternoon_M;




    public Material current_material;

    float duration = 3; //3초마다
    float smoothness = 0.1f; //부드러움? 색깔 변화 텀


    void Start()
    {
        current_material = afternoon_M;
        gameObject.GetComponent<Skybox>().material = current_material;
 

        StartCoroutine(Update_SkyBox_Night());
    }



    private void Update()
    {
        //if (GameManager.instance.currentTime >= 3f)
        //{
        //    //this.gameObject.GetComponent<Skybox>().material = night_M;
        //    StartCoroutine(Update_SkyBox_Night());
        //}



    }






    private IEnumerator Update_SkyBox_Night()
    {
       
        float progress = 0; //매개변수 대용

        float increment = smoothness / duration; //적용할 변경수준

     
        while (progress < 1)
        {
            Debug.Log(progress);
            //current_material.Lerp(afternoon_M, night_M, progress);
            gameObject.GetComponent<Skybox>().material.Lerp(afternoon_M, night_M, progress);
           // Debug.Log(this.gameObject.GetComponent<Skybox>().material);
            progress += increment;
            yield return new WaitForSeconds(smoothness);        //컬렉션 데이터를 하나씩 리턴
        }
       
    }
}
