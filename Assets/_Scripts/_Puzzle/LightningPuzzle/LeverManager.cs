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
    [SerializeField] private Generator leverGenerator;
    #endregion


    #region CustomMethods

    public void CheckLevers()
    {
        isLeverCoolDown = true;
        StartCoroutine(LeverCoolDown());
        if (leverList.All(obj => obj.isOn) && leverGenerator.isLeverControlled)
        {
            completedLeverSystem = true;
            leverGenerator.Hodor();
        }
    }

    IEnumerator LeverCoolDown()
    {
        yield return new WaitForSeconds(leverCoolDown);
        isLeverCoolDown = false;
    }

    #endregion
}
