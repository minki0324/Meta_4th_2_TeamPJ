using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Start_Btn : MonoBehaviour
{
    [SerializeField] private GameObject light;
    [SerializeField] private GameObject camera_;

   public void Click()
    {
        GameManager.instance.Color_Index = 2;
        GameManager.instance.T1_Color = 6;
        GameManager.instance.T2_Color = 8;
        GameManager.instance.T3_Color = 9;

        SceneManager.LoadScene(1);
    }
}
