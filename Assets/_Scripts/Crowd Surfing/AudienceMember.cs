using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents an audience member in the crowd
/// </summary>
public class AudienceMember : Floater
{
    #region Variables
    public enum FanMode { ConvertToPlayer, AlwaysPlayer, AlwaysWizzo}

    [Header("Audience Member Options")]
    [Tooltip("The renderer for this audience member")]
    [SerializeField] Renderer render;
    
    [Tooltip("The hands-down texture")]
    [SerializeField] Texture2D handsDown;
    [Tooltip("The hands-up texture")]
    [SerializeField] Texture2D handsUp;
    [Tooltip("The distance from the player at which to switch to the hands-up sprite")]
    [SerializeField] float handsUpDistance;
    
    [Tooltip("The possible colors for a Wizzo fan to be")]
    [SerializeField] Color[] wizzoColors;
    [Tooltip("The possible colors for a player fan to be")]
    [SerializeField] Color[] playerColors;

    [Tooltip("How this audience member handles who it is a fan of")]
    public FanMode fanMode = FanMode.ConvertToPlayer;
    [Tooltip("How long this audience member takes to fade to its player color")]
    [MMEnumCondition("fanMode", (int)FanMode.ConvertToPlayer, Hidden = true)]
    [SerializeField] float colorFadeDuration = 0.25f;

    [Tooltip("Whether this audience member is a fan of the player")]
    bool playerFan = false;
    [Tooltip("The percentage of enemies in the scene that must be killed before this audience member switches sides")]
    [SerializeField] float conversionPercentage;
    #endregion

    #region Unity Methods
    protected override void Start()
    {
        base.Start();

        render.material.color = RandomColor(false);
        
        if (fanMode == FanMode.ConvertToPlayer)
        {
            //Register enemy died callback
            BaseEnemyHealth.EnemyDied += CheckBecomePlayerFan;
            conversionPercentage = Random.Range(0.01f, 0.99f);
        }
        else if (fanMode == FanMode.AlwaysPlayer)
        {
            //Set initial fan state
            playerFan = true;
            render.material.color = RandomColor(playerFan);
        }
    }

    protected override void Update()
    {
        base.Update();

        if (playerFan && PlayerController.instance != null)
        {
            Vector3 playerOffset = PlayerController.instance.transform.position - transform.position;
            playerOffset.y = 0;
            float playerDistance = playerOffset.magnitude;

            if (playerDistance < handsUpDistance)
                render.material.mainTexture = handsUp;
            else
                render.material.mainTexture = handsDown;
        }
    }

     void OnDestroy()
    {
        if (fanMode == FanMode.ConvertToPlayer)
        {
            //Remove enemy died callback
            BaseEnemyHealth.EnemyDied -= CheckBecomePlayerFan;
        }
    }
    #endregion

    #region Custom Methods
    /// <summary>
    /// Pick a random color based on current allegiance
    /// </summary>
    /// <param name="playerFan">Whether to pick a player color or not</param>
    /// <returns>The random color selected</returns>
    Color RandomColor(bool playerFan)
    {
        Color[] colorArray = wizzoColors;
        if (playerFan)
            colorArray = playerColors;

        if (colorArray.Length <= 0)
        {
            Debug.LogWarning("No colors set for " + (playerFan ? "player fans" : "wizzo fans"));
            return Color.magenta;
        }

        int colorIndex = Random.Range(0, colorArray.Length);
        return colorArray[colorIndex];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="enemyGO"></param>
    void CheckBecomePlayerFan(GameObject enemyGO)
    {
        if (!playerFan)
        {
            if (conversionPercentage >= (float)(EnemyManager.RemainingEnemiesInScene - 1) / EnemyManager.TotalEnemiesInScene)
                StartCoroutine(BecomePlayerFan());
        }
        
    }

    /// <summary>
    /// Fade color to player color
    /// </summary>
    /// <returns>Coroutine</returns>
    IEnumerator BecomePlayerFan()
    {
        Color wizzoColor = render.material.color;
        Color playerColor = RandomColor(true);

        float t = 0;
        while (t < 1)
        {
            render.material.color = Color.Lerp(wizzoColor, playerColor, t);
            t += Time.deltaTime / colorFadeDuration;

            yield return new WaitForEndOfFrame();
        }

        playerFan = true;
    }
    #endregion
}
