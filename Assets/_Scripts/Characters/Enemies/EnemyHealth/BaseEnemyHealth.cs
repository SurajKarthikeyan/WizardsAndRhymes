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
    /// Method that is unique to each enemy in its own death
    /// </summary>
    protected virtual void EnemyDeath()
    {
        Debug.Log("BaseEnemyDeath");
        Destroy(gameObject);
    }
    #endregion
}
