using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollowing : MonoBehaviour
{

    [SerializeField]
    public List<GameObject> nearestMinion_List = new List<GameObject>();
    public LayerMask TargetLayer;

    // private UnityEngine.AI.NavMeshAgent[] agents;



    private LeaderState leaderState;
    private Minion_Controller[] minionController;
    GameObject shortobj;
    GameObject FollowObj;

    List<Vector3> listVetor = new List<Vector3>();

    public bool isa = false;

    public Vector3 StopPos;

    private void Awake()
    {
        TryGetComponent(out leaderState);
    }

    private void Start()
    {
        //StartCoroutine(Mode_Follow_co());
    }

    private void Update()
    {


        if (leaderState.bat_State == LeaderState.BattleState.Follow)
        {
            StopCoroutine(Mode_Stop_co());
            StartCoroutine(Mode_Follow_co());
        }
        else
        {
            StopAllCoroutines();
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


        for (int i = 0; i < leaderState.UnitList.Count; i++)
        {
            leaderState.UnitList[i].GetComponent<Minion_Controller>().isClose = false;
            leaderState.UnitList[i].GetComponent<NavMeshAgent>().isStopped = false;
        }



        if (!leaderState.isDead)
        {


            for (int i = 0; i < leaderState.UnitList.Count; i++)
            {
                listVetor.Add(leaderState.UnitList[i].transform.position);

            }

            leaderState.UnitList.Sort(delegate (GameObject a, GameObject b)
            {
                return Compare2(a, b, leaderState.gameObject);
            });


            for (int i = 0; i < leaderState.UnitList.Count; i++)
            {
                if (i == 0)
                {
                    leaderState.UnitList[i].GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(leaderState.transform.position + Vector3.back);
                }
                else
                {

                    leaderState.UnitList[i].GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(leaderState.UnitList[i - 1].transform.position + Vector3.back);
                }

            }



            for (int i = 0; i < leaderState.UnitList.Count; i++)
            {

                leaderState.UnitList[i].GetComponent<Minion_Controller>().isClose = false;

            }
        }









        #endregion

        if (!leaderState.isMoving)
        {
            #region 멈춤

            for (int i = 0; i < leaderState.UnitList.Count; i++)
            {
                if (i <= 4)
                {
                    if (i % 2 == 0)
                    {
                        if (i == 0)
                        {
                            leaderState.UnitList[i].GetComponent<NavMeshAgent>().SetDestination(leaderState.transform.position + Vector3.left);
                        }
                        else
                        {
                            leaderState.UnitList[i].GetComponent<NavMeshAgent>().SetDestination(leaderState.UnitList[i - 2].transform.position + Vector3.left);
                        }
                    }
                    else
                    {
                        leaderState.UnitList[i].GetComponent<NavMeshAgent>().SetDestination(leaderState.transform.position + Vector3.right);

                        if (i == 1)
                        {
                            leaderState.UnitList[i].GetComponent<NavMeshAgent>().SetDestination(leaderState.transform.position + Vector3.right);
                        }
                        else
                        {
                            leaderState.UnitList[i].GetComponent<NavMeshAgent>().SetDestination(leaderState.UnitList[i - 2].transform.position + Vector3.right);

                        }
                    }

                }
                else
                {
                    //nearestMinion 의 index 5번째부터

                    leaderState.UnitList[i].GetComponent<NavMeshAgent>().SetDestination(leaderState.UnitList[i - 5].transform.position + Vector3.back);

                }


                if (leaderState.UnitList[i].GetComponent<NavMeshAgent>().remainingDistance <= 0.3f)
                {
                    leaderState.UnitList[i].GetComponent<Minion_Controller>().isClose = true;
                    leaderState.UnitList[i].GetComponent<NavMeshAgent>().isStopped = true;
                }
            }
            #endregion
        }


        yield return null;

    }






    public IEnumerator Mode_Stop_co()
    {

        for (int i = 0; i < leaderState.UnitList.Count; i++)
        {
            if (i <= 4)
            {
                if (i % 2 == 0)
                {
                    if (i == 0)
                    {
                        leaderState.UnitList[i].GetComponent<NavMeshAgent>().SetDestination(StopPos);
                    }
                    else
                    {
                        leaderState.UnitList[i].GetComponent<NavMeshAgent>().SetDestination(leaderState.UnitList[i - 2].transform.position + Vector3.left);
                    }

                }
                else
                {
                    if (i == 1)
                    {
                        leaderState.UnitList[i].GetComponent<NavMeshAgent>().SetDestination(StopPos + Vector3.right);
                    }
                    else
                    {
                        leaderState.UnitList[i].GetComponent<NavMeshAgent>().SetDestination(leaderState.UnitList[i - 2].transform.position + Vector3.right);
                    }

                }

            }
            else
            {
                //nearestMinion 의 index 5번째부터

                leaderState.UnitList[i].GetComponent<NavMeshAgent>().SetDestination(leaderState.UnitList[i - 5].transform.position + Vector3.back);

            }


            if (nearestMinion_List[i].GetComponent<NavMeshAgent>().remainingDistance <= 0.5f)
            {

                leaderState.UnitList[i].GetComponent<Minion_Controller>().isClose = true;
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