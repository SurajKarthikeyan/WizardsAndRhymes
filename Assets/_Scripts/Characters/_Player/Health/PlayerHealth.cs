using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class that handles all of the health and status of the player character
/// </summary>
public class PlayerHealth : Health
{
    #region Vars

    [SerializeField] private AK.Wwise.Event playerHurtSoundEffect;

    public PauseMenu pauseMenu;
 
    #endregion
    
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
