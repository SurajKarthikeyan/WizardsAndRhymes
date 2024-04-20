using Dreamteck.Splines;
using Language.Lua;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Defines a path for the player to crowd surf along
/// </summary>
public class CrowdSurfPath : MonoBehaviour
{
    #region Variables
    [Tooltip("The spline computer component that defines the path")]
    [SerializeField] SplineComputer splineComputer;
    [Tooltip("The trigger at the start of the path")]
    [SerializeField] CrowdSurfTrigger startTrigger;
    [Tooltip("The trigger at the end of the path")]
    [SerializeField] CrowdSurfTrigger endTrigger;
    [Tooltip("How fast the player should move to the start of the path")]
    [SerializeField] float approachSpeed = 15f;
    [Tooltip("How fast the player should move along the path")]
    [SerializeField] float pathSpeed = 25f;
    [Tooltip("How long the player must wait after using this path before they can use it again")]
    [SerializeField] float pathCooldown = 1f;
    [Tooltip("Whether this path is two-way")]
    [SerializeField] bool twoWay;
    [Tooltip("Whether this path is active")]
    public bool active;
    [Tooltip("Whether this path activates when all enemies have been defeated")]
    [SerializeField] bool activateOnRoomCleared = true;
    [Tooltip("Whether to automatically place the start and and triggers at the start and end of the path")]
    [SerializeField] bool autoPlaceTriggers = true;
    [Tooltip("Whether to automatically place the start and and triggers at the start and end of the path")]
    public bool isReversed;

    [SerializeField]
    private AK.Wwise.Event crowdSurfSoundEffect;
    
    [Tooltip("Whether the player is currently crowd-surfing on this path")] 
    public bool PlayerOnPath { get; private set; }
    #endregion

    #region Unity Methods
    /// <summary>
    /// Initialization
    /// </summary>
    private void Start()
    {
        //Register enemies defeated callback
        if (activateOnRoomCleared)
            EnemyManager.RoomCleared += () => { active = true; };

        //Initialize start and end triggers
        InitializeTriggers();
    }
    #endregion

    #region Custom Methods
    /// <summary>
    /// Initialize the values of the triggers at the start and end of this crowd surf path
    /// </summary>
    [ContextMenu("Initialize Triggers")]
    void InitializeTriggers()
    {
        //References
        startTrigger.crowdSurfPath = this;
        endTrigger.crowdSurfPath = this;

        //Auto place
        if (autoPlaceTriggers)
        {
            startTrigger.transform.position = splineComputer.EvaluatePosition(0.0);
            endTrigger.transform.position = splineComputer.EvaluatePosition(1.0);
        }

        //Configure end trigger
        endTrigger.end = true;
        if (!twoWay)
            endTrigger.gameObject.SetActive(false);
    }

    /// <summary>
    /// Trigger a crowd surf. Called by a CrowdSurfTrigger
    /// </summary>
    /// <param name="reverse">Whether this crowd surf goes from end to start</param>
    public void StartCrowdSurf(bool reverse)
    {
        if (active)
            StartCoroutine(CrowdSurf(reverse));
    }

    /// <summary>
    /// Move the player along the crowd surf path
    /// </summary>
    /// <param name="reverse">Whether the crowd surf goes from end to start</param>
    /// <returns>Coroutine</returns>
    IEnumerator CrowdSurf(bool reverse)
    {
        //play enter state
        crowdSurfSoundEffect.Post(this.GameObject());
        AkSoundEngine.SetState("CrowdSurfing", "InCrowdSurf");
        PlayerOnPath = true;

        PlayerController.instance.DisablePlayerControls();
        PlayerController.instance.AddMovementLock(this); //Lock the player's movement
        Transform playerTransform = PlayerController.instance.transform;
        //Move player to start of path
        Vector3 pathStartPosition = splineComputer.EvaluatePosition(0.0);
        if (reverse)
            pathStartPosition = splineComputer.EvaluatePosition(1.0);
        Vector3 playerStartPosition = playerTransform.position;
        pathStartPosition.y = playerStartPosition.y;
        float distance = Vector3.Distance(pathStartPosition, playerStartPosition);
        float t = 0;
        while (t < 1)
        {
            playerTransform.position = Vector3.Lerp(playerStartPosition, pathStartPosition, t);
            t += (approachSpeed / distance) * Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        //Move player along path
        distance = splineComputer.CalculateLength();
        t = 0;
        while (t < 1)
        {
            if (!reverse)
                playerTransform.position = splineComputer.EvaluatePosition(t);
            else
                playerTransform.position = splineComputer.EvaluatePosition(1 - t);

            playerTransform.position = new Vector3(playerTransform.position.x, playerStartPosition.y, playerTransform.position.z); //Keep player on same horizontal plane. Only follow spline in X and Z axes
            t += (pathSpeed / distance) * Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        //Restore normal movement
        PlayerController.instance.RemoveMovementLock(this);
        PlayerController.instance.EnablePlayerControls();

        yield return new WaitForSeconds(pathCooldown); //Prevent triggers from firing twice

        PlayerOnPath = false;
        AkSoundEngine.SetState("CrowdSurfing", "OutOfCrowdSurf");
    }

    /// <summary>
    /// Sets whether this crowd surf path is active
    /// </summary>
    /// <param name="value">The value to set</param>
    public void SetActive(bool value)
    {
        active = value;
    }
    #endregion
}
