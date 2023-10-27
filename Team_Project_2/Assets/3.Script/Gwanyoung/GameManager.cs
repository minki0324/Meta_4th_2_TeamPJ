using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public float currentTime = 0;
    public float Gold = 0;

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
    // 기존 골드 상승량
    // 점령 어드벤티지
    // 골드 상승량 업그레이드
    
    private void Update()
    {
        currentTime += Time.deltaTime;

        Gold += Time.deltaTime; // 골드수급
    }
}
