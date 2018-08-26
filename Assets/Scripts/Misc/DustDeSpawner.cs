using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustDeSpawner : MonoBehaviour {
    //This scripts only serves to despawn dust from the game to save memory

    #region declaring variables
    private float timer = 1f;
    #endregion
    #region main code
    private void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            Destroy(gameObject);
        }
    }
    #endregion

}
