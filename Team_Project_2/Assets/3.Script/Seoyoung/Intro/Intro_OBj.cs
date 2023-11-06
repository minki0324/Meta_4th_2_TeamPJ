using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro_OBj : MonoBehaviour
{
    //unitValue�� ���� ��ȯ�Ǵ� unit
    [SerializeField] private GameObject[] unit;
    private LeaderState leaderState;
    //������ġ 3��
    private Transform[] SpawnPoint = new Transform[3];
    //������ġ�� 0~2 ����ġ ���ʴ�� ��ȯ�ϱ����� �ε���
    private int SpawnIndex = 0;
    //��ȯ�Ǵ� ����
    private float Spawninterval = 0.4f;
    private void Awake()
    {
        leaderState = FindLeader();

        for (int i = 0; i < 3; i++)
        {
            SpawnPoint[i] = transform.GetChild(i); // �� �ڽ� ��ü�� �迭�� ����
        }
    }
    private void Update()
    {
        if (leaderState.isDead)
        {
            leaderState.canSpawn = false;

            return;
        }




        //����ī��Ʈ�� �ƽ��� ��ų� , ���ֺ�뺸�� ���� ��尡 ������ false;
        if (leaderState.maxUnitCount <= leaderState.currentUnitCount || leaderState.Gold <= leaderState.unitCost /*|| leaderState.isDead*/)
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
       

            InvokeRepeating("UnitSpawn", 0f, Spawninterval);


     
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Leader") && other.gameObject.layer == gameObject.layer)
        {

            CancelInvoke("UnitSpawn");
        }

    }
    private void UnitSpawn()
    {
        if (leaderState.currentUnitCount > 40)
        {
            return;
        }
        GameObject newUnit = Instantiate(unit[leaderState.unitValue], SpawnPoint[SpawnIndex].position, Quaternion.identity);
        SetLayerRecursively(newUnit, leaderState.gameObject.layer);
        SetColar(newUnit);


        leaderState.UnitList.Add(newUnit);
        //leaderState.Gold -= leaderState.unitCost;
        
        SpawnIndex++;

        leaderState.currentUnitCount++;
        //������ġ�� ���ʴ�� �������ϱ����� �޼ҵ� 
        if (SpawnIndex > 2)
        {
            SpawnIndex = 0;
        }

    }
    private LeaderState FindLeader()
    {
        GameObject[] objectsWithSameLayer = GameObject.FindGameObjectsWithTag("Leader"); // YourTag���� LeaderState ������Ʈ�� �ִ� ������Ʈ�� �±׸� �ֽ��ϴ�.

        // ã�� ������Ʈ �߿��� LeaderState ������Ʈ�� ���� ù ��° ������Ʈ�� ã���ϴ�.


        foreach (var obj in objectsWithSameLayer)
        {
            if (obj.gameObject.layer == gameObject.layer)
            {
                leaderState = obj.GetComponent<LeaderState>();

                if (leaderState != null)
                {
                    return leaderState;
                    // LeaderState�� ã���� ������ �����մϴ�.
                }
            }
        }

        if (leaderState == null)
        {
            Debug.LogWarning("LeaderState ������Ʈ�� ���� ������Ʈ�� ã�� �� �����ϴ�.");
        }
        return null;
    }
    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer; // ���� ������Ʈ�� ���̾� ����

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer); // ���� ������Ʈ�� ���� ��� ȣ��
        }


    }
    private void SetColar(GameObject newUnit)
    {
        ColorSet unitColorSet = newUnit.gameObject.GetComponent<ColorSet>();

        ColorSet leaderColorSet = leaderState.gameObject.GetComponent<ColorSet>();
        unitColorSet.Color_Index = leaderColorSet.Color_Index;


    }
}
