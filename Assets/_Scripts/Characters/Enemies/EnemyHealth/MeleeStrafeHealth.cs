using UnityEngine;
public class MeleeStrafeHealth : BaseEnemyHealth
{
    #region Custom Methods
    /// <summary>
    /// Method that is unique to each enemy in its own death
    /// </summary>
    protected override void EnemyDeath()
    {
        Debug.Log("MeleeStrafeDeath");
        Destroy(gameObject);
    }
    #endregion
}
