using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas_OptionPanel : MonoBehaviour
{

    public static Canvas_OptionPanel instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
