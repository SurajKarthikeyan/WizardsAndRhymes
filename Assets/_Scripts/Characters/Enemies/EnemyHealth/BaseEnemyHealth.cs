using UnityEngine;

/// <summary>
/// Base class that handles all enemy health, status effects, and death
/// </summary>
public abstract class BaseEnemyHealth : Health
{
    #region Custom Methods
    /// <summary>
    /// Death method that is overridden for all characters with health
    /// </summary>
    public override void Death()
    {
        EnemyDeath();
    }
    /// <summary>
    /// Death method here is generalized to ensure that the script is removed from the manager
    /// </summary>
    protected virtual void EnemyDeath()
    {
        EnemyManager.enemyManager.enemies.Remove(this.gameObject.GetComponent<BaseEnemyBehavior>());

        /*Debug.Log("BaseEnemyDeath");
        Destroy(gameObject);*/
    }
    #endregion
}
