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
    private PlayerInput m_PlayerInput;

    /// <summary>
    /// Move input action from the PlayerInput action map
    /// </summary>
    private InputAction m_MoveAction;

    /// <summary>
    /// Input action used specifically for controllers to m_LookAction around the player
    /// </summary>
    private InputAction m_LookAction;
    #endregion

    #region Movement Variables
    /// <summary>
    /// Rigidbody of the player
    /// </summary>
    private Rigidbody m_RigidBody;

    [Header("Movement Variables")]
    /// <summary>
    /// Magnitude of appliedForce applied to player Rigidbody for movement
    /// </summary>
    [SerializeField]
    private float m_MovementForce = 1f;

    /// <summary>
    /// Maximum speed that the player is allowed to m_MoveAction generally
    /// </summary>
    [SerializeField]
    private float m_MaxMoveSpeed = 5f;

    /// <summary>
    /// Direction to apply the appliedForce of movement when moving
    /// </summary>
    private Vector3 m_ForceDirection = Vector3.zero;

    [Header("Dashing Variables")]
    /// <summary>
    /// Magnitude of appliedForce applied to player Rigidbody for dashing
    /// </summary>
    [SerializeField]
    private float m_DashForce = 3f;

    /// <summary>
    /// Maximum speed that the player is allowed to m_MoveAction while dashing
    /// </summary>
    [SerializeField]
    private float m_MaxDashSpeed = 10f;

    /// <summary>
    /// Cooldown of the dash in seconds
    /// </summary>
    [SerializeField]
    private float m_DashTime = .5f;

    /// <summary>
    /// Number in seconds of the cooldown in between dashes
    /// </summary>
    [SerializeField]
    private float m_DashCooldownThreshold = 2f;

    /// <summary>
    /// Timer that tracks how long it has been since last dash
    /// </summary>
    private float m_DashCooldownTimer;
    #endregion

    #region Camera
    [Header("Camera")]
    /// <summary>
    /// Reference to the camera focusing on the player
    /// </summary>
    [SerializeField]
    private Camera m_PlayerCamera;
    #endregion


    public AbilityManager m_AbilityManager;

    #region Ranged Attack
    /// <summary>
    /// Prefab used as the player's ranged attack
    /// </summary>
    [Header("Ranged Attack Variables")]
    [SerializeField]
    private GameObject rangedPrefab;

    /// <summary>
    /// Transform to spawn the projectiles
    /// </summary>
    [SerializeField]
    private Transform rangedSpawnPoint;

    /// <summary>
    /// Speed the prefab is shot at
    /// </summary>
    [SerializeField]
    float rangedPrefabSpeed = 5f;
    #endregion 
    
    #endregion

    #region Properties
    /// <summary>
    /// Returns true if the player is able to dash
    /// </summary>
    public bool CanDash => m_AbilityManager.m_CurrentAbilityValue >= 10 && m_DashCooldownTimer >= m_DashCooldownThreshold;

    /// <summary>
    /// Returns true if the player is currently moving
    /// </summary>
    public bool IsMoving => m_MoveAction.ReadValue<Vector2>().sqrMagnitude > 0.1f;

    /// <summary>
    /// Returns true if the mouse is over the game window
    /// </summary>
    bool MouseOverGameWindow { get { return !(0 > Input.mousePosition.x || 0 > Input.mousePosition.y || 
                Screen.width < Input.mousePosition.x || Screen.height < Input.mousePosition.y); } }
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

    /// <summary>
    /// Enum that represents the current state of the player attacking
    /// </summary>
    private enum AttackStatus
    {
        None, 
        Ranged,
        Melee
    }

    private AttackStatus attackStatus;

    #endregion

    #region Methods

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

            m_ForceDirection += m_MoveAction.ReadValue<Vector2>().x * appliedForce * GetCameraRight(m_PlayerCamera);
            m_ForceDirection += m_MoveAction.ReadValue<Vector2>().y * appliedForce * GetCameraForward(m_PlayerCamera);

            m_RigidBody.AddForce(m_ForceDirection, ForceMode.Impulse);
            m_ForceDirection = Vector3.zero;

            Vector3 horizontalVelocity = m_RigidBody.velocity;
            horizontalVelocity.y = 0;
            if (horizontalVelocity.sqrMagnitude > m_MaxDashSpeed * m_MaxDashSpeed && moveStatus == MoveStatus.Dashing)
            {
                m_RigidBody.velocity = horizontalVelocity.normalized * m_MaxDashSpeed + Vector3.up * m_RigidBody.velocity.y;
            }
            else if (horizontalVelocity.sqrMagnitude > m_MaxMoveSpeed * m_MaxMoveSpeed && moveStatus == MoveStatus.Moving)
            {
                m_RigidBody.velocity = horizontalVelocity.normalized * m_MaxMoveSpeed + Vector3.up * m_RigidBody.velocity.y;
            }
        }
        else
        {
            moveStatus = MoveStatus.Idle;
        }

        Look();
    }
    #endregion

    #region Custom Methods
    /// <summary>
    /// Function that makes the player m_LookAction in the direction that it's moving
    /// </summary>
    private void Look()
    {
        Vector2 aim = m_LookAction.ReadValue<Vector2>();
        Vector3 direction = new(aim.x, 0, aim.y);
        if (aim.magnitude > 0.2f)
        {
            Cursor.visible = false;
            Vector3 rotation = Vector3.Slerp(m_RigidBody.rotation.eulerAngles, direction, 0.5f);
            rotation.y = 0;
            m_RigidBody.rotation = Quaternion.LookRotation(rotation, Vector3.up);
        }
        else if (MouseOverGameWindow)
        {
            Cursor.visible = true;
            Vector3 mouseRotationVector = Camera.main.ScreenToWorldPoint(Input.mousePosition) - m_RigidBody.position;
            direction = new(mouseRotationVector.x, 0, mouseRotationVector.y);
            m_RigidBody.rotation = Quaternion.LookRotation(direction, Vector3.up);
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
        m_AbilityManager.ReduceAbilityGuage(10);
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
        m_AbilityManager.ReduceAbilityGuage(2);
        GameObject projectile = Instantiate(rangedPrefab, rangedSpawnPoint.position, rangedSpawnPoint.rotation);
        projectile.GetComponent<Rigidbody>().velocity = rangedSpawnPoint.forward * rangedPrefabSpeed;
        m_AbilityManager.ResetAbilityRecharge();
        yield return new WaitForSeconds(0.3f);
        attackStatus = AttackStatus.None;
    }
    
    #endregion
    #endregion
}
