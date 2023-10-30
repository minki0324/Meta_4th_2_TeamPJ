using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /*
        ���� �Ŵ������� �����ؾ� �� ���� ���
        1. ���
        2. �÷��̾� ü��
        3. ������ (���� ����)
    */

    public static GameManager instance = null;

    [Header("���� �÷���")]
    public float currentTime = 0f; // ������ �����ϰ� ���� �ð�
    public float EndTime = 1800f; // ���� �ð��� 30��

    [Header("��� ����")]
    public float Gold = 0; // ��差
    private float Magnifi = 2f;  // �⺻ ��� ���� (������Ʈ�� ������ 60 x 2f�� �⺻ ȹ�� ��差�� �д� 120)
    
    [Header("�÷��̾� ����")]
    public bool isLive = true;
    public float Current_HP = 150f;
    public float Max_Hp = 150f;
    public float Regeneration = 0.5f;

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
    // ���� ��� ��·�
    // ���� ��庥Ƽ��
    // ��� ��·� ���׷��̵�
    
    private void Update()
    {
        currentTime += Time.deltaTime;

        Gold += Time.deltaTime * Magnifi; // ������
    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
    }
    
    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
    }
}