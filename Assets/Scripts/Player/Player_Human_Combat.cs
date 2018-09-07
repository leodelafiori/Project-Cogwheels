using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Human_Combat : MonoBehaviour {

    #region Declaring variables
    //Variables for the reset time on combos
    [SerializeField] private float timeBtwAttacks;
    [SerializeField] private LayerMask whatIsEnemies;
    [SerializeField] private Transform attackPos;
    [SerializeField]private float attackRange = 0.5f;
    [SerializeField] private int damage = 20;
    private Animator anim;
    public static bool isAttacking = false;
    private int comboIndex;
    private int heavyComboIndex;
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

        //Setting the damage and trigger animations
        if (timeBtwAttacks <= 0.1 && Input.GetMouseButton(1))
        {
            isAttacking = true;
            Player_Human_Movement.isMovable = false;
            comboIndex++;
            anim.SetInteger("ComboIndex", comboIndex);
            anim.SetTrigger("HeavyAttack");
            timeBtwAttacks = anim.GetCurrentAnimatorStateInfo(0).length;

        }
        if (timeBtwAttacks <= 0.1 && Input.GetMouseButton(0))
        {
            isAttacking = true;
            Player_Human_Movement.isMovable = false;
            comboIndex++;
            anim.SetInteger("ComboIndex", comboIndex);
            anim.SetTrigger("LightAttack");          
            timeBtwAttacks = anim.GetCurrentAnimatorStateInfo(0).length;

        } else if (timeBtwAttacks <= 0)
        {
            ResetComboIndex();
            Player_Human_Movement.isMovable = true;
            isAttacking = false;
        } 
        else
        {
            timeBtwAttacks -= Time.deltaTime;
        }
       
	}
    #endregion

    #region DamageEnemies(int damageTimer) private function and minor functions to now allow player movement with the animation frames
    private void DamageEnemies() //To be used in the animation frame events
    {
        Collider2D[] enemyToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
        for (int i = 0; i < enemyToDamage.Length; i++)
        {
            enemyToDamage[i].GetComponent<Enemy1>().TakeDamage(damage);
        }
        transform.Translate(0.05f, 0, 0);
    }
    private void Attacking()
    {
        Player_Human_Movement.isMovable = false;
    }

    private void NotAttacking()
    {
        Player_Human_Movement.isMovable = false;
    }
    #endregion

    #region ResetComboIndex private
    private void ResetComboIndex()
    {
        heavyComboIndex = 0;
        anim.SetInteger("HeavyComboIndex", 0);
        comboIndex = 0;
        anim.SetInteger("ComboIndex", 0);
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
