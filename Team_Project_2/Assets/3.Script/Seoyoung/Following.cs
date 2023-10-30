using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Following : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> nearestMinion_List = new List<GameObject>();
    public LayerMask TargetLayer;

   // private UnityEngine.AI.NavMeshAgent[] agents;

    private Player_Controller pc;
    private Player_Movment pm;

    private Minion_Controller[] minionController;
    GameObject shortobj = new GameObject();
    GameObject FollowObj;
    private void Awake()
    {
        pc = FindObjectOfType<Player_Controller>();
        pm = FindObjectOfType<Player_Movment>();
    }

    private void Start()
    {
        StartCoroutine(Mode_Follow_co());
    }

    private void Update()
    {
        
            StartCoroutine(Mode_Follow_co());
      
        //if(pm.isPlayerMove == false)
        //{
        //    StopCoroutine(Mode_Follow_co());
        //    StartCoroutine(Mode_Stop_Follow_co());
        //}
           
        
       

    }
    private bool isTarget
    {
        get
        {
            if (!pc.isDead)
            {
                return true;
            }
            return false;
        }

    }


  


    public IEnumerator Mode_Follow_co()
    {
        nearestMinion_List.Clear();

        minionController = GetComponentsInChildren<Minion_Controller>();
        // agents = GetComponentsInChildren<UnityEngine.AI.NavMeshAgent>();


        if (isTarget)
        {
            if(pm.isPlayerMove)
            {
                for (int i = 0; i < pc.Minions_List.Count; i++)
                {
                    pc.Minions_List[i].GetComponent<UnityEngine.AI.NavMeshAgent>().isStopped = false;
                }
            }
           
         

            //수정중..이 망할놈들이 추가될 때도 작동하도록 변경..

            //agents[0].SetDestination(transform.position + Vector3.back);



            for (int i = 0; i < pc.Minions_List.Count; i++)
            {
                if (i == 0)
                {
                    FollowObj = pc.gameObject;
                }
                float ShortDis = Vector3.Distance(FollowObj.transform.position, pc.Minions_List[i].transform.position);
                for (int j = 0; j < pc.Minions_List.Count; j++)
                {
                    float Distance = Vector3.Distance(FollowObj.transform.position, pc.Minions_List[j].transform.position);

                    if (ShortDis >= Distance)
                    {
                        // nearestMinion_List.Add(pc.Minions_List[j]);


                        ShortDis = Distance;
                        shortobj = pc.Minions_List[j];
                    }
                    else
                    {
                        shortobj = pc.Minions_List[i];
                    }


                }
                nearestMinion_List.Add(shortobj);
                if (i == 0)
                {
                    nearestMinion_List[i].GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(pc.transform.localPosition);

                 

                }
                else
                {
                    nearestMinion_List[i].GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(nearestMinion_List[i - 1].transform.localPosition + Vector3.back);
                    //앞 캐릭터랑 근접하면 멈추기
                    //if (Vector3.Distance(nearestMinion_List[i].transform.position, nearestMinion_List[i-1].transform.position) <= 1f)
                    //{
                    //    nearestMinion_List[i].GetComponent<Minion_Controller>().isClose = true;
                    //}
                    //else
                    //{
                    //    nearestMinion_List[i].GetComponent<Minion_Controller>().isClose = false;
                    //}
                }

                FollowObj = nearestMinion_List[i];

            }


        }



        yield return null;



    }






    public IEnumerator Mode_Stop_Follow_co()
    {
        minionController = GetComponentsInChildren<Minion_Controller>();
        for (int i = 0; i<nearestMinion_List.Count; i++)
        {
            if(i%2 == 0)
            {
                nearestMinion_List[i].GetComponent<NavMeshAgent>().SetDestination(pm.CurrentPos + Vector3.left); 
             
            }
            else
            {
                nearestMinion_List[i].GetComponent<NavMeshAgent>().SetDestination(pm.CurrentPos + Vector3.right);
                
           
            }

           
        }
        yield return null;
  
       
    }


    public void Swap(ref float a ,ref float b)
    {
        float temp = a;
        a = b;
        b = temp;

    }
}
