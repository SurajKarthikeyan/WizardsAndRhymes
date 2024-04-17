using UnityEngine;
public class MeleeStrafeHealth : BaseEnemyHealth
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
        LightningVFXPosition[] lVFXArray = GetComponentsInChildren<LightningVFXPosition>();
        for (int i = 0; i < lVFXArray.Length; i++)
        {
            Destroy(lVFXArray[i].gameObject);
        }
       Destroy(gameObject);
    }
    #endregion
}
