using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class that handles all of the health and status of the player character
/// </summary>
public class PlayerHealth : Health
{
    #region Vars

    [SerializeField] private AK.Wwise.Event playerHurtSoundEffect;
 
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
        Heal(0, true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public override void TakeDamage(float value, DamageType dType)
    {
        base.TakeDamage(value, dType);
        playerHurtSoundEffect.Post(this.gameObject);
    }
    #endregion
}
