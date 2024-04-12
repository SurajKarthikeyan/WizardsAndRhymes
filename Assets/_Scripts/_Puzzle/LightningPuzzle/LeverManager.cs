using System;
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
    [SerializeField] private LightningBlockManager lbManager;
    [SerializeField] private bool useLBManager;
    [SerializeField] private TeslaTower teslaTower;
    [SerializeField] private List<IndividualEmissionChange> emissionChangeList;
    #endregion


    #region CustomMethods

    public void CheckLevers()
    {
        isLeverCoolDown = true;
        if (useLBManager)
        {
            lbManager.SetHoloBlocks();
        }
        if (leverList.All(obj => obj.isOn) && leverGenerator.isLeverControlled)
        {
            completedLeverSystem = true;
            if (teslaTower != null)
            {
                teslaTower.DisableElectricEffect();
            }
            for (int i = 0; i < emissionChangeList.Count; i++)
            {
                StartCoroutine(emissionChangeList[i].AlterEmissionOverTime(true));
            }
            leverGenerator.Hodor();
        }
        StartCoroutine(LeverCoolDown());

    }

    IEnumerator LeverCoolDown()
    {
        yield return new WaitForSeconds(leverCoolDown);
        isLeverCoolDown = false;
    }

    #endregion
}
