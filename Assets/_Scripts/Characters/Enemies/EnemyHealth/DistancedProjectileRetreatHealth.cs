using UnityEngine;

public class DistancedProjectileRetreatHealth : BaseEnemyHealth
{
    #region Custom Methods
    /// <summary>
    /// Method that is unique to each enemy in its own death
    /// </summary>
    protected override void EnemyDeath()
    {
        base.EnemyDeath();
        Debug.Log("DistancedProjectileRetreatDeath");
        Destroy(gameObject);
    }
    #endregion
}
