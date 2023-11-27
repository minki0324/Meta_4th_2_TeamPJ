using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderState : MonoBehaviour
{
    [SerializeField] private GameObject[] OrderIcon;
    private Ply_Controller ply;

    private void Start()
    {
        ply = GetComponent<Ply_Controller>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            for (int i = 0; i < OrderIcon.Length; i++)
            {
                OrderIcon[i].SetActive(false);
                OrderIcon[0].SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            for (int i = 0; i < OrderIcon.Length; i++)
            {
                OrderIcon[i].SetActive(false);
                OrderIcon[1].SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            for (int i = 0; i < OrderIcon.Length; i++)
            {
                OrderIcon[i].SetActive(false);
                OrderIcon[2].SetActive(true);
            }
        }
    }
}
