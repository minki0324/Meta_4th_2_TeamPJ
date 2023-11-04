using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSet1 : MonoBehaviour
{
    /*
         UI���� ���� ������ �ε����� �ް� �迭(�÷�)���ִ� �ε��� ������ �ҷ���
        ���� ������Ʈ �ڽİ�ü�� Renderer������Ʈ�� �ִ��� �˻� 
        ->������Ʈ�� ������ Renderer material�� texture�� �ε����� �ش��ϴ� �÷��� ����
        -> �˻��ϴ� ��ü�� �ڽİ�ü�� ���������� �ݺ�.(����Լ�)
    */

    //�÷��迭
    [Header("������ �÷��迭(Texture)")]
    [SerializeField] private Texture2D[] Color_Texture;

    //�ٲܿ�����Ʈ ->  Ȥ�ø��� �־�� �ʿ������ �����ɵ� 
    [Header("������ �ٲ� ������Ʈ")]
    [SerializeField] private Transform player;

    //�迭 �ε���
    [Header("�����ų �÷��迭�� �ε���")]
    [TextArea()] public string Color_Num;

    private void Start()
    {
        //�������ϰ� ��ũ��Ʈ�������ִ¿�����Ʈ���� �����ų�Ÿ� �̰� �����ϱ�
        //RecursiveSearchAndSetTexture(transform);
        //if(player.TryGetComponent<Material>(out 
                
        // player ���� �ɶ� �̰Ż���ϱ� 
    }

    public void RecursiveSearchAndSetTexture(Transform currentTransform, int index)
    {
        foreach (Transform child in currentTransform)
        {
            if (child.gameObject.activeSelf)
            {
                Renderer renderer = child.GetComponent<Renderer>();

                if (renderer != null && child.tag != "Flag")
                {
                    Material material = renderer.material;
                    material.SetTexture("_BaseMap", Color_Texture[index]);
                }

                // ���� �ڽ� ������Ʈ�� ���� �ڽ� ������Ʈ�� ��������� �˻�
        
                RecursiveSearchAndSetTexture(child, index);
            }
        }
    }
}