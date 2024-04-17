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
        StopAllCoroutines();
        LightningVFXPosition[] lVFXArray = GetComponentsInChildren<LightningVFXPosition>();
        for (int i = 0; i < lVFXArray.Length; i++)
        {
            Destroy(lVFXArray[i].gameObject);
        }
        Destroy(gameObject);
    }
    #endregion
}
