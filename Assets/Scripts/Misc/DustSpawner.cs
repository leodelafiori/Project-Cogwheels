using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// Script to add dust to characters feet when they land


public class DustSpawner : MonoBehaviour {

    #region Declaring variables
    [SerializeField] private GameObject[] particles; //Place the dust to spawn
    private Vector3 groundCheck;
    private Vector3 groundPositionSpawn;
    [SerializeField] private float checkRadius = 0.1f;
    [SerializeField] private float groundOffset = 0.45f;
    [SerializeField] private LayerMask ground;
    private bool isGrounded;
    private bool hitGround;
    private int dustCounter;
    #endregion

    #region fixedupdate method, for checking if it hit ground
    private void FixedUpdate()
    {
        groundCheck = new Vector3(transform.position.x, transform.position.y - groundOffset, 0);
        isGrounded = Physics2D.OverlapCircle(groundCheck, checkRadius, ground);

        if (isGrounded)
        {
            if (hitGround)
            {
                for(dustCounter = 0; dustCounter < particles.Length; dustCounter++)
                {
                    groundPositionSpawn = new Vector3(Random.Range(groundCheck.x - 0.1f, groundCheck.x + 0.1f), groundCheck.y, 0);
                    Instantiate(particles[dustCounter], groundPositionSpawn, Quaternion.identity);
                }
                hitGround = false;
            }
        } else
        {
            hitGround = true;
        }
    }
    #endregion


}
