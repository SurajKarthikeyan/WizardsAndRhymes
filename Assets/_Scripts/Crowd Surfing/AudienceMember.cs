using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class AudienceMember : Floater
{
    #region Variables
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

    [Tooltip("The maximum amount of time this audience member can delay becoming a fan of the player")]
    [SerializeField] float maxPlayerFanDelay = 1f;
    [Tooltip("How long this audience member takes to fade to its player color")]
    [SerializeField] float colorFadeDuration = 0.25f;

    [Tooltip("Whether this audience member is a fan of the player")]
    bool playerFan = false;
    #endregion

    #region Unity Methods
    protected override void Start()
    {
        base.Start();

        render.material.color = RandomColor(false);

        //Register room cleared callback
        EnemyManager.RoomCleared += () => { StartCoroutine(BecomePlayerFan()); };
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

    IEnumerator BecomePlayerFan()
    {
        yield return new WaitForSeconds(Random.Range(0, maxPlayerFanDelay));

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
