using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    #region Declaring Variables
    private Image currentHealth;
    #endregion

    private void Start()
    {
        currentHealth = GetComponent<Image>();
    }

    private void Update()
    {
        UpdateHealthBar();
    }
    public void UpdateHealthBar()
    {
        float healthRatio = GameManager.instance.CurrentPlayerHealth / GameManager.instance.MaxPlayerHealth;
        if(healthRatio >= 0)
        {
            currentHealth.rectTransform.localScale = new Vector3(healthRatio, 1, 1);
        } else {}
        
    }

}
