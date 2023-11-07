using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Following : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> Stop_List = new List<GameObject>();

    [SerializeField]
    public List<GameObject> Stop_A_List = new List<GameObject>();

    public LayerMask TargetLayer;

    // private UnityEngine.AI.NavMeshAgent[] agents;

    private Ply_Controller pc;
    private Ply_Movement pm;


    GameObject shortobj;
    GameObject FollowObj;

    List<Vector3> listVetor = new List<Vector3>();

    public bool isa = false;

    public Vector3 StopPos;



    public bool isStop = false;
    public Vector3 Standard;

    [SerializeField]
    private GameObject Foward;

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
        if(Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log(pc.transform.position + (Vector3.forward*3));
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
        if (pm.isPlayerMove)
        {
            #region 자연스러운 이동
            isStop = true;
            if (isTarget)
            {


                for (int i = 0; i < pc.UnitList_List.Count; i++)
                {
                    pc.UnitList_List[i].GetComponent<Minion_Controller>().isClose = false;
                    pc.UnitList_List[i].GetComponent<NavMeshAgent>().isStopped = false;
                }


                pc.UnitList_List.Sort(delegate (GameObject a, GameObject b)
                {
                    return Compare2(a, b, pc.gameObject);
                });


                for (int i = 0; i < pc.UnitList_List.Count; i++)
                {
                    if (i == 0)
                    {
                        pc.UnitList_List[i].GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(pc.transform.position + Vector3.back);
                    }
                    else
                    {

                        pc.UnitList_List[i].GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(pc.transform.position + Vector3.back);
                    }

                }



                for (int i = 0; i < pc.UnitList_List.Count; i++)
                {

                    pc.UnitList_List[i].GetComponent<Minion_Controller>().isClose = false;

                }
            }
            #endregion
        }


        else
        {
            
            if(isStop == true)
            {
                Stop_List.Clear();

                Stop_List.Add(pc.gameObject);
                for (int i = 0; i < pc.UnitList_List.Count; i++)
                {
                    Stop_List.Add(pc.UnitList_List[i]);
                }

                //리스트 정렬
                Stop_List.Sort(delegate (GameObject a, GameObject b)
                {
                    return Compare2(a, b, Foward);
                });
                isStop = false;
            }



            //위치 배치
            for (int i = 0; i < Stop_List.Count; i++)
            {
                if (Stop_List[0] == pc.gameObject)
                {
                    Debug.Log("플레이어가 StopList 0번");

                    if (i <= 4)
                    {
                        if (i % 2 == 0)
                        {
                            if(i != 0)
                            {
                                pc.UnitList_List[i].GetComponent<NavMeshAgent>().SetDestination(pc.UnitList_List[i - 2].transform.position + Vector3.left);
                            }
                        }
                        else
                        {
                            pc.UnitList_List[i].GetComponent<NavMeshAgent>().SetDestination(pm.CurrentPos + Vector3.right);

                            if (i == 1)
                            {
                                pc.UnitList_List[i].GetComponent<NavMeshAgent>().SetDestination(pm.CurrentPos + Vector3.right);
                            }
                            else
                            {
                                pc.UnitList_List[i].GetComponent<NavMeshAgent>().SetDestination(pc.UnitList_List[i - 2].transform.position + Vector3.right);

                            }
                        }

                    }
                    else
                    {
                        //nearestMinion 의 index 5번째부터

                        pc.UnitList_List[i].GetComponent<NavMeshAgent>().SetDestination(pc.UnitList_List[i - 5].transform.position + Vector3.back);

                    }

                }
                else
                {
                    Debug.Log("플레이어는 0번이 아니야.." + i + "번이야..");

                    Stop_List.Clear();
                    Stop_A_List.Add(pc.gameObject);
                    for (int j = 0; j < Stop_List.Count; j++)
                    {
                        Stop_List.Add(pc.UnitList_List[j]);
                    }

                    Stop_A_List.Sort(delegate (GameObject a, GameObject b)
                    {
                        return Compare2(a, b, pc.gameObject);
                    });



                    if (i % 4 == 1)
                    {
                        if (i == 1)
                        {
                            Stop_A_List[i].GetComponent<NavMeshAgent>().SetDestination(pc.gameObject.transform.position + Vector3.forward);
                        }
                        else
                        {
                            Stop_A_List[i].GetComponent<NavMeshAgent>().SetDestination(Stop_A_List[i - 4].transform.position + Vector3.left);
                        }
                    }
                    if (i % 4 == 2)
                    {
                        if (i == 2)
                        {
                            Stop_A_List[i].GetComponent<NavMeshAgent>().SetDestination(pc.gameObject.transform.position + Vector3.right);
                        }
                        else
                        {
                            Stop_A_List[i].GetComponent<NavMeshAgent>().SetDestination(Stop_A_List[i - 4].transform.position + Vector3.left);
                        }
                    }

                    if (i % 4 == 3)
                    {
                        if (i == 3)
                        {
                            Stop_A_List[i].GetComponent<NavMeshAgent>().SetDestination(pc.gameObject.transform.position + Vector3.back);
                        }
                        else
                        {
                            Stop_A_List[i].GetComponent<NavMeshAgent>().SetDestination(Stop_A_List[i - 4].transform.position + Vector3.left);
                        }
                    }
                    if (i % 4 == 0)
                    {
                        if (i != 0)
                        {
                            Stop_A_List[i].GetComponent<NavMeshAgent>().SetDestination(Stop_A_List[i - 4].transform.position + Vector3.left);
                        }
                    }
                }







                #region ㅎㅎㅎㅎ
                //if (Stop_List[0] == pc.gameObject)
                //{
                //    Standard = pc.transform.position;

                //    if (i <= 4)
                //    {

                //        if (i % 2 == 0)
                //        {
                //            if (i != 0)
                //            {
                //                Stop_List[i].GetComponent<NavMeshAgent>().SetDestination(Standard + Vector3.left);
                //            }

                //        }
                //        else
                //        {
                //            Stop_List[i].GetComponent<NavMeshAgent>().SetDestination(Standard + Vector3.right);
                //        }

                //    }
                //    else
                //    {
                //        Stop_List[i].GetComponent<NavMeshAgent>().SetDestination(Stop_List[i - 4].transform.position + Vector3.back);
                //    }

                //}
                #endregion


                if (Stop_List[i].GetComponent<NavMeshAgent>().remainingDistance <= 10f)
                {
                    StartCoroutine(Timer());

                }
            }

        }




        yield return null;

    }






    public IEnumerator Mode_Stop_co()
    {

        for (int i = 0; i < pc.UnitList_List.Count; i++)
        {
            if (i <= 4)
            {
                if (i % 2 == 0)
                {
                    if (i == 0)
                    {
                        pc.UnitList_List[i].GetComponent<NavMeshAgent>().SetDestination(StopPos);
                    }
                    else
                    {
                        pc.UnitList_List[i].GetComponent<NavMeshAgent>().SetDestination(pc.UnitList_List[i - 2].transform.position + Vector3.left);
                    }

                }
                else
                {
                    if (i == 1)
                    {
                        pc.UnitList_List[i].GetComponent<NavMeshAgent>().SetDestination(StopPos + Vector3.right);
                    }
                    else
                    {
                        pc.UnitList_List[i].GetComponent<NavMeshAgent>().SetDestination(pc.UnitList_List[i - 2].transform.position + Vector3.right);
                    }

                }

            }
            else
            {
                //nearestMinion 의 index 5번째부터

                pc.UnitList_List[i].GetComponent<NavMeshAgent>().SetDestination(pc.UnitList_List[i - 5].transform.position + Vector3.back);

            }


            if (pc.UnitList_List[i].GetComponent<NavMeshAgent>().remainingDistance <= 0.5f)
            {

                pc.UnitList_List[i].GetComponent<Minion_Controller>().isClose = true;

                pc.UnitList_List[i].GetComponent<NavMeshAgent>().isStopped = true;
            }

        }



        yield return null;
    }

    public IEnumerator Timer()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < Stop_List.Count; i++)
        { 
            Stop_List[i].GetComponent<Minion_Controller>().isClose = true;
            Stop_List[i].GetComponent<NavMeshAgent>().isStopped = true;
            Stop_A_List[i].GetComponent<Minion_Controller>().isClose = true;
            Stop_A_List[i].GetComponent<NavMeshAgent>().isStopped = true;
        }
       
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

    public int Compare3(GameObject a, GameObject b, GameObject dest)
    {
        float lengthA_Dest = Vector3.Distance(a.transform.position, dest.transform.position + (Vector3.forward * 100));
        float lengthB_Dest = Vector3.Distance(b.transform.position, dest.transform.position + (Vector3.forward * 100));

        return lengthA_Dest < lengthB_Dest ? -1 : 1;
    }


    void printList<T>(List<T> list)
    {
        string text = string.Empty;
        foreach (T l in list) text += l.ToString() + ", ";
        //Debug.Log(text);
    }
}