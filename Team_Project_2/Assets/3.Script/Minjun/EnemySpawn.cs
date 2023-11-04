using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    //unitValue에 따라 소환되는 unit
    [SerializeField] private GameObject[] unit;
    private LeaderState leaderState;
    //스폰위치 3개
    private Transform[] SpawnPoint = new Transform[3];
    //스폰위치를 0~2 번위치 차례대로 소환하기위한 인덱스
    private int SpawnIndex = 0;
    //소환되는 간격
    private float Spawninterval = 0.4f;
    private void Awake()
    {
        leaderState = FindLeader();

        for (int i = 0; i < 3; i++)
        {
            SpawnPoint[i] = transform.GetChild(i); // 각 자식 객체를 배열에 저장
        }
    }
    private void Update()
    {
        if (leaderState.isDead)
        {
            return;
        }

       
     

        //유닛카운트가 맥스가 됬거나 , 유닛비용보다 가진 골드가 적을때 false;
        if(leaderState.maxUnitCount <= leaderState.currentUnitCount || leaderState.Gold <= leaderState.unitCost)
        {
            leaderState.canSpawn = false;
        }
        else
        {
            leaderState.canSpawn = true;
        }
       





        }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Leader") && other.gameObject.layer == gameObject.layer && leaderState.canSpawn)
        {
            Debug.Log("유닛스폰 시작");
            InvokeRepeating("UnitSpawn", 0f, Spawninterval);


        }
        else if(!leaderState.canSpawn)  
        {
            Debug.Log("canSPawn false로 스폰중지");
            CancelInvoke("UnitSpawn");
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Leader") && other.gameObject.layer == gameObject.layer)
        {
            Debug.Log("리더가 나가서 중지");
            CancelInvoke("UnitSpawn");
        }
       
    }
    private void UnitSpawn()
    {
        GameObject newUnit = Instantiate(unit[leaderState.unitValue], SpawnPoint[SpawnIndex].position, Quaternion.identity);
        SetLayerRecursively(newUnit, leaderState.gameObject.layer);

        leaderState.UnitList.Add(newUnit);
        leaderState.Gold -= leaderState.unitCost;
        SpawnIndex++;
        
        leaderState.currentUnitCount++;
        //스폰위치를 차례대로 나오게하기위한 메소드 
        if (SpawnIndex > 2)
        {
            SpawnIndex = 0;
        }

    }
    private LeaderState FindLeader()
    {
        GameObject[] objectsWithSameLayer = GameObject.FindGameObjectsWithTag("Leader"); // YourTag에는 LeaderState 컴포넌트가 있는 오브젝트의 태그를 넣습니다.

        // 찾은 오브젝트 중에서 LeaderState 컴포넌트를 가진 첫 번째 오브젝트를 찾습니다.


        foreach (var obj in objectsWithSameLayer)
        {
            if (obj.gameObject.layer == gameObject.layer)
            {
                leaderState = obj.GetComponent<LeaderState>();

                if (leaderState != null)
                {
                    return leaderState;
                    // LeaderState를 찾으면 루프를 종료합니다.
                }
            }
        }

        if (leaderState == null)
        {
            Debug.LogWarning("LeaderState 컴포넌트를 가진 오브젝트를 찾을 수 없습니다.");
        }
        return null;
    }
    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer; // 현재 오브젝트의 레이어 변경

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer); // 하위 오브젝트에 대해 재귀 호출
        }
    }

}
