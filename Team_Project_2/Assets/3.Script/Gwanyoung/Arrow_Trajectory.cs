using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_Trajectory : MonoBehaviour
{
    [SerializeField] float ArrowSpeed = 12f; // 화살 속도
    private Rigidbody rigid;
    private float graph = 5f;  // 포물선 보정수치

    private void Start()
    {
        StartCoroutine(Arrow_rot());
    }


    IEnumerator Arrow_rot()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.AddForce(transform.forward * ArrowSpeed, ForceMode.Impulse);

        while (true) 
        {
            Quaternion Tar_Q = new Quaternion();
            Tar_Q.eulerAngles = new Vector3(-(graph * rigid.velocity.y), 0, 0);
           
            transform.rotation = Tar_Q;
            yield return null;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Weapon"))
        {
            Debug.Log("맞음");
            rigid.useGravity = false;

            rigid.constraints = RigidbodyConstraints.FreezeAll;

            Destroy(gameObject, 3f);
        }
    }
    
}
