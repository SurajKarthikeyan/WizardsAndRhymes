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


    private InputAction look;
    #endregion

    #region Movement Variables
    /// <summary>
    /// Rigidbody of the player
    /// </summary>
    private Rigidbody rb;

    /// <summary>
    /// Magnitude of appliedForce applied to player Rigidbody for movement
    /// </summary>
    [SerializeField]
    private float movementForce = 1f;

    /// <summary>
    /// Magnitude of appliedForce applied to player Rigidbody for movement
    /// </summary>
    [SerializeField]
    private float dashForce = 3f;

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
    /// Direction to apply the appliedForce of movement when moving
    /// </summary>
    private Vector3 forceDirection = Vector3.zero;


    private Vector3 lookDirectionVector = Vector3.zero;
    /// <summary>
    /// Cooldown of the dash in seconds
    /// </summary>
    //[SerializeField]
    //private float dashCooldown = 1.5f;
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


    public GameObject rangedPrefab;

    public Transform rangedSpawnPoint;

    public float rangedPrefabSpeed = 5f;
    #endregion

    #endregion

    #region Properties
    public bool CanDash => abilityValue >= 10 && dashCooldownTimer >= dashCooldownThreshold;

    public bool IsMoving => move.ReadValue<Vector2>().sqrMagnitude > 0.1f;
    #endregion

    #region Enum and Enum Instances

    /// <summary>
    /// Enum that represents the movement state of the player
    /// </summary>
    private enum MoveStatus
    {
        Idle,
        Moving,
        Dashing
    }

    /// <summary>
    /// MoveStatus instance
    /// </summary>
    private MoveStatus moveStatus;


    private enum AttackStatus
    {
        None, 
        Ranged,
        Melee
    }

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
        look = playerInput.Player.Look;
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
        //Debug.Log(rb.velocity.magnitude);
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

        //lookDirectionVector = look.ReadValue<Vector2>().x * GetCameraRight(playerCamera);
        //lookDirectionVector = look.ReadValue<Vector2>().y * GetCameraForward(playerCamera);

        Debug.Log(lookDirectionVector);

        if (IsMoving)
        {
            float appliedForce;
            if (moveStatus == MoveStatus.Dashing)
            {
                appliedForce = dashForce;
            }
            else
            {
                appliedForce = movementForce;
                moveStatus = MoveStatus.Moving;
            }

            forceDirection += move.ReadValue<Vector2>().x * appliedForce * GetCameraRight(playerCamera);
            forceDirection += move.ReadValue<Vector2>().y * appliedForce * GetCameraForward(playerCamera);

            rb.AddForce(forceDirection, ForceMode.Impulse);
            forceDirection = Vector3.zero;

            Vector3 horizontalVelocity = rb.velocity;
            horizontalVelocity.y = 0;
            if (horizontalVelocity.sqrMagnitude > maxDashSpeed * maxDashSpeed && moveStatus == MoveStatus.Dashing)
            {
                rb.velocity = horizontalVelocity.normalized * maxDashSpeed + Vector3.up * rb.velocity.y;
            }
            else if (horizontalVelocity.sqrMagnitude > maxMoveSpeed * maxMoveSpeed && moveStatus == MoveStatus.Moving)
            {
                rb.velocity = horizontalVelocity.normalized * maxMoveSpeed + Vector3.up * rb.velocity.y;
            }
        }
        else
        {
            moveStatus = MoveStatus.Idle;
        }

        LookAndMoveStatus();
    }
    #endregion

    #region Custom Methods
    /// <summary>
    /// Function that makes the player look in the direction that it's moving
    /// </summary>
    private void LookAndMoveStatus()
    {
        //Vector3 direction = lookDirectionVector;
        ////lookDirectionVector = Vector3.zero;
        //direction.y = 0f;

        ////Debug.Log(direction);

        //rb.rotation = Quaternion.LookRotation(direction, Vector3.up);

        //if (move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
        //{
        //    rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        //}
        //else
        //{ 
        //    rb.angularVelocity = Vector3.zero;
        //}

        Vector2 aim = playerInput.Player.Look.ReadValue<Vector2>();
        Vector3 direction = new(aim.x, 0, aim.y);
        if (aim.magnitude > 0.2f)
        {
            Vector3 rotation = Vector3.Slerp(rb.rotation.eulerAngles, direction, 0.5f);
            rotation.y = 0;
            rb.rotation = Quaternion.LookRotation(rotation, Vector3.up);
        }
        else if (Input.mousePresent)
        {
            Vector2 mouseRotationVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            direction = new(mouseRotationVector.x, 0, mouseRotationVector.y);
            Vector3 rotation = Vector3.Slerp(rb.rotation.eulerAngles, direction, 0.5f);
            rotation.y = 0;
            rb.rotation = Quaternion.LookRotation(rotation, Vector3.up);
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

            StartCoroutine(Dash());
            
        }
    }
    /// <summary>
    /// Coroutine called in the do dash function that sets timing status for the dash
    /// </summary>
    /// <returns>Various wait for seconds in between cooldowns</returns>
    IEnumerator Dash()
    { 
        moveStatus = MoveStatus.Dashing;
        abilityValue -= 10;
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
        GameObject projectile = Instantiate(rangedPrefab, rangedSpawnPoint.position, rangedSpawnPoint.rotation);
        projectile.GetComponent<Rigidbody>().velocity = rangedSpawnPoint.forward * rangedPrefabSpeed;
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
