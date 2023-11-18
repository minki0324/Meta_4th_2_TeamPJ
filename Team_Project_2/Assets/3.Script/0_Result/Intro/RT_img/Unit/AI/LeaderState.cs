using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderState : Unit
{

    public enum BattleState
    {

        //Follow, //������ �̵��Ҷ� 
        Attack,  // AI�� ���� �����ϰ� �����ð� �Ǵ� �Ÿ��������� 
        Search,
        Move,
        Defense,
        Detect


    }
    public float Teampoint = 0;

    [Header("��� ����")]
    public float total_Gold = 500;
    public float Gold = 500; // ��差
    public float Upgrade_GoldValue = 1f;
    // private float Magnifi = 2f;  // �⺻ ��� ���� (������Ʈ�� ������ 60 x 2f�� �⺻ ȹ�� ��差�� �д� 120)

    [Header("AI ����")]
    // private bool Ready =true;
    public float Regeneration = 0.5f;
    public int maxUnitCount = 19;
    public int currentUnitCount = 0;
    public int killCount = 0;
    public int deathCount = 0;
    public int unitValue = 0;
    public float unitCost = 16f;
    public bool canSpawn;
    public bool isMoving;
    public Transform respawnPoint;
    //EnemySpawn respawnPoint;
    public BattleState bat_State;
    public int has_Flag = 0;
    public int Hire = 0;
    public bool isPossible_Upgrade_1 = false;
    public bool isPossible_Upgrade_2 = false;

    public bool isUpgrade_SolDam = false;
    public bool isUpgrade_SolHP = false;
    public List<int> Upgrade_List = new List<int>();
    public List<GameObject> UnitList = new List<GameObject>();

   
    public override void Die()
    {
     
    }
    //AI �ൿ �켱����
    /*
     1. �߸������� ������
     2. �߸������� �ƴ����� �ƹ��� ������ 
     3. 
     
     1.����
     2. �ƹ��������� ����
     
     */
    public void Respawn(GameObject leader)
    {
        //�ִϸ��̼��ʱ�ȭ
        //HP , �ݶ��̴� , isDead ,���̾� �ٽü���
        //������ ������ ��ġ�� �̵�
        aipath.canMove = false;
        aipath.canSearch = false;
        ani.SetTrigger("Reset");
        ani.SetLayerWeight(1, 1);
        data.currentHP = data.maxHP;
        data.isDie = false;
        Debug.Log(respawnPoint.parent.gameObject.layer);
        leader.layer = respawnPoint.parent.gameObject.layer;
        leader.transform.position = respawnPoint.position;


    }
    public override void HitDamage(float damage)
    {
       
    }
    public override void Lostleader()
    {
       
    }
}
