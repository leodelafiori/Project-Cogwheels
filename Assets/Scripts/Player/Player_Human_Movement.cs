using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Human_Movement : MonoBehaviour {

    #region Declaring variables //////////////////////////////////////////////////////////////////////////////////////
    //General Variables to be used on prefab
    private Animator animator;
    private Rigidbody2D rigidBody;
    public static bool isMovable=true;
//Variables for flipping
    private float moveInput;
    private bool facingRight = true;
//Variables for jumping
    private bool hitGround = false;
    private bool isGrounded;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float checkRadius = 0.2f;
    [SerializeField] private LayerMask ground;
    [SerializeField] private int extraJumps = 2;
//Variables for movement speed/force
    [SerializeField] private float walkingSpeed = 1.0f;
    [SerializeField] private float runningSpeed = 1.5f;
    [SerializeField] private float jumpForce = 300;
//Variable for checking when running
    private bool isRunning = false;
//Camera Shake landing
    private GameObject cameraScript;
    private Animator cameraShake;
//Rolling variables
    private int rollCountLeft = 0;
    private int rollCountRight = 0;
    private float rollCooler = 0.5f;
    [SerializeField] private float rollSpeed;
    public static bool isRolling = false;
    [SerializeField] private float rollTimerReset = .2f;
    private float rollTimer;
  //Blocking
    public static bool isBlocking = false;


    #endregion
    #region Void awake ////////////////////////////////////////////////////////////////////////////////////// 
    private void Awake()
    {
  //Getting the components for animations on the character
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        //Getting the animator for the camera shake
        cameraScript = GameObject.FindGameObjectWithTag("MainCamera");
        cameraShake = cameraScript.GetComponent<Animator>();
    }
    #endregion
    #region Fixed Update Method //////////////////////////////////////////////////////////////////////////////////////
    // Void Fixed update (used to all physics related things on the game
    private void FixedUpdate()
    {
            moveInput = Input.GetAxis("Horizontal");
            animator.SetFloat("InputSpeed", Mathf.Abs(moveInput));

        #region Animations/Running/Walking) //////////////////////////////////////////////////////////////////////////////////////
        if (moveInput != 0 && Input.GetKey(KeyCode.LeftShift) && isMovable && !isRolling && !isBlocking && !Player_Human_Combat.isAttacking)
        {
            if (isGrounded && !isRunning)
            {
                isRunning = true;
            }
            transform.Translate(moveInput * runningSpeed * Time.deltaTime, 0, 0);
            //rigidBody.velocity = new Vector2(moveInput * runningSpeed, rigidBody.velocity.y);
            animator.SetBool("IsRunning", true);
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsIdle", false);
        }
        else if (moveInput != 0 && isMovable && !isRolling && !isBlocking && !Player_Human_Combat.isAttacking)
        {
            transform.Translate(moveInput * walkingSpeed * Time.deltaTime, 0, 0);
            //rigidBody.velocity = new Vector2(moveInput * walkingSpeed, rigidBody.velocity.y);
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsIdle", false);
            isRunning = false;
        }

        if (moveInput == 0 || !isMovable)
        {
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsIdle", true);
            isRunning = false;
        }
        #endregion
        #region Jumping (isGrounded) //////////////////////////////////////////////////////////////////////////////////////
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, ground);
        #endregion
    }
    #endregion   
    #region Update Method //////////////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        #region Double Jump and Jump
        if (Input.GetKey(KeyCode.Space) && isGrounded && !isRolling && !Player_Human_Combat.isAttacking)
        {
            animator.SetBool("Jumped", true);
        } else
        {
            animator.SetBool("Jumped", false);
        }
        //Landing and spawning dust
        if (isGrounded)
        {
            extraJumps = 1;
            if (hitGround){
                animator.SetTrigger("Landed");
                hitGround = false;
            }
        } else {
            hitGround = true;
        }
        if (Input.GetKeyDown(KeyCode.Space) && extraJumps > 0 && !isRolling && !Player_Human_Combat.isAttacking)  /// Jumping
        {
            rigidBody.velocity = Vector2.up * jumpForce;
            extraJumps--;
        }

        #endregion
        #region Jumping animations
        if (rigidBody.velocity.y > 0) {
            animator.SetBool("Ascending", true);
            animator.SetBool("Descending", false);
        } else if (rigidBody.velocity.y < 0)
        {
            animator.SetBool("Ascending", false);
            animator.SetBool("Descending", true);
        } else
        {
            animator.SetBool("Descending", false);
            animator.SetBool("Ascending", false);
        }
        #endregion
        #region Flipping character model //////////////////////////////////////////////////////////////////////////////////////
        if (facingRight == false && moveInput > 0 && isMovable)
        {
            FlipCharacter();
        }
        else if (facingRight == true && moveInput < 0 && isMovable)
        {
            FlipCharacter();
        }

        #endregion
        #region Rolling if/else statements //////////////////////////////////////////////////////////////////////////////////////
        //First if's if to check when the player presses the button
        if (Input.GetKeyDown(KeyCode.D) && !isRolling)
        {
            if (rollCooler > 0 && rollCountRight == 1 && isGrounded && isMovable)
            {
                animator.SetTrigger("Rolled");
                StartCoroutine("RollRight");
            }
            else
            {
                rollCooler = 0.5f;
                rollCountRight += 1;
            }

        }

        if (Input.GetKeyDown(KeyCode.A) && !isRolling)
        {
            if (rollCooler > 0 && rollCountLeft == 1 && isGrounded && isMovable)
            {
                animator.SetTrigger("Rolled");
                StartCoroutine("RollLeft");
            }
            else
            {
                rollCooler = 0.5f;
                rollCountLeft += 1;
            }

        }
        //Second if to count the dashcooler
        if (rollCooler > 0)
        {
            //Solution to not use co routines
            rollCooler -= 1 * Time.deltaTime;
        }
        else
        {
            rollCountLeft = 0;
            rollCountRight = 0;
        }
        #endregion
        #region Blocking
        if (Input.GetKey(KeyCode.LeftControl) && isGrounded)
        {
            isBlocking = true;
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsIdle", false);
            animator.SetBool("IsBlocking", true);
        }
        else
        {
            animator.SetBool("IsBlocking", false);
            isBlocking = false;
        }
        #endregion
    }

    #endregion
    #region Flipping character method //////////////////////////////////////////////////////////////////////////////////////
    private void FlipCharacter()
    {
        facingRight = !facingRight;
        Vector3 Scale = transform.localScale;
        Scale.x *= -1;
        transform.localScale = Scale;
    }
    #endregion
    #region Roll Coroutines //////////////////////////////////////////////////////////////////////////////////////
    IEnumerator RollRight()
    {
        isRolling = true;
        rollTimer = rollTimerReset; //Reseting roll timer
        while (rollTimer > 0)
        {
            transform.Translate(rollSpeed, 0, 0);
            yield return null; //this line needs to return null, it's a coroutine.
            rollTimer -= Time.deltaTime;
        }

        isRolling = false;
       

    }
    IEnumerator RollLeft()
    {
        isRolling = true;
        rollTimer = rollTimerReset; //Reseting roll timer
        while (rollTimer > 0)
        {
            transform.Translate(-rollSpeed, 0, 0);
            yield return null; //this line needs to return null, it's a coroutine.
            rollTimer -= Time.deltaTime;
        }

        isRolling = false;
    }
    #endregion

}
