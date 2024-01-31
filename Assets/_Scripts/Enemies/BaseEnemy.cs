using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Base class for enemies, all enemies derive in some way from here
/// </summary>

public class BaseEnemy : MonoBehaviour
{

    public int hp;

    protected virtual void FixedUpdate()
    {
        if (hp <= 0)
        {
            EnemyDeath();
        }
    }

    public void EnemyDeath()
    {
        EnemyDeathHelper();
        Destroy(gameObject);
    }

    protected virtual void EnemyDeathHelper()
    {
        Debug.Log("Base BaseEnemy Death Helper");
    }
}
