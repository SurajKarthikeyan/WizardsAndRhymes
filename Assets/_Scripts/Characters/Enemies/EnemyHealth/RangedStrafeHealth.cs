using UnityEngine;

public class RangedStrafeHealth : BaseEnemyHealth
{
    #region Custom Methods
    /// <summary>
    /// Method that is unique to each enemy in its own death
    /// </summary>
    protected override void EnemyDeath()
    {
        base.EnemyDeath();
        Debug.Log("DistancedProjectileStrafeDeath");
        Destroy(gameObject);
    }
    #endregion
}