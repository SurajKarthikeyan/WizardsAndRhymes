using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Class that is responsible for controlling the player character and managing its abilities
/// 
/// Author: Zane O'Dell
/// </summary>
public class PlayerController : MonoBehaviour
{
    #region Class Variables

    #region Input
    /// <summary>
    /// C# Class generated from the input action map
    /// </summary>
    private PlayerInput playerInput;

    /// <summary>
    /// Move input action from the PlayerInput action map
    /// </summary>
    private InputAction move;
    #endregion

    #region Movement Variables
    /// <summary>
    /// Rigidbody of the player
    /// </summary>
    private Rigidbody rb;

    /// <summary>
    /// Magnitude of force applied to player Rigidbody for movement
    /// </summary>
    [SerializeField]
    private float movementForce = 1f;

    /// <summary>
    /// Maximum speed that the player is allowed to move while dashing
    /// </summary>
    [SerializeField]
    private float maxDashSpeed = 10f;

    /// <summary>
    /// Maximum speed that the player is allowed to move generally
    /// </summary>
    [SerializeField]
    private float maxMoveSpeed = 5f;

    /// <summary>
    /// Direction to apply the force of movement when moving
    /// </summary>
    private Vector3 forceDirection = Vector3.zero;


    private bool canDash;

    /// <summary>
    /// Cooldown of the dash in seconds
    /// </summary>
    [SerializeField]
    private float dashCooldown = 1.5f;
    #endregion

    #region Camera
    /// <summary>
    /// Reference to the camera focusing on the player
    /// </summary>
    [SerializeField]
    private Camera playerCamera;
    #endregion


    #region Ability Slider
    public GameObject abilitySliderGO;

    private Slider abilitySlider;

    private Vector3 uiOffsetVec = new Vector3(-0.1f, -0.45f, -1.25f);

    public GameObject sliderFill;

    public float abilityValue = 100f;

    public float abilityRechargeThreshold = 2f;

    public float abilityRechargeTimer;

    public float dashCooldownThreshold = 2f;

    public float dashCooldownTimer;
    
    #endregion

    #endregion

    #region Properties
    public bool CanDash => abilityValue >= 10 && dashCooldownTimer >= dashCooldownThreshold;
    #endregion

    #region Enum and Enum Instances

    /// <summary>
    /// Enum that represents the movement state of the player
    /// </summary>
    private enum MoveStatus
    {
        Moving,
        Dashing
    }

    /// <summary>
    /// MoveStatus instance
    /// </summary>
    private MoveStatus moveStatus;

    #endregion

    #region Methods

    #region Unity Methods
    /// <summary>
    /// Method called on scene startup
    /// </summary>
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = new PlayerInput();
        abilitySlider = abilitySliderGO.GetComponent<Slider>();
        abilitySlider.value = abilityValue;
    }

    /// <summary>
    /// Method called when this script is enabled
    /// </summary>
    private void OnEnable()
    {
        playerInput.Player.Dash.started += DoDash;
        playerInput.Player.RangedAttack.started += DoRanged;
        playerInput.Player.MeleeAttack.started += DoMelee;
        move = playerInput.Player.Movement;
        playerInput.Player.Enable();
    }


    /// <summary>
    /// Method called when this script is disabled
    /// </summary>
    private void OnDisable()
    {
        playerInput.Player.Dash.canceled -= DoDash;
        playerInput.Player.RangedAttack.canceled -= DoRanged;
        playerInput.Player.MeleeAttack.canceled -= DoMelee;
        playerInput.Player.Disable();
    }



    /// <summary>
    /// Function called once every frame, generally 60 frames per second
    /// </summary>
    private void FixedUpdate()
    {
        abilitySliderGO.transform.position = transform.position + uiOffsetVec;

        abilityRechargeTimer += Time.deltaTime;

        dashCooldownTimer += Time.deltaTime;

        if (abilityRechargeTimer >= abilityRechargeThreshold)
        {
            FillAbilityGauge();
        }

        abilityValue = Mathf.Clamp(abilityValue, 0, 100);
        abilitySlider.value = abilityValue;

        if (abilitySlider.value <= 0)
        {
            sliderFill.SetActive(false);
        }
        else if (abilitySlider.value > 0 && !sliderFill.activeInHierarchy)
        {
            sliderFill.SetActive(true);
        }

        forceDirection += move.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * movementForce;
        forceDirection += move.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * movementForce;

        rb.AddForce(forceDirection, ForceMode.Impulse);
        forceDirection = Vector3.zero;

        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0;
        if (horizontalVelocity.sqrMagnitude > maxMoveSpeed * maxMoveSpeed && moveStatus == MoveStatus.Moving)
        {
            rb.velocity = horizontalVelocity.normalized * maxMoveSpeed + Vector3.up * rb.velocity.y;
        }
        else if (horizontalVelocity.sqrMagnitude > maxDashSpeed * maxDashSpeed && moveStatus == MoveStatus.Dashing)
        {
            rb.velocity = horizontalVelocity.normalized * maxDashSpeed + Vector3.up * rb.velocity.y;
        }

        LookAt();
    }
    #endregion

    #region Custom Methods
    /// <summary>
    /// Function that makes the player look in the direction that it's moving
    /// </summary>
    private void LookAt()
    {
        Vector3 direction = rb.velocity;
        direction.y = 0f;

        if (move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
        {
            rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
        else
        {
            rb.angularVelocity = Vector3.zero;
        }
    }
    /// <summary>
    /// Gets the forward direction of the camera regarding the x and z directions
    /// </summary>
    /// <param name="playerCamera">Reference to the camera looking at the player</param>
    /// <returns>Vector3 representing the forward direction of the camera in the x and z directions</returns>
    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    /// <summary>
    /// Gets the right direction of the camera regarding the x and z directions
    /// </summary>
    /// <param name="playerCamera">Reference to the camera looking at the player</param>
    /// <returns>Vector3 representing the right direction of the camera in the x and z directions</returns>
    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }

    /// <summary>
    /// Function that is called upon pressing any of the Ranged Attack inputs
    /// </summary>
    /// <param name="obj">Input callback context for the ranged attack</param>
    private void DoRanged(InputAction.CallbackContext obj)
    {
        Debug.Log("Ranged attack");
        abilityRechargeTimer = 0;
        StartCoroutine(Projectile());
    }
    /// <summary>
    /// Function that is called upon pressing any of the Melee Attack inputs
    /// </summary>
    /// <param name="obj">Input callback context for the melee attack</param>
    private void DoMelee(InputAction.CallbackContext obj)
    {
        Debug.Log("Melee attack");
        abilityRechargeTimer = 0;
    }

    /// <summary>
    /// Function that is called upon pressing any of the Dash inputs
    /// </summary>
    /// <param name="obj">Input callback context for the dash</param>
    private void DoDash(InputAction.CallbackContext obj)
    {
        abilityRechargeTimer = 0;
        if (CanDash)
        {
            print("Starting dash");
            abilityValue -= 10;
            
            rb.AddForce(forceDirection, ForceMode.Impulse);
            if (moveStatus == MoveStatus.Moving)
            {
                StartCoroutine(Dash());
            }
        }
    }
    /// <summary>
    /// Coroutine called in the do dash function that sets timing status for the dash
    /// </summary>
    /// <returns>Various wait for seconds in between cooldowns</returns>
    IEnumerator Dash()
    {
        moveStatus = MoveStatus.Dashing;
        dashCooldownTimer = 0;
        yield return new WaitForSeconds(0.5f);
        moveStatus = MoveStatus.Moving;
        print("Ending dash");
    }

    /// <summary>
    /// Coroutine called when the projectile function is called, handles cooldowns
    /// </summary>
    /// <returns>Various wait for seconds in between cooldowns</returns>
    IEnumerator Projectile()
    {
        abilityValue -= 2;
        abilityRechargeTimer = 0;
        yield return new WaitForSeconds(0.3f);
    }
    /// <summary>
    /// Functioncalled routinely to refill ability guage when not dashing or attacking
    /// </summary>
    void FillAbilityGauge()
    {
        abilityValue += 3;
        abilityRechargeTimer = 0;
    }
    #endregion



    #endregion
}
