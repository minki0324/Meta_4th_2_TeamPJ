using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Following : MonoBehaviour
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

        nearestMinion_List.Clear();
        //minionController = GetComponentsInChildren<Minion_Controller>();
        // minionController = FindObjectsOfType<Minion_Controller>();
        #region 삽입정렬알고리즘
        if (isTarget)
        {
            if (pm.isPlayerMove)
            {
                for (int i = 0; i < pc.Minions_List.Count; i++)
                {
                    pc.Minions_List[i].GetComponent<UnityEngine.AI.NavMeshAgent>().isStopped = false;
                }
            }

            Vector3 PlayerPos = pm.CurrentPos;
           


            for (int i = 0; i < pc.Minions_List.Count; i++)
            {
                if( i == 0)
                {
                    FollowObj = pc.gameObject;
                }
                else
                {
                    FollowObj = pc.Minions_List[i];
                }

                float shortDis = Vector3.Distance(pc.Minions_List[i].transform.position, FollowObj.transform.position);
                for (int j = 0; j<pc.Minions_List.Count; j++)
                {
                    float newDis = Vector3.Distance(pc.Minions_List[j].transform.position, FollowObj.transform.position);

                    if (shortDis > newDis)
                    {
                        shortobj = pc.Minions_List[j];
                        pc.Minions_List[j] = pc.Minions_List[i];
                        pc.Minions_List[i] = shortobj;


                        shortDis = newDis;
                    }
                    else break;
                }//end of j


            }//end of i



                //for (int a = 0; a < pc.Minions_List.Count; a++)
                //{
                //    FollowObj = pc.Minions_List[a];
                //}




            for (int i = 0; i < pc.Minions_List.Count; i++)
            {
                if (i == 0)
                {
                    pc.Minions_List[i].GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(pm.transform.position + Vector3.back);
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


            #endregion




            #region 따라옴

            //if (isTarget)
            //{
            //    if (pm.isPlayerMove)
            //    {
            //        for (int i = 0; i < pc.Minions_List.Count; i++)
            //        {
            //            pc.Minions_List[i].GetComponent<UnityEngine.AI.NavMeshAgent>().isStopped = false;
            //        }
            //    }

            //    for (int i = 0; i < pc.Minions_List.Count; i++)
            //    {
            //        if (i == 0)
            //        {
            //            FollowObj = pc.gameObject;
            //        }

            //        float ShortDis = Vector3.Distance(FollowObj.transform.position, pc.Minions_List[i].transform.position);
            //        for (int j = 0; j < pc.Minions_List.Count; j++)
            //        {
            //            float Distance = Vector3.Distance(FollowObj.transform.position, pc.Minions_List[j].transform.position);

            //            if (ShortDis >= Distance)
            //            {
            //                // nearestMinion_List.Add(pc.Minions_List[j]);


            //                ShortDis = Distance;
            //                shortobj = pc.Minions_List[j];
            //            }
            //            else
            //            {
            //                shortobj = pc.Minions_List[i];
            //            }
            //        }

            //        nearestMinion_List.Add(shortobj);


            //        FollowObj = nearestMinion_List[i];

            //    }
            //}

            //for (int i = 0; i < nearestMinion_List.Count; i++)
            //{
            //    if (i == 0)
            //    {
            //        nearestMinion_List[i].GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(pc.transform.position + Vector3.back);
            //    }
            //    else
            //    {

            //        nearestMinion_List[i].GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(nearestMinion_List[i - 1].transform.position + Vector3.back);
            //    }

            //}



            //for (int i = 0; i < nearestMinion_List.Count; i++)
            //{

            //    nearestMinion_List[i].GetComponent<Minion_Controller>().isClose = false;

            //}

            #endregion





            #region 일단 한마리만 유동적으로 따라오는 코드


            /*if (isTarget)
            {


                if (pm.isPlayerMove)
                {
                    Vector3 PlayerPos = pm.CurrentPos;
                    FollowObj = pc.gameObject;

                    for (int i = 0; i < pc.Minions_List.Count; i++)
                    {
                        pc.Minions_List[i].GetComponent<UnityEngine.AI.NavMeshAgent>().isStopped = false;
                    }



                    for (int i = 0; i < pc.Minions_List.Count; i++)
                    {
                        float shortDis = Vector3.Distance(pc.Minions_List[i].transform.position, FollowObj.transform.position);
                        for (int j = i; j >= 0; j--)
                        {
                            float newDis = Vector3.Distance(pc.Minions_List[j].transform.position, FollowObj.transform.position);

                            if (shortDis >= newDis)
                            {
                                shortobj = pc.Minions_List[j];
                                shortDis = newDis;
                            }
                            else break;
                        }//end of j

                    }//end of i     - 가장 길이 짧은 하나를 찾는 코드 블록

                    nearestMinion_List.Add(shortobj);

                    pc.Minions_List.Remove(shortobj);


                    for (int a = 0; a < nearestMinion_List.Count; a++)
                    {

                        FollowObj = nearestMinion_List[a];
                    }

                }






                for (int i = 0; i < nearestMinion_List.Count; i++)
                {
                    if (i == 0)
                    {
                        nearestMinion_List[i].GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(pc.transform.position + Vector3.back);
                    }
                    else
                    {

                        nearestMinion_List[i].GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(nearestMinion_List[i - 1].transform.position + Vector3.back);
                    }

                }



                for (int i = 0; i < nearestMinion_List.Count; i++)
                {

                    nearestMinion_List[i].GetComponent<Minion_Controller>().isClose = false;

                }




            }*/








            #endregion




            if (!pm.isPlayerMove)
            {
                #region 멈춤

                for (int i = 0; i < nearestMinion_List.Count; i++)
                {
                    if (i <= 4)
                    {
                        if (i % 2 == 0)
                        {
                            if (i == 0)
                            {
                                nearestMinion_List[i].GetComponent<NavMeshAgent>().SetDestination(pm.CurrentPos + Vector3.left);
                            }
                            else
                            {
                                nearestMinion_List[i].GetComponent<NavMeshAgent>().SetDestination(nearestMinion_List[i - 2].transform.position + Vector3.left);
                            }
                        }
                        else
                        {
                            nearestMinion_List[i].GetComponent<NavMeshAgent>().SetDestination(pm.CurrentPos + Vector3.right);

                            if (i == 1)
                            {
                                nearestMinion_List[i].GetComponent<NavMeshAgent>().SetDestination(pm.CurrentPos + Vector3.right);
                            }
                            else
                            {
                                nearestMinion_List[i].GetComponent<NavMeshAgent>().SetDestination(nearestMinion_List[i - 2].transform.position + Vector3.right);

                            }
                        }

                    }
                    else
                    {
                        //nearestMinion 의 index 5번째부터

                        nearestMinion_List[i].GetComponent<NavMeshAgent>().SetDestination(nearestMinion_List[i - 5].transform.position + Vector3.back);
                        //if (Vector3.Distance(nearestMinion_List[i].GetComponent<Minion_Controller>().transform.position, nearestMinion_List[i - 5].GetComponent<Minion_Controller>().transform.position) <= 2f)
                        //{
                        //    nearestMinion_List[i].GetComponent<Minion_Controller>().isClose = true;
                        //}

                    }


                    if (nearestMinion_List[i].GetComponent<NavMeshAgent>().remainingDistance <= 0.3f)
                    {
                        nearestMinion_List[i].GetComponent<Minion_Controller>().isClose = true;
                        nearestMinion_List[i].GetComponent<NavMeshAgent>().isStopped = true;
                    }
                }
                #endregion
            }


            yield return new WaitForSeconds(0.1f);

        }

    }

//works when Player Stop. -> included in Mode_Follow_co()
    public IEnumerator Mode_Stop_Follow_co()
    {


        //for (int i = 0; i < nearestMinion_List.Count; i++)
        //{
        //    minionController[i] = GetComponent<Minion_Controller>();            
        //}
        #region 멈춤

        for (int i = 0; i < nearestMinion_List.Count; i++)
        {
            if (i <= 4)
            {
                if (i % 2 == 0)
                {
                    if (i == 0)
                    {
                        nearestMinion_List[i].GetComponent<NavMeshAgent>().SetDestination(pm.CurrentPos + Vector3.left);
                        //(nearestMinion_List[i].GetComponent<NavMeshAgent>().velocity.sqrMagnitude >= 0.2f * 0.2f && nearestMinion_List[i].GetComponent<NavMeshAgent>().remainingDistance <= 0.3f
                        if (nearestMinion_List[i].GetComponent<NavMeshAgent>().remainingDistance <= 0.3f)
                        {
                            Debug.Log("ㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎ");
                            nearestMinion_List[i].GetComponent<Animator>().SetBool("Move", false);
                        }



                    }
                    else
                    {
                        nearestMinion_List[i].GetComponent<NavMeshAgent>().SetDestination(nearestMinion_List[i - 2].transform.position + Vector3.left);
                        if (Vector3.Distance(nearestMinion_List[i].GetComponent<Minion_Controller>().transform.position, nearestMinion_List[i - 2].GetComponent<Minion_Controller>().transform.position) <= 1f)
                        {
                            nearestMinion_List[i].GetComponent<Minion_Controller>().isClose = true;

                        }

                    }
                }
                else
                {
                    nearestMinion_List[i].GetComponent<NavMeshAgent>().SetDestination(pm.CurrentPos + Vector3.right);

                    if (i == 1)
                    {
                        nearestMinion_List[i].GetComponent<NavMeshAgent>().SetDestination(pm.CurrentPos + Vector3.right);
                        if (Vector3.Distance(nearestMinion_List[i].GetComponent<Minion_Controller>().transform.position, pm.CurrentPos) <= 1f)
                        {

                            nearestMinion_List[i].GetComponent<Minion_Controller>().isClose = true;
                        }


                    }
                    else
                    {
                        nearestMinion_List[i].GetComponent<NavMeshAgent>().SetDestination(nearestMinion_List[i - 2].transform.position + Vector3.right);
                        if (Vector3.Distance(nearestMinion_List[i].GetComponent<Minion_Controller>().transform.position, nearestMinion_List[i - 2].GetComponent<Minion_Controller>().transform.position) <= 1f)
                        {
                            nearestMinion_List[i].GetComponent<Minion_Controller>().isClose = true;
                        }

                    }
                }

            }
            else
            {
                //nearestMinion 의 index 5번째부터

                nearestMinion_List[i].GetComponent<NavMeshAgent>().SetDestination(nearestMinion_List[i - 5].transform.position + Vector3.back);
                if (Vector3.Distance(nearestMinion_List[i].GetComponent<Minion_Controller>().transform.position, nearestMinion_List[i - 5].GetComponent<Minion_Controller>().transform.position) <= 2f)
                {
                    nearestMinion_List[i].GetComponent<Minion_Controller>().isClose = true;
                }

            }


            // nearestMinion_List[i].GetComponent<NavMeshAgent>().isStopped = true;
        }
        #endregion 
        yield return null;
    }



    public IEnumerator Mode_Stop_co()
    {

        for (int i = 0; i < nearestMinion_List.Count; i++)
        {
            if (i <= 4)
            {
                if (i % 2 == 0)
                {
                    if (i == 0)
                    {
                        nearestMinion_List[i].GetComponent<NavMeshAgent>().SetDestination(StopPos);
                    }
                    else
                    {
                        nearestMinion_List[i].GetComponent<NavMeshAgent>().SetDestination(nearestMinion_List[i - 2].transform.position + Vector3.left);
                    }

                }
                else
                {
                    if (i == 1)
                    {
                        nearestMinion_List[i].GetComponent<NavMeshAgent>().SetDestination(StopPos + Vector3.right);
                    }
                    else
                    {
                        nearestMinion_List[i].GetComponent<NavMeshAgent>().SetDestination(nearestMinion_List[i - 2].transform.position + Vector3.right);
                    }

                }

            }
            else
            {
                //nearestMinion 의 index 5번째부터

                nearestMinion_List[i].GetComponent<NavMeshAgent>().SetDestination(nearestMinion_List[i - 5].transform.position + Vector3.back);

            }


            if (nearestMinion_List[i].GetComponent<NavMeshAgent>().remainingDistance <= 0.5f)
            {

                nearestMinion_List[i].GetComponent<Minion_Controller>().isClose = true;
                Debug.Log(nearestMinion_List[i].GetComponent<Minion_Controller>().isClose);
                //nearestMinion_List[i].GetComponent<NavMeshAgent>().isStopped = true;
            }

        }



        yield return null;
    }






    public void Swap(ref float a, ref float b)
    {
        float temp = a;
        a = b;
        b = temp;

    }

}