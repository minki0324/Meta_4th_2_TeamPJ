using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public float currentTime = 0; // 게임이 시작하고 지난 시간

    public float Gold = 0; // 골드량
    private float Magnifi = 2f;  // 처음 골드 배율

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

        Gold += Time.deltaTime * Magnifi; // 골드수급
    }
}
