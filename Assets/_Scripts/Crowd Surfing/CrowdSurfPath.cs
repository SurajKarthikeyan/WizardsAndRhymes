using Dreamteck.Splines;
using Language.Lua;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines a path for the player to crowd surf along
/// </summary>
public class CrowdSurfPath : MonoBehaviour
{
    #region Variables
    [Tooltip("The spline computer component that defines the path")]
    [SerializeField] SplineComputer splineComputer;
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

    [Tooltip("Whether the player is currently crowd-surfing on this path")]
    public bool PlayerOnPath { get; private set; }
    #endregion

    #region Unity Methods
    private void Start()
    {
        //Register enemies defeated callback
        if (activateOnRoomCleared)
            EnemyManager.RoomCleared += () => { active = true; };
    }
    #endregion

    #region Custom Methods
    public void StartCrowdSurf(bool reverse)
    {
        if (active)
            StartCoroutine(CrowdSurf(reverse));
    }

    IEnumerator CrowdSurf(bool reverse)
    {
        Debug.Log("Starting crowd surf");

        PlayerOnPath = true;

        PlayerController.instance.AddMovementLock(this); //Lock the player's movement
        Transform playerTransform = PlayerController.instance.transform;
        //Move player to start of path
        Vector3 pathStartPosition = splineComputer.EvaluatePosition(0);
        if (reverse)
            pathStartPosition = splineComputer.EvaluatePosition(1);
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

        Debug.Log("Player at start of path");

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

        Debug.Log("Player at end of path");

        //Restore normal movement
        PlayerController.instance.RemoveMovementLock(this);

        yield return new WaitForSeconds(pathCooldown); //Prevent triggers from firing twice

        PlayerOnPath = false;
    }

    public Vector3 GetPositionOnPath(float percentage)
    {
        return splineComputer.EvaluatePosition(percentage);
    }

    public bool IsTwoWay()
    {
        return twoWay;
    }
    #endregion
}
