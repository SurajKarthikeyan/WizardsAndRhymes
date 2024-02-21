using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mixtape : MonoBehaviour
{

    [SerializeField] public Health.DamageType mixtapeDType;

    [SerializeField] private AK.Wwise.Event rangedEvent;
    [SerializeField] private AK.Wwise.Event meleeEvent;
    public Health.DamageType GetDamagePlaySound(bool isRanged)
    {
        if (isRanged)
        {
            rangedEvent.Post(this.gameObject);
        }
        else
        {
            meleeEvent.Post(this.gameObject);
        }
        return mixtapeDType;
    }
}
