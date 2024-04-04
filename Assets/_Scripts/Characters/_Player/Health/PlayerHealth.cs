using System;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Class that handles all of the health and status of the player character
/// </summary>
public class PlayerHealth : Health
{
    #region Vars

    [SerializeField] private AK.Wwise.Event playerHurtSoundEffect;

    public PauseMenu pauseMenu;

    public List<GameObject> hearts = new();

    public Sprite fullHeartImage;

    public Sprite emptyHeartImage;

    private float currHealthPercentage;
 
    #endregion

    private void Update()
    {
        float healthPercentage = currentHP / maximumHP;

        if (healthPercentage != currHealthPercentage)
        {
            currHealthPercentage = healthPercentage;
            
            int heartIndex = (int)(healthPercentage * hearts.Count);

            for (int i = 0; i < hearts.Count; i++)
            {
                if (hearts[i].TryGetComponent(out Image heartImage))
                {
                    if (i < heartIndex)
                    {
                        heartImage.sprite = fullHeartImage;
                    }
                    else
                    {
                        heartImage.sprite = emptyHeartImage;
                    }
                }
            
            }
        }

    }

    #region Custom Methods
    /// <summary>
    /// Overridden function of all deaths
    /// </summary>
    public override void Death()
    {
        PlayerDeath();
    }
    /// <summary>
    /// Method that is called upon the player's death
    /// </summary>
    private void PlayerDeath()
    {
        Debug.Log("PlayerDeath");
        PlayerController.instance.DisablePlayerControls();
        pauseMenu.PlayerDeathUI();
    }

    public override void TakeDamage(float value, DamageType dType)
    {
        base.TakeDamage(value, dType);
        playerHurtSoundEffect.Post(gameObject);
    }
    #endregion
}
