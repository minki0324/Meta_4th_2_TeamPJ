using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    // ���� ���� �ݴ� ��ũ��Ʈ

    private Animator Gate_Ani;  
    private BoxCollider Gate_Col;  // Gate ���� Collider
    private List<int> Units = new List<int>();
    private Unit_Gate ply_gate;
    private bool isOpen = true;

    [SerializeField]
    private AudioSource GateInter;  //����Ʈ ��ȣ�ۿ� ����


    private void Start()
    {
        Gate_Ani = GetComponentInParent<Animator>();
        Gate_Col = GetComponentInParent<BoxCollider>();
        Gate_Ani.SetTrigger("OpenDoor");
        Gate_Col.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Soldier") || other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Leader")) 
        {            
            if (!other.gameObject.layer.Equals(transform.root.gameObject.layer))
            {
                Units.Add(other.gameObject.layer);
            }
            else
            {
                if (other.gameObject.CompareTag("Player"))
                {
                    ply_gate = other.gameObject.GetComponent<Unit_Gate>();
                }
            }
        }        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Soldier") || other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Leader"))
        {
            if (!other.gameObject.layer.Equals(transform.root.gameObject.layer))
            {
                Units.Remove(other.gameObject.layer);
            }
            else
            {
                if (other.gameObject.CompareTag("Player"))
                {
                    ply_gate = null;
                }
            }
        }
    }
    private void Update()
    {
        if (!GameManager.instance.isLive) 
        {
            return;
        }

        if (ply_gate != null)
        {
            if (ply_gate.isMyGate && Input.GetKeyDown(KeyCode.J) && isOpen)
            {
                GateInter.PlayOneShot(AudioManager.instance.clip_SFX[((int)SFXList.Gate_Inter)]);
                
                Gate_Ani.SetTrigger("CloseDoor");
                isOpen = false;
                Gate_Col.enabled = true;
            }
            else if (ply_gate.isMyGate && Input.GetKeyDown(KeyCode.J) && !isOpen)
            {
                GateInter.PlayOneShot(AudioManager.instance.clip_SFX[((int)SFXList.Gate_Inter)]);
                Gate_Ani.SetTrigger("OpenDoor");
                isOpen = true;
                Gate_Col.enabled = false;
            }
        }
        if (Units.Count > 0 && isOpen)
        {
            GateInter.PlayOneShot(AudioManager.instance.clip_SFX[((int)SFXList.Gate_Inter)]);
            Gate_Ani.SetTrigger("CloseDoor");
            isOpen = false;
            Gate_Col.enabled = true;
        }
        else if (Units.Count.Equals(0) && !isOpen && !transform.root.gameObject.layer.Equals((int)TeamLayerIdx.Player)) 
        {
            GateInter.PlayOneShot(AudioManager.instance.clip_SFX[((int)SFXList.Gate_Inter)]);
            Gate_Ani.SetTrigger("OpenDoor");
            isOpen = true;
            Gate_Col.enabled = false;
        }
    }
}
