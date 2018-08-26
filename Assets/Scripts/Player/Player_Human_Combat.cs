using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Human_Combat : MonoBehaviour {

    #region Declaring variables
 //Main variables
    private float timeBtwAttacks = 1;
    private float resetTimeBtwAttacks = 1;
    [SerializeField] private LayerMask whatIsEnemies;
    [SerializeField] private Transform attackPos;
    [SerializeField]private float attackRange = 0.5f;
    [SerializeField] private int damage = 20;
    private Animator anim;
    public static bool isAttacking = false;

    private float timerAttackAnimation = 1;
    [SerializeField] private float maxTimerAttackAnimation = 1;
    #endregion
    #region Awake
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    #endregion

    #region Update Function
    void Update () {
        if (timeBtwAttacks <= 0 && Input.GetMouseButton(0) && Player_Human_Movement.isMovable)
        {
            isAttacking = true;
            Collider2D[] enemyToDamage = Physics2D.OverlapCircleAll(attackPos.position,attackRange,whatIsEnemies);
            anim.SetTrigger("LightAttack1");
            for(int i = 0; i < enemyToDamage.Length; i++)
            {
                enemyToDamage[i].GetComponent<Enemy1>().TakeDamage(damage);
            }
            timeBtwAttacks = resetTimeBtwAttacks;
        } else
        {
            timeBtwAttacks -= Time.deltaTime;
        }
        if (isAttacking)
        {

            timerAttackAnimation -= Time.deltaTime;

            //test -= Time.deltaTime;
            if (timerAttackAnimation <= 0)
            {
                timerAttackAnimation = maxTimerAttackAnimation;
                isAttacking = false;
            }
        }
	}
    #endregion

    #region onDrawGizmosSelected (visualize the player attack range
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
    #endregion

}
