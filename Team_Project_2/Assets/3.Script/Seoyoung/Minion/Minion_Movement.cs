using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion_Movement : MonoBehaviour
{
    private Player_Movement playerMovement;

    private Rigidbody rigidBody;

    Vector3 MinionRotate = new Vector3();

    private void Awake()
    {
        playerMovement = FindObjectOfType<Player_Movement>();
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
    }

    private void Update()
    {
        MinionRotate = playerMovement.playerRotate;
    }
}
