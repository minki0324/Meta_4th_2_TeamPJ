using System.Collections;
using UnityEngine;

public class Archer_Attack : MonoBehaviour
{
    // 궁수가 쏜 화살 구현

    [SerializeField] GameObject Arrow; // 화살 프리팹
    [SerializeField] float ArrowSpeed = 15f; // 화살 속도
    [SerializeField] private Rigidbody rigid; 
    private float graph = 5f;  // 포물선 보정수치

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameObject Arrow_ob = Instantiate(Arrow, transform.position + transform.forward, transform.rotation); // 두번째 매개변수에 아바타 활 넣어주십숑
            
            StartCoroutine(Arrow_rot(Arrow_ob));
        }
    }

    IEnumerator Arrow_rot(GameObject Shoot_A)
    {
        rigid = Shoot_A.GetComponent<Rigidbody>();
        rigid.AddForce(Shoot_A.transform.forward * ArrowSpeed, ForceMode.Impulse);


        while (true) // 나중에 사라지는 조건 넣기
        {
            Quaternion Tar_Q = new Quaternion();
            Tar_Q.eulerAngles = new Vector3(-(graph * rigid.velocity.y), 0, 0);


            Shoot_A.transform.rotation = Tar_Q;
            yield return null;
        }
        Destroy(Shoot_A, 4f);


    }



}
