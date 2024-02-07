using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCollider : MonoBehaviour
{
    PlayerController player;

    BaseEnemy enemy;

    private void Start()
    {
        player = transform.GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<BaseEnemy>(out var enemy))
        {
            enemy.gameObject.GetComponent<Character>().TakeDamage(player.meleeDamage, Character.DamageType.Fire);
        }
    }
}
