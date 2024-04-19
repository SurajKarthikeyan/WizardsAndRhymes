using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizzoCamTransitionTrigger : MonoBehaviour
{
    #region Vars
    [SerializeField] private Animator cinemachineAnimator;
    #endregion

    #region CustomMethods

    public void ChangeToWizzoCam()
    {
        cinemachineAnimator.SetBool("WizzoCam", true);
    }

    public void ChangeOutWizzoCam()
    {
        cinemachineAnimator.SetBool("WizzoCam", false);
    }
    #endregion
}
