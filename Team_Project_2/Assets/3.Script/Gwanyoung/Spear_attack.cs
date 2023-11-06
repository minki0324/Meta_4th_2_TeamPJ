using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear_attack : MonoBehaviour
{
    private float SpearSpeed = 1.3f; // 화살 속도
    private Rigidbody rigid;
    private float graph = 5f;  // 포물선 보정수치

    private void Start()
    {
        StartCoroutine(Spear_throw());
    }


    IEnumerator Spear_throw()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.AddForce(transform.forward * SpearSpeed, ForceMode.Impulse);

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
        if (!other.CompareTag("Weapon") && !other.gameObject.Equals(transform.parent))
        {
            Debug.Log("맞음");
            rigid.useGravity = false;

            rigid.constraints = RigidbodyConstraints.FreezeAll;

            Destroy(gameObject, 3f);
        }
    }
}
