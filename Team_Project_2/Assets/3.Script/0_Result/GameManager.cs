using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /*
        게임 매니저에서 관리해야 할 변수 목록
        1. 골드
        2. 플레이어 체력
        3. 점령지 (골드와 연동)
    */

    public static GameManager instance = null;

    public float currentTime = 0f; // 게임이 시작하고 지난 시간
    public float EndTime = 1800f; // 게임 시간은 30분

    public float Gold = 0; // 골드량
    private float Magnifi = 2f;  // 기본 골드 배율 (업데이트문 프레임 60 x 2f로 기본 획득 골드량은 분당 120)

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
