using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour {

    /// Meele enemy AI 1
    /// This AI is for when the player gets close to an attacker
    /// this AI will be complementary to the pathfinding AI, depending on the enemy
    /// made by: Leonardo Delafiori

    #region Declaring variables
    //Main variables
    private Pathfinding pathfinding;
    private GameObject player;
    private Transform playerTransform;
    [SerializeField] private float speed = 1;
    private Animator anim;
    //Variables to test direction of the enemy
    private float playerX;
    private float enemyX;
    private float oldEnemyX;
    private bool facingRight = true;
    //Variables for aggro distance
    [SerializeField] private float aggroDistance = 5;
    [SerializeField] private float attackDistance = 1;
    //Variables for enemy health
    private float health = 50;
    [SerializeField] private float maxHealth = 50;
    [SerializeField] private Transform enemyHealth;
    private bool isDead = false;
    //Variable for timing the animations
    private bool isAttacking = false;
    [SerializeField] private float damageTimer;
    [SerializeField] private float idleTimer;
    //Variables for pathfinding extras (limits off the map
    private bool outOfBounds = false;
    
    #endregion
    #region Awake (to set animator/attacking collider)
    private void Awake()
    {
        health = maxHealth;
        pathfinding = GetComponent<Pathfinding>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.GetComponent<Transform>();
        anim = GetComponent<Animator>();
        //Variables to change direction depending on the movement of the enemy
        enemyX = transform.position.x;
        oldEnemyX = enemyX;
    }
    #endregion
    #region Update Function
    void Update()
    {
        #region Flipping enemy
            playerX = player.transform.position.x;
            enemyX = transform.position.x;
        //If he is pathfinding, his X position checking is going to be different from when he is attacking
        if (pathfinding.isPathfinding)
        {
            if (enemyX > oldEnemyX && !facingRight && !isDead)
            {
                Flip();
            }
            if (enemyX < oldEnemyX && facingRight && !isDead)
            {
                Flip();
            }
            oldEnemyX = enemyX;
        } else
        {
            if (enemyX < playerX && !facingRight && !isDead)
            {
                Flip();
            }
            if (enemyX > playerX && facingRight && !isDead)
            {
                Flip();
            }
        }
            

        #endregion
        #region Killing enemy when health reaches zero
        if (health <= 0)
        {
            isDead = true;
            anim.SetTrigger("Dead");
        }
        #endregion
        #region Enemy Health Bar
        if (health < maxHealth)
        {
            UpdateEnemyHealthBar();
        }
        #endregion
        #region Checking if the enemy is out of bounds
        if(transform.position.x >= pathfinding.PointB.position.x || transform.position.x <= pathfinding.PointA.position.x)
        {
            outOfBounds = true;
        } else
        {
            outOfBounds = false;
        }
        #endregion

    }
    #endregion
    #region fixedupdate
    private void FixedUpdate()
    {
        #region Finding/attacking player
        //Check player position
        if (Vector2.Distance(playerTransform.position, transform.position) < aggroDistance && Vector2.Distance(playerTransform.position, transform.position) > attackDistance && !isDead && !outOfBounds)
        {
            pathfinding.isPathfinding = false;
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
            anim.SetBool("IsRunning", true);
            anim.SetBool("IsIdle", false);
        }
        else if (Vector2.Distance(playerTransform.position, transform.position) <= attackDistance && !isDead && !outOfBounds)
        {
            pathfinding.isPathfinding = false;
            anim.SetBool("IsIdle", true); //Is iddle=true because after the attack he returns to his running animation
            anim.SetBool("IsRunning", false);
            if (!isAttacking)
            {
                //Activating variables and booleans
                isAttacking = true;
                anim.SetTrigger("Attack");
                //Time to start another animation and reset the isAttacking
                StartCoroutine(AttackingTimer());
                //Time to give another damage to the player in the animation
                StartCoroutine(DamageTimer());
            }

        } else
        {
            pathfinding.isPathfinding = true;
            isAttacking = false;
            anim.SetBool("IsRunning", false);
        }

        #region pathfinding animation space
        if (pathfinding.isPathfinding)
        {
            if (pathfinding.idle)
            {
                anim.SetBool("IsRunning", false);
                anim.SetBool("IsWalking", false);
                anim.SetBool("IsIdle", true);
            }
            if (pathfinding.moving)
            {
                anim.SetBool("IsRunning", false);
                anim.SetBool("IsWalking", true);
                anim.SetBool("IsIdle", false);
            }
        }
        #endregion
        #endregion
    }
    #endregion
    #region Flipping Methods
    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scale = transform.localScale;
        Scale.x *= -1;
        transform.localScale = Scale;
    }
    #endregion
    #region Taking Damage from the player
    public void TakeDamage(float damage)
    {
        health -= damage;
    }
    #endregion
    #region EnemyHealthBar function
    private void UpdateEnemyHealthBar()
    {
        float healthRatio = health / maxHealth;
        if(healthRatio >= 0)
        {
            enemyHealth.transform.localScale = new Vector3(healthRatio, 1, 1);
        } else
        {
            enemyHealth.transform.localScale = new Vector3(0, 1, 1);
        }

    }
    #endregion
    #region AttackingTimer/DamageTimer/IdleTimer Coroutine
    //This region is for timing new animations and damage for every animatino at the right time
    IEnumerator AttackingTimer()
    {
        //Waiting for the animation to end to restart the bool
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        //Idling the animations
        StartCoroutine(IdleTimer());
    }
    IEnumerator IdleTimer()
    {
        yield return new WaitForSeconds(idleTimer);
        isAttacking = false;
    }
    IEnumerator DamageTimer()
    {
        yield return new WaitForSeconds(damageTimer);
        GameManager.instance.PlayerHit(5);
    }
    #endregion

}
