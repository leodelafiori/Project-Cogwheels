using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Singleton
/// ////////////////// GameManager
/// Basically a script on an element of the game that will always be active (will manage player health,damage,saves)

public class GameManager : MonoBehaviour {

    #region Declaring variables
//Main Variables
    public static GameManager instance = null; //Static to only have 1 in memory
    private bool playerActive = false;
    private bool gameOver = false;
 //Player Health
    private float currentPlayerHealth = 100;
    private float maxPlayerHealth = 100;

    #endregion
    #region Getters and setters
    /// Data encapsulation
    /// Getters and setters
    /// Getters are being use for read-only on other scripts that are not the GameManager
    public bool PlayerActive
    {
        get { return playerActive;}
    }
    public bool GameOver
    {
        get { return gameOver; }
    }
    public float CurrentPlayerHealth
    {
        get { return currentPlayerHealth; }
    }
    public float MaxPlayerHealth
    {
        get { return maxPlayerHealth; }
    }

    #endregion
    #region Awake
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
    #endregion
    #region FixedUpdate/Update
    private void Update()
    {
    //Player Health
        if(currentPlayerHealth <= 0)
        {
            StateGameOver();
        }
    }
    #endregion
    #region Player Hit/Healed
    public void PlayerHit(float damage)
    {
        if (Player_Human_Movement.isBlocking)
        {
            currentPlayerHealth -= damage/5;
        } else if (!Player_Human_Movement.isRolling)
        {
            currentPlayerHealth -= damage;
        } 
        
    }
    #endregion
    #region GameStates
    public void StateGameOver()
    {
        Time.timeScale = 0;
    }
    #endregion


}
