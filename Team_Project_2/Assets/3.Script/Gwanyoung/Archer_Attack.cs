using System.Collections;
using UnityEngine;

public class Archer_Attack : MonoBehaviour
{
    // 궁수가 쏜 화살 구현

    [SerializeField] GameObject Arrow; // 화살 프리팹
    

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameObject Arrow_ob = Instantiate(Arrow, transform.position + transform.forward, transform.rotation); // 두번째 매개변수에 아바타 활 넣어주십숑
        }
    }





}
