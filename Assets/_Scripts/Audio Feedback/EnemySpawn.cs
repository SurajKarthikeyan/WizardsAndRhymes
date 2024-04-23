using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    #region Vars

    [SerializeField] private AK.Wwise.Event enemySpawnSoundEffect;

    #endregion

    #region CustomMethods

    public void SoundOnEnemySpawn()
    {
        enemySpawnSoundEffect.Post(this.gameObject);
    }

    #endregion
}
