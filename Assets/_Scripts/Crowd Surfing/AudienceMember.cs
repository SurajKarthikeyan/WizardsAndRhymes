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
    [Tooltip("The enemy manager this audience members tracks to determine when to convert to a player fan")]
    [MMEnumCondition("fanMode", (int)FanMode.ConvertToPlayer, Hidden = true)]
    public EnemyManager enemyManager;
    [Tooltip("How long this audience member takes to fade to its player color")]
    [MMEnumCondition("fanMode", (int)FanMode.ConvertToPlayer, Hidden = true)]
    [SerializeField] float colorFadeDuration = 0.25f;

    [Tooltip("Whether this audience member is a fan of the player")]
    bool playerFan = false;
    [Tooltip("The percentage of enemies in the scene that must be killed before this audience member switches sides")]
    float conversionPercentage;
    [Tooltip("Whether this audience member is currently fading")]
    bool isFading = false;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        render.material.color = RandomColor(false);
        
        if (fanMode == FanMode.ConvertToPlayer)
        {
            //Register enemy died callback
            BaseEnemyHealth.EnemyDied += CheckBecomePlayerFan;
            conversionPercentage = Random.Range(0.01f, 0.99f);

            //Register waves cleared callback
            EnemyManager.WavesCleared += CheckWavesCleared;
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
            //Remove callbacks
            BaseEnemyHealth.EnemyDied -= CheckBecomePlayerFan;
            EnemyManager.WavesCleared -= CheckWavesCleared;
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
    /// Check whether this audience member should convert to being a player fan
    /// </summary>
    /// <param name="enemyGO">The enemy that was killed to trigger this function (unused)</param>
    void CheckBecomePlayerFan(GameObject enemyGO)
    {
        if (!playerFan && enemyManager != null)
        {
            if (conversionPercentage >= (float)(enemyManager.RemainingEnemies) / enemyManager.TotalEnemies)
                StartBecomePlayerFan();
        }
    }

    /// <summary>
    /// Called when an enemy manager has all of its waves cleared, convert to player fan if it is this audience member's enemy manager
    /// </summary>
    /// <param name="clearedEnemyManager">The enemy manager that had all its waves cleared</param>
    void CheckWavesCleared(EnemyManager clearedEnemyManager)
    {
        if (clearedEnemyManager == enemyManager)
        {
            StartBecomePlayerFan(false);
        }
    }

    /// <summary>
    /// Become a player fan and start the fade coroutine
    /// </summary>
    void StartBecomePlayerFan(bool fade = true)
    {
        if (!playerFan)
        {
            playerFan = true;
            if (fade)
                StartCoroutine(BecomePlayerFan());
            else if (!isFading)
                render.material.color = RandomColor(true);
        }
    }

    /// <summary>
    /// Fade color to player color
    /// </summary>
    /// <returns>Coroutine</returns>
    IEnumerator BecomePlayerFan()
    {
        isFading = true;

        Color wizzoColor = render.material.color;
        Color playerColor = RandomColor(true);

        float t = 0;
        while (t < 1)
        {
            render.material.color = Color.Lerp(wizzoColor, playerColor, t);
            t += Time.deltaTime / colorFadeDuration;

            yield return new WaitForEndOfFrame();
        }
    }
    #endregion
}
