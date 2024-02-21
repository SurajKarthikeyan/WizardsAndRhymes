using UnityEngine;

public class MeleeRetreatHealth : BaseEnemyHealth
{
    #region Custom Methods
    /// <summary>
    /// Calls parent function to ensure script is removed from manager
    /// Method that is unique to each enemy in its own death
    /// </summary>
    protected override void EnemyDeath()
    {
        base.EnemyDeath();
        StopAllCoroutines();
        Destroy(gameObject);
    }
    #endregion
}
