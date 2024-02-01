using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<BaseEnemy> enemies;
    // Start is called before the first frame update
    void Start()
    {
        enemies = FindObjectsOfType<BaseEnemy>().ToList();
    }

    public void ActivateEnemies()
    {
        foreach (BaseEnemy enemy in enemies)
        {
            enemy.GetComponent<MeshRenderer>().material = enemy.m_ActivatedMaterial;
            enemy.m_Activated = true;
        }
        
    }
}
