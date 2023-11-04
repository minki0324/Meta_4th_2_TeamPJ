using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_Trajectory : MonoBehaviour
{
    [SerializeField] float ArrowSpeed = 15f;
    [SerializeField] bool isDown = false;
    Rigidbody rigid;
    [SerializeField] Transform edge;



    public void Fire()
    {
        if (rigid == null) rigid = GetComponent<Rigidbody>();
        rigid.AddForce(-transform.up * ArrowSpeed, ForceMode.Impulse);
    }
  
    public IEnumerator Down_Arrow()
    {
        if (rigid == null) rigid = GetComponent<Rigidbody>();
        rigid.AddForce(-transform.up * ArrowSpeed, ForceMode.Impulse);
            yield return new WaitForSeconds(0.35f);
        while (rigid.velocity.y > 0.5f)
        {
            yield return null;
        }
        Quaternion tar_q = new Quaternion();
        tar_q.eulerAngles = new Vector3(300f, 0, 0);
        while(Quaternion.Angle(transform.rotation, tar_q)>0.01f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, tar_q, 2.5f * Time.deltaTime);
            yield return null;
        }
        transform.rotation = tar_q;
    }
     
}
