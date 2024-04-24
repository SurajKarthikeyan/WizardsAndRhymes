using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using MoreMountains.Feedbacks;
using Cinemachine;

/// <summary>
/// Handles the behaviour of the Wizzo Blast sequence
/// </summary>
public class WizzoBlastController : MonoBehaviour
{
    [Header("Component References")]
    [Tooltip("The MMF_Player to play feedbacks from when the orb grows")]
    [SerializeField] MMF_Player orbGrowFeedbacks;
    [Tooltip("The MMF_Player to play feedbacks from when the orb is struck")]
    [SerializeField] MMF_Player orbStrikeFeedbacks;
    [Tooltip("The scale root of the orb the player is blasting Wizzo with")]
    [SerializeField] Transform orbScaleRoot;
    [Tooltip("The animator on the player")]
    [SerializeField] Animator playerAnimator;
    [Tooltip("The transform the orb moves to once launched")]
    [SerializeField] Transform orbTargetTransform;
    [Tooltip("The splash screen GameObject")]
    [SerializeField] GameObject splashScreen;
    [Tooltip("The transition flash image")]
    [SerializeField] Image transitionFlash;
    [Tooltip("The MMShaker on the splash screen")]
    [SerializeField] MMShaker splashScreenShaker;
    [Tooltip("Sound effect for when the player presses the 'e' button")]
    [SerializeField] private AK.Wwise.Event chargeSoundEffect;
    
    
    [Header("Value Settings")]
    [Tooltip("The state on the player animator to play sequencially once the orb is charged")]
    [SerializeField] string[] playerAnimatorStates;

    [Header("Timing Settings")]
    [Tooltip("How big the orb must be to trigger the blast")]
    [SerializeField] float chargedOrbScale = 5;
    [Tooltip("How quickly the orb shrinks over time")]
    [SerializeField] float orbShrinkSpeed = 0.1f;
    [Tooltip("How long it takes the orb to reach its target")]
    [SerializeField] float orbTravelDuration = 0.5f;
    [Tooltip("The animation curve to follow as the orb moves towards its target")]
    [SerializeField] AnimationCurve orbTravelCurve;
    [Tooltip("How long it takes the transition flash to fade")]
    [SerializeField] float transitionFlashFadeDuration;

    [Tooltip("The PlayerInput instance used to detect input")]
    PlayerInput playerInput;
    [Tooltip("Whether the orb has been charged")]
    bool orbCharged = false;
    [Tooltip("Whether the orb has been struck")]
    bool orbStruck = false;
    [Tooltip("The index of the player animator states we are on")]
    int playerAnimatorStateIndex = 0;

    [Tooltip("Canvas holding the interact E button")]
    [SerializeField] private GameObject interactCanvas;

    [SerializeField] private AKChangeState akChangeState;

    /// <summary>
    /// Initialization
    /// </summary>
    private void Awake()
    {
        //Initialize input
        playerInput = new PlayerInput();
        playerInput.Player.Enable();
        playerInput.Player.Interact.started += DoInteract;
        akChangeState.ChangeState();
    }

    private void Update()
    {
        //Shink over time
        if (!orbCharged)
        {
            orbScaleRoot.transform.localScale -= Vector3.one * orbShrinkSpeed * Time.deltaTime;
            orbScaleRoot.transform.localScale = Vector3.Max(Vector3.one, orbScaleRoot.transform.localScale);

            //Check if the orb is big enough to start the blast
            if (orbScaleRoot.transform.localScale.x >= chargedOrbScale)
            {
                orbCharged = true;
                Debug.Log("Orb charged");
            }
        }
    }

    /// <summary>
    /// Called whenever the interact button is pressed
    /// </summary>
    /// <param name="obj">The input action callback context</param>
    private void DoInteract(InputAction.CallbackContext obj)
    {
        Debug.Log("Interacted");
        if (!orbCharged)
        {
            chargeSoundEffect.Post(this.gameObject);
            orbGrowFeedbacks.PlayFeedbacks();
        }
        else if (!orbStruck)
        {
            if (playerAnimatorStateIndex < playerAnimatorStates.Length)
            {
                if (playerAnimatorStateIndex == 0 || playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                {
                    chargeSoundEffect.Post(this.gameObject);
                    orbStrikeFeedbacks.PlayFeedbacks();
                    playerAnimator.Play(playerAnimatorStates[playerAnimatorStateIndex]);
                    playerAnimatorStateIndex++;

                    if (playerAnimatorStateIndex >= playerAnimatorStates.Length)
                    {
                        orbStruck = true;
                        StartCoroutine(OrbFire());
                    }
                }
            }
        }
    }

    IEnumerator OrbFire()
    {
        yield return new WaitForSeconds(0.5f); //Wait for the position shaker to be done

        float startTime = Time.time;
        Vector3 startPosition = orbScaleRoot.transform.position;
        chargeSoundEffect.Post(this.gameObject);
        while (Time.time - startTime < orbTravelDuration)
        {
            float t = (Time.time - startTime) / orbTravelDuration;
            orbScaleRoot.transform.position = Vector3.Lerp(startPosition, orbTargetTransform.position, orbTravelCurve.Evaluate(t));

            yield return new WaitForEndOfFrame();
        }
        chargeSoundEffect.Post(this.gameObject);
        
        splashScreen.SetActive(true);
        splashScreenShaker.Play();
        float transitionStartTime = Time.time;

        interactCanvas.SetActive(false);
        while (Time.time - transitionStartTime < transitionFlashFadeDuration)
        {
            float t = (Time.time - transitionStartTime) / transitionFlashFadeDuration;

            transitionFlash.color = Color.Lerp(Color.black, new Color(0, 0, 0, 0), t);

            yield return new WaitForEndOfFrame();
        }
        transitionFlash.color = new Color(0, 0, 0, 0);
    }
}
