using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LeverManager : MonoBehaviour
{

    #region Vars

    [SerializeField] private List<Lever> leverList;
    [SerializeField] public bool isLeverCoolDown;
    [SerializeField] private int leverCoolDown;
    [SerializeField] public bool completedLeverSystem;
    #endregion


    #region CustomMethods

    public void CheckLevers()
    {
        isLeverCoolDown = true;
        StartCoroutine(LeverCoolDown());
        bool check =  leverList.All(obj => obj.isOn);
        if (check)
        {
            Debug.Log("Here");
            completedLeverSystem = true;
            //Call generator on
        }
    }

    IEnumerator LeverCoolDown()
    {
        yield return new WaitForSeconds(leverCoolDown);
        isLeverCoolDown = false;
    }

    #endregion
}
