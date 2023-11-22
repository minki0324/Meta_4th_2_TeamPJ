using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    //unitValue�� ���� ��ȯ�Ǵ� unit
    [SerializeField] private GameObject[] unit;
    [SerializeField] private Ply_Controller player;
    private LeaderAI leaderAI;
    [SerializeField] private GameObject targetLeader;

    //������ġ 3��
    public Transform[] SpawnPoint = new Transform[3];
    //������ġ�� 0~2 ����ġ ���ʴ�� ��ȯ�ϱ����� �ε���
    private int SpawnIndex = 0;
    private float SpawnCoolTime;
    //��ȯ�Ǵ� ����
    private int myLayer;
    private bool isAI;
    private bool isRespawning;
    // ���� ��� ���̾�
    private LayerMask TeamLayer;

    // Action 은 return이 없는 메소드만 추가한다
    public event Action OnEnemySpawn;
    // Function 은 return이 없는 메소드만 추가한다
    public event Func<int> OnEnemySpawn2;

    private void Awake()
    {
      
        TeamLayer = LayerMask.NameToLayer("Team");
        targetLeader = null;


        for (int i = 0; i < 3; i++)
        {
            SpawnPoint[i] = transform.GetChild(i); // �� �ڽ� ��ü�� �迭�� ����
        }
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Ply_Controller>();
    }
    private void Update()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }
        SpawnCoolTime += Time.deltaTime;

        if (gameObject.layer != transform.parent.gameObject.layer)
        {
            Debug.Log("레이어바꾸는거 계속들어옴");
            gameObject.layer = transform.parent.gameObject.layer;

            if (gameObject.layer == 0)
            {
                return;
            }
            else if (gameObject.layer == TeamLayer)
            {
                Debug.Log("몇번 부르나요");
                targetLeader = player.gameObject;
            }
            else
            {
                try
                {
                    targetLeader = SetLeader();
                    targetLeader.TryGetComponent(out leaderAI);
                }
                catch
                {
                    Debug.Log("Ÿ��ã������");
                }
            }
        }
        if (targetLeader != null)
        {

            if (targetLeader.layer == LayerMask.NameToLayer("Die"))
            {
                if (leaderAI.data.isDie && !isRespawning)
                {
                    StartCoroutine(RespawnAfterDelay(5f, gameObject.layer));
                }
            }
            if (targetLeader.gameObject.layer == TeamLayer)
            {
                isAI = false;

            }
            //Ÿ���� ���̾ƴ϶�� ��ȯ�ϴ� Ÿ���� AI�̴�
            else
            {
                isAI = true;

            }
        }


        if (isAI)
        {
            AIspawn();

        }






    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && gameObject.layer == other.gameObject.layer)
        {
            GameManager.instance.inRange = true;
            Ply_Controller ply = other.GetComponent<Ply_Controller>();
            ply.spawnPoint = gameObject.GetComponent<EnemySpawn>();
        }

        if (isAI && SpawnCoolTime >= 0.4f && leaderAI.Gold >= 350)
        {
            if (other.CompareTag("Leader") && other.gameObject.layer == gameObject.layer && leaderAI.canSpawn)
            {

                UnitSpawn();

                SpawnCoolTime = 0f;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && gameObject.layer == other.gameObject.layer)
        {
            GameManager.instance.inRange = false;
        }

    }
    private void UnitSpawn()
    {
        if (leaderAI.currentUnitCount > leaderAI.maxUnitCount)
        {
            return;
        }
        Unit_Information currentUnit = GameManager.instance.units[leaderAI.unitValue];
        GameObject newUnit = Instantiate(currentUnit.unitObject, SpawnPoint[SpawnIndex].position, Quaternion.identity);
        SetLayerRecursively(newUnit, leaderAI.gameObject.layer);
        Soilder_Controller soilder_Con = newUnit.GetComponent<Soilder_Controller>();
        soilder_Con.infodata = currentUnit;
        soilder_Con.Setunit();
        switch (targetLeader.gameObject.layer)
        {
            case 7:
                ColorManager.instance.RecursiveSearchAndSetUnit(newUnit.transform, GameManager.instance.T1_Color);
                GameManager.instance.leaders[0].Hire++;
                Upgrade_Set(0, soilder_Con);
                break;
            case 8:
                ColorManager.instance.RecursiveSearchAndSetUnit(newUnit.transform, GameManager.instance.T2_Color);
                GameManager.instance.leaders[1].Hire++;
                Upgrade_Set(1, soilder_Con);
                break;
            case 9:
                ColorManager.instance.RecursiveSearchAndSetUnit(newUnit.transform, GameManager.instance.T3_Color);
                GameManager.instance.leaders[2].Hire++;
                Upgrade_Set(2, soilder_Con);
                break;

        }
        
        
        leaderAI.UnitList.Add(newUnit);
        leaderAI.Gold -= currentUnit.cost;
        SpawnIndex++;

        leaderAI.currentUnitCount++;
        //������ġ�� ���ʴ�� �������ϱ����� �޼ҵ� 
        if (SpawnIndex > 2)
        {
            SpawnIndex = 0;
        }

    }
    
    private LeaderAI FindLeader()
    {
        GameObject[] objectsWithSameLayer = GameObject.FindGameObjectsWithTag("Leader"); // YourTag���� LeaderState ������Ʈ�� �ִ� ������Ʈ�� �±׸� �ֽ��ϴ�.

        // ã�� ������Ʈ �߿��� LeaderState ������Ʈ�� ���� ù ��° ������Ʈ�� ã���ϴ�.


        foreach (var obj in objectsWithSameLayer)
        {
            if (obj.gameObject.layer == gameObject.layer)
            {
                leaderAI = obj.GetComponent<LeaderAI>();

                if (leaderAI != null)
                {
                    return leaderAI;
                    // LeaderState�� ã���� ������ �����մϴ�.
                }
            }
        }

        if (leaderAI == null)
        {
            Debug.LogWarning("LeaderState ������Ʈ�� ���� ������Ʈ�� ã�� �� �����ϴ�.");
        }
        return null;
    }
    public void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer; // ���� ������Ʈ�� ���̾� ����

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer); // ���� ������Ʈ�� ���� ��� ȣ��
        }


    }
    private GameObject SetLeader()
    {

        if (myLayer != TeamLayer)
        {

            leaderAI = FindLeader();
            if (leaderAI != null)
            {
                targetLeader = leaderAI.gameObject;

            }
        }
        else
        {

            targetLeader = player.gameObject;


        }
        return targetLeader;
    }
    private void AIspawn()
    {
        if (leaderAI.data.isDie)
        {
            leaderAI.canSpawn = false;

            return;
        }

        if (leaderAI.maxUnitCount <= leaderAI.currentUnitCount || leaderAI.Gold <= leaderAI.unitCost)
        {
            leaderAI.canSpawn = false;
        }
        else
        {
            leaderAI.canSpawn = true;
        }
    }

    IEnumerator RespawnAfterDelay(float delay, int layer)
    {
        isRespawning = true;

        yield return new WaitForSeconds(delay);

        // ��Ȱ ������ ���⿡ ����
        // ���� ���, �׾��� ������ �ٽ� �����ϴ� ���� ������ ����

        // ��Ȱ�� �Ϸ�Ǹ� �ٽ� ��Ƴ� ������ �÷��׸� ����
        leaderAI.Respawn(targetLeader);

        // ���� ��Ȱ�� ���� �÷��׸� �ʱ�ȭ
        isRespawning = false;
    }
    private void Upgrade_Set(int Team, Soilder_Controller soilder_Controller)
    {
        if (GameManager.instance.leaders[Team].isUpgrade_SolDam)
        {
            soilder_Controller.data.damage = soilder_Controller.infodata.damage + 5;
        }

        if (GameManager.instance.leaders[Team].isUpgrade_SolHP)
        {
            soilder_Controller.data.currentHP = soilder_Controller.infodata.currentHP + 50;
            soilder_Controller.data.maxHP = soilder_Controller.infodata.maxHP + 50;
        }
    }


}
