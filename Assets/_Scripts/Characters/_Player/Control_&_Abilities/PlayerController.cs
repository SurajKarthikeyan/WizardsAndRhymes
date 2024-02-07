using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Class that is responsible for controlling the player health and managing its abilities
/// 
/// Author: Zane O'Dell
/// </summary>
public class PlayerController : MonoBehaviour
{
    #region Variables

    [Tooltip("Enum that represents the movement state of the player")]
    private enum MoveStatus
    {
        Idle,
        Moving,
        Dashing
    }

    [Tooltip("MoveStatus instance")]
    private MoveStatus moveStatus;

    [Tooltip("Enum that represents the current state of the player attacking")]
    private enum AttackStatus
    {
        None,
        Ranged,
        Melee
    }

    [Tooltip("AttackStatus instance")]
    private AttackStatus attackStatus;

    [Header("Movement Variables")]

    [Tooltip("Magnitude of appliedForce applied to player Rigidbody for movement")]
    [SerializeField]
    private float m_MovementForce = 1f;

    [Tooltip("Maximum speed that the player is allowed to move generally")]
    [SerializeField]
    private float m_MaxMoveSpeed = 5f;

    [Tooltip("Direction to apply the force of movement when moving")]
    private Vector3 m_ForceDirection = Vector3.zero;


    [Header("Dashing Variables")]

    [Tooltip("Magnitude of appliedForce applied to player Rigidbody for dashing")]
    [SerializeField]
    private float m_DashForce = 3f;

    [Tooltip("Maximum speed that the player is allowed to move while dashing")]
    [SerializeField]
    private float m_MaxDashSpeed = 10f;

    [Tooltip("Cooldown of the dash in seconds")]
    [SerializeField]
    private float m_DashTime = .5f;

    [Tooltip("Number in seconds of the cooldown in between dashes")]
    [SerializeField]
    private float m_DashCooldownThreshold = 2f;

    
    [Header("Camera")]
    [Tooltip("Reference to the camera focusing on the player")]
    [SerializeField]
    private Camera m_PlayerCamera;

    [Header("Ranged Attack Variables")]

    [Tooltip("Prefab used as the player's ranged attack")]
    [SerializeField]
    private GameObject rangedPrefab;

    [Tooltip("Transform to spawn the projectiles")]
    [SerializeField]
    private Transform rangedSpawnPoint;

    [Tooltip("Speed the prefab is shot at")]
    [SerializeField]
    private float rangedPrefabSpeed = 5f;

    [Header("Melee Attack Variables")]
    [Tooltip("Damage inflicted by the player's melee attack")]
    [SerializeField]
    public float meleeDamage = 10f;

    [Tooltip("Damage inflicted by the player's melee attack")]
    [SerializeField]
    private GameObject meleeBox;

    [Tooltip("Timer that tracks how long it has been since last dash")]
    private float m_DashCooldownTimer;

    [Header("Script References")]
    [SerializeField]
    private AbilityManager m_AbilityManager;

    [Tooltip("C# Class generated from the input action map")]
    private PlayerInput m_PlayerInput;

    [Tooltip("Move input action from the PlayerInput action map")]
    private InputAction m_MoveAction;

    [Tooltip("Input action used specifically for controllers to look around the player")]
    private InputAction m_LookAction;

    [Tooltip("Rigidbody of the player")]
    private Rigidbody m_RigidBody;

    [Tooltip("Direction in which the will attack if they choose to attack")]
    private Vector3 attackDirection;

    [Tooltip("Returns true if the player is able to dash")]
    private bool CanDash => m_AbilityManager.currentAbilityValue >= 10 && m_DashCooldownTimer >= m_DashCooldownThreshold;

    [Tooltip("Returns true if the player is currently moving")]
    private bool IsMoving => m_MoveAction.ReadValue<Vector2>().sqrMagnitude > 0.1f;

    [Tooltip("Returns true if the mouse is over the game window, used for looking")]
    private bool MouseOverGameWindow
    {
        get
        {
            return !(0 > Input.mousePosition.x || 0 > Input.mousePosition.y ||
                Screen.width < Input.mousePosition.x || Screen.height < Input.mousePosition.y);
        }
    }
    #endregion

    #region Unity Methods
    /// <summary>
    /// Method called on scene startup
    /// </summary>
    private void Awake()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_PlayerInput = new PlayerInput();
    }

    /// <summary>
    /// Method called when this script is enabled
    /// </summary>
    private void OnEnable()
    {
        m_PlayerInput.Player.Dash.started += DoDash;
        m_PlayerInput.Player.RangedAttack.started += DoRanged;
        m_PlayerInput.Player.MeleeAttack.started += DoMelee;
        m_MoveAction = m_PlayerInput.Player.Movement;
        m_LookAction = m_PlayerInput.Player.Look;
        m_PlayerInput.Player.Enable();
    }


    /// <summary>
    /// Method called when this script is disabled
    /// </summary>
    private void OnDisable()
    {
        m_PlayerInput.Player.Dash.canceled -= DoDash;
        m_PlayerInput.Player.RangedAttack.canceled -= DoRanged;
        m_PlayerInput.Player.MeleeAttack.canceled -= DoMelee;
        m_PlayerInput.Player.Disable();
    }



    /// <summary>
    /// Function called once every frame, generally 60 frames per second
    /// </summary>
    private void FixedUpdate()
    {
        m_DashCooldownTimer += Time.deltaTime;

        if (IsMoving)
        {
            //Adds different force to the rigidbody depending on if we are dashing or not
            float appliedForce;
            if (moveStatus == MoveStatus.Dashing)
            {
                appliedForce = m_DashForce;
            }
            else
            {
                appliedForce = m_MovementForce;
                moveStatus = MoveStatus.Moving;
            }
            //Reading the input given by the player and moving away from the camera
            m_ForceDirection += m_MoveAction.ReadValue<Vector2>().x * appliedForce * GetCameraRight(m_PlayerCamera);
            m_ForceDirection += m_MoveAction.ReadValue<Vector2>().y * appliedForce * GetCameraForward(m_PlayerCamera);

            //Adds the force and then we assume that the player is not inputting a direction
            m_RigidBody.AddForce(m_ForceDirection, ForceMode.Impulse);
            m_ForceDirection = Vector3.zero;

            Vector3 horizontalVelocity = m_RigidBody.velocity;
            horizontalVelocity.y = 0;

            //These checks are to clamp the velocity at the maximum move and dashing speed respectively
            if (horizontalVelocity.sqrMagnitude > m_MaxDashSpeed * m_MaxDashSpeed && moveStatus == MoveStatus.Dashing)
            {
                m_RigidBody.velocity = horizontalVelocity.normalized * m_MaxDashSpeed + Vector3.up * m_RigidBody.velocity.y;
            }
            else if (horizontalVelocity.sqrMagnitude > m_MaxMoveSpeed * m_MaxMoveSpeed && moveStatus == MoveStatus.Moving)
            {
                m_RigidBody.velocity = horizontalVelocity.normalized * m_MaxMoveSpeed + Vector3.up * m_RigidBody.velocity.y;
            }
        }

        //If we are not moving, we are assumed idle
        else
        {
            moveStatus = MoveStatus.Idle;
        }

        //Function that has the player look in the direction the player is inputting
        Look();
    }
    #endregion

    #region Custom Methods
    /// <summary>
    /// Function that makes the player look in the direction that the player inputs
    /// </summary>
    private void Look()
    {
        //Gets the position of the player's look input
        Vector2 aim = m_LookAction.ReadValue<Vector2>();
        Vector3 direction = new(aim.x, 0, aim.y);
        /**
         * First check is to see if the player is using a controller.
         * If the player is using a mouse, we have to calculate the look direction differently
         */
        if (aim.magnitude > 0.2f)
        {
            Cursor.visible = false;
            Vector3 rotation = Vector3.Slerp(m_RigidBody.rotation.eulerAngles, direction, 0.5f);
            rotation.y = 0;
            attackDirection = rotation;
            m_RigidBody.rotation = Quaternion.LookRotation(rotation, Vector3.up);
        }
        //This is if the player is using the mouse to look around
        else if (MouseOverGameWindow)
        {
            Cursor.visible = true;
            Vector3 mouseRotationVector = Camera.main.ScreenToWorldPoint(Input.mousePosition) - m_RigidBody.position;
            direction = new(mouseRotationVector.x, 0, mouseRotationVector.y);
            attackDirection = direction;
            m_RigidBody.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
        //This is if neither are currently being used
        else
        {
            m_RigidBody.angularVelocity = Vector3.zero;
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
        attackStatus = AttackStatus.Ranged;
        m_AbilityManager.ResetAbilityRecharge();
        StartCoroutine(Projectile());
    }
    /// <summary>
    /// Function that is called upon pressing any of the Melee Attack inputs
    /// </summary>
    /// <param name="obj">Input callback context for the melee attack</param>
    private void DoMelee(InputAction.CallbackContext obj)
    {
        attackStatus = AttackStatus.Melee;
        m_AbilityManager.ResetAbilityRecharge();
        StartCoroutine(Melee());
    }

    /// <summary>
    /// Function that is called upon pressing any of the Dash inputs
    /// </summary>
    /// <param name="obj">Input callback context for the dash</param>
    private void DoDash(InputAction.CallbackContext obj)
    {
        m_AbilityManager.ResetAbilityRecharge();
        if (CanDash)
        {
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
        m_AbilityManager.ReduceAbilityGuage(m_AbilityManager.dashAbilityCost);
        m_DashCooldownTimer = 0;
        yield return new WaitForSeconds(m_DashTime);
        moveStatus = MoveStatus.Moving;
    }

    /// <summary>
    /// Coroutine called when the projectile function is called, handles cooldowns
    /// </summary>
    /// <returns>Various wait for seconds in between cooldowns</returns>
    IEnumerator Projectile()
    {
        m_AbilityManager.ReduceAbilityGuage(m_AbilityManager.rangedAbilityCost);
        //Instantiate projectile and give it the proper velocity
        GameObject projectile = Instantiate(rangedPrefab, rangedSpawnPoint.position, rangedSpawnPoint.rotation);
        projectile.GetComponent<Rigidbody>().velocity = rangedSpawnPoint.forward * rangedPrefabSpeed;
        m_AbilityManager.ResetAbilityRecharge();
        yield return new WaitForSeconds(0.3f);
        attackStatus = AttackStatus.None;
    }

    /// <summary>
    /// Coroutine called when the melee function is called, handles cooldowns
    /// </summary>
    /// <returns>Various wait for seconds in between cooldowns</returns>
    IEnumerator Melee()
    {
        //Sets the collider to be active and pushes player forward as if they're lunging
        meleeBox.SetActive(true);
        m_AbilityManager.ReduceAbilityGuage(m_AbilityManager.meleeAbilityCost);
        m_RigidBody.AddForce(attackDirection.normalized * 12, ForceMode.Impulse);
        m_AbilityManager.ResetAbilityRecharge();
        yield return new WaitForSeconds(0.5f);
        meleeBox.SetActive(false);
        attackStatus = AttackStatus.None;
    }
    
    #endregion
}
