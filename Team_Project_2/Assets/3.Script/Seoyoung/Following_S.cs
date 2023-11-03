/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Following_S : MonoBehaviour
{

    [SerializeField]
    public List<GameObject> nearestMinion_List = new List<GameObject>();
    public LayerMask TargetLayer;

    // private UnityEngine.AI.NavMeshAgent[] agents;

    private Ply_Controller pc;
    private Ply_Movement pm;

    private Minion_Controller[] minionController;
    GameObject shortobj;
    GameObject FollowObj;

    List<Vector3> listVetor = new List<Vector3>();

    public bool isa = false;

    public Vector3 StopPos;

    private void Awake()
    {
        pc = FindObjectOfType<Ply_Controller>();
        pm = FindObjectOfType<Ply_Movement>();
    }

    private void Start()
    {
        //StartCoroutine(Mode_Follow_co());
    }

    private void Update()
    {


        if (pc.CurrentMode == Ply_Controller.Mode.Follow)
        {
            StopCoroutine(Mode_Stop_co());
            StartCoroutine(Mode_Follow_co());
        }
        if (pc.CurrentMode == Ply_Controller.Mode.Stop)
        {
            if (pc.ischeckPosition)
            {
                StopPos = pc.transform.position;
                pc.ischeckPosition = false;
            }
            StopCoroutine(Mode_Follow_co());
            StartCoroutine(Mode_Stop_co());

        }






    }
    private bool isTarget
    {

        get
        {
            if (GameManager.instance.isLive)
            {
                return true;
            }
            return false;
        }

    }




    // 플레이어 움직일 때 : 한줄로 줄세우는 코루틴
    public IEnumerator Mode_Follow_co()
    {



        #region 자연스러운 이동


        for (int i = 0; i < pc.Minions_List.Count; i++)
        {
            pc.Minions_List[i].GetComponent<Minion_Controller>().isClose = false;
            pc.Minions_List[i].GetComponent<NavMeshAgent>().isStopped = false;
        }



        if (isTarget)
        {


            for (int i = 0; i < pc.Minions_List.Count; i++)
            {
                listVetor.Add(pc.Minions_List[i].transform.position);

            }

            pc.Minions_List.Sort(delegate (GameObject a, GameObject b)
            {
                return Compare2(a, b, pc.gameObject);
            });


            for (int i = 0; i < pc.Minions_List.Count; i++)
            {
                if (i == 0)
                {
                    pc.Minions_List[i].GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(pc.transform.position + Vector3.back);
                }
                else
                {

                    pc.Minions_List[i].GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(pc.Minions_List[i - 1].transform.position + Vector3.back);
                }

            }



            for (int i = 0; i < pc.Minions_List.Count; i++)
            {

                pc.Minions_List[i].GetComponent<Minion_Controller>().isClose = false;

            }
        }









        #endregion

        if (!pm.isPlayerMove)
        {
            #region 멈춤

            for (int i = 0; i < pc.Minions_List.Count; i++)
            {
                if (i <= 4)
                {
                    if (i % 2 == 0)
                    {
                        if (i == 0)
                        {
                            pc.Minions_List[i].GetComponent<NavMeshAgent>().SetDestination(pm.CurrentPos + Vector3.left);
                        }
                        else
                        {
                            pc.Minions_List[i].GetComponent<NavMeshAgent>().SetDestination(pc.Minions_List[i - 2].transform.position + Vector3.left);
                        }
                    }
                    else
                    {
                        pc.Minions_List[i].GetComponent<NavMeshAgent>().SetDestination(pm.CurrentPos + Vector3.right);

                        if (i == 1)
                        {
                            pc.Minions_List[i].GetComponent<NavMeshAgent>().SetDestination(pm.CurrentPos + Vector3.right);
                        }
                        else
                        {
                            pc.Minions_List[i].GetComponent<NavMeshAgent>().SetDestination(pc.Minions_List[i - 2].transform.position + Vector3.right);

                        }
                    }

                }
                else
                {
                    //nearestMinion 의 index 5번째부터

                    pc.Minions_List[i].GetComponent<NavMeshAgent>().SetDestination(pc.Minions_List[i - 5].transform.position + Vector3.back);

                }


                if (pc.Minions_List[i].GetComponent<NavMeshAgent>().remainingDistance <= 0.3f)
                {
                    pc.Minions_List[i].GetComponent<Minion_Controller>().isClose = true;
                    pc.Minions_List[i].GetComponent<NavMeshAgent>().isStopped = true;
                }
            }
            #endregion
        }


        yield return null;

    }






    public IEnumerator Mode_Stop_co()
    {

        for (int i = 0; i < pc.Minions_List.Count; i++)
        {
            if (i <= 4)
            {
                if (i % 2 == 0)
                {
                    if (i == 0)
                    {
                        pc.Minions_List[i].GetComponent<NavMeshAgent>().SetDestination(StopPos);
                    }
                    else
                    {
                        pc.Minions_List[i].GetComponent<NavMeshAgent>().SetDestination(pc.Minions_List[i - 2].transform.position + Vector3.left);
                    }

                }
                else
                {
                    if (i == 1)
                    {
                        pc.Minions_List[i].GetComponent<NavMeshAgent>().SetDestination(StopPos + Vector3.right);
                    }
                    else
                    {
                        pc.Minions_List[i].GetComponent<NavMeshAgent>().SetDestination(pc.Minions_List[i - 2].transform.position + Vector3.right);
                    }

                }

            }
            else
            {
                //nearestMinion 의 index 5번째부터

                pc.Minions_List[i].GetComponent<NavMeshAgent>().SetDestination(pc.Minions_List[i - 5].transform.position + Vector3.back);

            }


            if (nearestMinion_List[i].GetComponent<NavMeshAgent>().remainingDistance <= 0.5f)
            {

                pc.Minions_List[i].GetComponent<Minion_Controller>().isClose = true;
                Debug.Log(nearestMinion_List[i].GetComponent<Minion_Controller>().isClose);
                //nearestMinion_List[i].GetComponent<NavMeshAgent>().isStopped = true;
            }

        }



        yield return null;
    }





    public int Compare(Vector3 a, Vector3 b, Vector3 dest)
    {
        float lengthA_Dest = Vector3.Distance(a, dest);
        float lengthB_Dest = Vector3.Distance(a, dest);

        return lengthA_Dest < lengthB_Dest ? -1 : 1;

    }

    public int Compare2(GameObject a, GameObject b, GameObject dest)
    {
        float lengthA_Dest = Vector3.Distance(a.transform.position, dest.transform.position);
        float lengthB_Dest = Vector3.Distance(b.transform.position, dest.transform.position);

        return lengthA_Dest < lengthB_Dest ? -1 : 1;
    }

    void printList<T>(List<T> list)
    {
        string text = string.Empty;
        foreach (T l in list) text += l.ToString() + ", ";
        Debug.Log(text);
    }
}
*/