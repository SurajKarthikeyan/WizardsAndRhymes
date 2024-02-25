using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Class that is responsible for controlling the player health and managing its abilities
/// </summary>
public class PlayerController : MonoBehaviour
{
    #region Variables
    [Tooltip("Singleton of player controller")]
    public static PlayerController instance;

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
    private float movementForce = 1f;

    [Tooltip("Maximum speed that the player is allowed to move generally")]
    [SerializeField]
    private float maxMoveSpeed = 5f;

    [Tooltip("Direction to apply the force of movement when moving")]
    private Vector3 forceDirection = Vector3.zero;

    [Header("Aiming Variables")]
    [Tooltip("LayerMask that is assigned for help in player aiming")]
    [SerializeField]
    private LayerMask lookLayerMask;

    [Tooltip("Constant used in helping player aim")]
    private const float aimAdjustmentConstant = 1.75f;

    [Header("Dashing Variables")]

    [Tooltip("Magnitude of appliedForce applied to player Rigidbody for dashing")]
    [SerializeField]
    private float dashForce = 3f;

    [Tooltip("Maximum speed that the player is allowed to move while dashing")]
    [SerializeField]
    private float maxDashSpeed = 10f;

    [Tooltip("Cooldown of the dash in seconds")]
    [SerializeField]
    private float dashTime = .5f;

    [Tooltip("Number in seconds of the cooldown in between dashes")]
    [SerializeField]
    private float dashCooldownThreshold = 2f;

    [Tooltip("Number in seconds of the cooldown in between dashes")]
    [SerializeField]
    private float allowedDashDistance = 10f;

    [Tooltip("LayerMask of the ground layer, used for dashing logic")]
    [SerializeField]
    private LayerMask groundLayerMask;

    [Tooltip("Model of the player displayed purely for dashing")]
    [SerializeField]
    private GameObject dashModel;

    [Tooltip("Timer that tracks how long it has been since last dash")]
    private float dashCooldownTimer;

    [Tooltip("Direction to apply the force of dashing when moving")]
    private Vector3 dashDirection = Vector3.zero;

    [Header("Camera")]
    [Tooltip("Reference to the camera focusing on the player")]
    [SerializeField]
    private Camera playerCamera;

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
    public float meleeDamage = 10f;

    [Tooltip("Time that the melee attack lasts for in seconds")]
    [SerializeField]
    private float meleeAttackDuration = 0.5f;

    [Tooltip("GameObject representing the hitbox of the melee attack")]
    [SerializeField]
    private GameObject meleeBox;

    [Header("UI Variables")]
    [Tooltip("Pause menu")]
    [SerializeField] private GameObject pauseMenu;

    [Tooltip("Pause menu active state")]
    [SerializeField] private bool isPaused;

    [Tooltip("Inventory menu")]
    [SerializeField] private GameObject inventoryMenu;

    [Tooltip("Inventory menu active state")]
    [SerializeField] private bool openInventory;

    [Header("General Attack/Combo Variables")]

    [Tooltip("Delay between attacks")]
    [SerializeField] private float attackDelayTimer;

    [Tooltip("How long should be allowed prior to restarting")]
    [SerializeField] private float mixtapeResetTimer;

    [Tooltip("Bool defining if the player can attack")]
    private bool canAttack;

    [Tooltip("Enumerator to reset mixtape combo")]
    private IEnumerator mixtapeResetRoutine;

    [Header("Script References")]
    [Tooltip("C# Class that handles all of the player abilities")]
    [SerializeField]
    private AbilityManager abilityManager;

    [Tooltip("Inventory for the player's mixtapes")]
    [SerializeField]
    private MixtapeInventory mixtapeInventory;

    [Tooltip("C# Class generated from the input action map")]
    [SerializeField]
    private PlayerHealth playerHealth;

    [Tooltip("C# Class generated from the input action map")]
    private PlayerInput playerInput;

    [Tooltip("Move input action from the PlayerInput action map")]
    private InputAction moveAction;

    [Tooltip("Input action used specifically for controllers to look around the player")]
    private InputAction gamepadLookAction;

    [Tooltip("Input action used specifically for controllers to look around the player")]
    private InputAction mouseLookAction;

    [Tooltip("Rigidbody of the player")]
    private Rigidbody rigidBody;

    [Tooltip("Direction in which the will attack if they choose to attack")]
    private Vector3 attackDirection;

    [Tooltip("Returns true if the player is currently moving")]
    private bool IsMoving => moveAction.ReadValue<Vector2>().sqrMagnitude > 0.1f;

    [Tooltip("Returns true if the mouse is over the game window, used for looking")]
    private bool MouseOverGameWindow
    {
        get
        {
            return !(0 > Input.mousePosition.x || 0 > Input.mousePosition.y ||
                Screen.width < Input.mousePosition.x || Screen.height < Input.mousePosition.y);
        }
    }


    public Gamepad gamepad;

    public Pointer pointer;

    //[Header("Test Variables")]
    //public GameObject testLight;
    #endregion

    #region Unity Methods
    /// <summary>
    /// Method called on scene startup
    /// </summary>
    private void Awake()
    {
        //Initialize singleton
        if (instance == null)
            instance = this;
        else
        {
            Debug.LogWarning("Duplicate player controller in scene: " + gameObject.name);
            Destroy(this);
        }
        dashModel.SetActive(false);
        rigidBody = GetComponent<Rigidbody>();
        playerInput = new PlayerInput();
        SetCanAttack(false);
    }

    /// <summary>
    /// Method called when this script is enabled
    /// </summary>
    private void OnEnable()
    {
        EnableUIControls();   
        EnablePlayerControls();
    }


    /// <summary>
    /// Method called when this script is disabled
    /// </summary>
    private void OnDisable()
    {
        DisableUIControls();
        DisablePlayerControls();
    }

    private void Update()
    {
        gamepad ??= Gamepad.current;
    }

    /// <summary>
    /// Function called once every frame, generally 60 frames per second
    /// </summary>
    private void FixedUpdate()
    { 

        dashCooldownTimer += Time.deltaTime;

        if (IsMoving)
        {
            //Adds different force to the rigidbody depending on if we are dashing or not
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
            //Reading the input given by the player and moving away from the camera
            forceDirection += moveAction.ReadValue<Vector2>().x * appliedForce * GetCameraRight(playerCamera);
            forceDirection += moveAction.ReadValue<Vector2>().y * appliedForce * GetCameraForward(playerCamera);

            //Adds the force and then we assume that the player is not inputting a direction
            rigidBody.AddForce(forceDirection, ForceMode.Impulse);
            dashDirection = forceDirection;
            forceDirection = Vector3.zero;

            Vector3 horizontalVelocity = rigidBody.velocity;
            horizontalVelocity.y = 0;

            //These checks are to clamp the velocity at the maximum move and dashing speed respectively
            if (horizontalVelocity.sqrMagnitude > maxDashSpeed * maxDashSpeed && moveStatus == MoveStatus.Dashing)
            {
                rigidBody.velocity = horizontalVelocity.normalized * maxDashSpeed + Vector3.up * rigidBody.velocity.y;
            }
            else if (horizontalVelocity.sqrMagnitude > maxMoveSpeed * maxMoveSpeed && moveStatus == MoveStatus.Moving)
            {
                rigidBody.velocity = horizontalVelocity.normalized * maxMoveSpeed + Vector3.up * rigidBody.velocity.y;
            }
        }

        //If we are not moving, we are assumed idle
        else
        {
            moveStatus = MoveStatus.Idle;
            rigidBody.velocity = Vector3.zero;
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
        Vector2 aim = gamepadLookAction.ReadValue<Vector2>();
        Vector3 direction = new(aim.x, 0, aim.y);

        /**
         * First check is to see if the player is using a controller.
         * If the player is using a mouse, we have to calculate the look direction differently
         */
        if (aim.magnitude > 0.2f && gamepad.IsActuated())
        {
            Cursor.visible = false;
            Vector3 rotation = Vector3.Slerp(rigidBody.rotation.eulerAngles, direction, 0.5f);
            rotation.y = 0;
            attackDirection = rotation;
            rigidBody.rotation = Quaternion.LookRotation(rotation, Vector3.up);
        }
        //This is if the player is using the mouse to look around
        else if (MouseOverGameWindow && mouseLookAction.ReadValue<Vector2>().magnitude > 0.1f)
        {
            Cursor.visible = true;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, lookLayerMask))
            {
                attackDirection = raycastHit.point - transform.position;
                float sinY = Mathf.Abs(Mathf.Sin(transform.rotation.eulerAngles.y * Mathf.Deg2Rad));
                attackDirection.z -= aimAdjustmentConstant * sinY;
                attackDirection.y = 0;
            }

            Quaternion lookRotation = Quaternion.LookRotation(attackDirection.normalized, Vector3.up);
            rigidBody.rotation = Quaternion.Slerp(rigidBody.rotation, lookRotation, 200 * Time.deltaTime);
        }
        //This is if neither are currently being used
        else
        {
            rigidBody.angularVelocity = Vector3.zero;
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
    /// Performs various actions for UI
    /// </summary>
    /// <param name="obj">Callback context of the button press</param>
    private void MenuSelect(InputAction.CallbackContext obj)
    {
        if (DialogueManager.instance.dialogueRunning)
        {
            DialogueManager.instance.DisplayNextSentence();
        }
    }

    /// <summary>
    /// Function that is called upon pressing any of the Ranged Attack inputs
    /// </summary>
    /// <param name="obj">Input callback context for the ranged attack</param>
    private void DoRanged(InputAction.CallbackContext obj)
    {
        if (canAttack)
        {
            SetCanAttack(false);
            StartCoroutine(AttackDelays(attackDelayTimer));
            ResetAttack();
            attackStatus = AttackStatus.Ranged;
            abilityManager.ResetAbilityRecharge();
            StartCoroutine(Projectile());
        }
    }
    /// <summary>
    /// Function that is called upon pressing any of the Melee Attack inputs
    /// </summary>
    /// <param name="obj">Input callback context for the melee attack</param>
    private void DoMelee(InputAction.CallbackContext obj)
    {
        if (canAttack)
        {
            SetCanAttack(false);
            StartCoroutine(AttackDelays(attackDelayTimer));
            ResetAttack();
            attackStatus = AttackStatus.Melee;
            abilityManager.ResetAbilityRecharge();
            StartCoroutine(Melee());
        }
    }

    /// <summary>
    /// Function to reset attack combo
    /// </summary>
    public void ResetAttack()
    {
        if (mixtapeResetRoutine != null)
        {
            StopCoroutine(mixtapeResetRoutine);
        }
        mixtapeResetRoutine = ResetMixtapeAttack(mixtapeResetTimer);
        StartCoroutine(mixtapeResetRoutine);
    }

    /// <summary>
    /// Function that determines whether or not the dash that the player 
    /// wants to do is valid. 
    /// </summary>
    /// <returns></returns>
    private bool IsValidDash()
    {
        //Puts a point out in front of the player, checks to see if it is above ground
        Vector3 dashCheck = transform.position + dashDirection.normalized * 
            allowedDashDistance + new Vector3(0, 1, 0);
        Ray dashCheckRay = new (dashCheck, Vector3.down);
        if (Physics.Raycast(dashCheckRay, float.MaxValue, groundLayerMask))
        {
            if (abilityManager.currentAbilityValue >= 10 && dashCooldownTimer >= dashCooldownThreshold)
            {
                return true;
            }
        }
        //If we do not hit any ground
        return false;
    }

    /// <summary>
    /// Function that is called upon pressing any of the Dash inputs
    /// </summary>
    /// <param name="obj">Input callback context for the dash</param>
    private void DoDash(InputAction.CallbackContext obj)
    {
        abilityManager.ResetAbilityRecharge();
        if (IsValidDash())
        {
            StartCoroutine(Dash());
        }
    }


    /// <summary>
    /// Function that is called when escape(KB) or start(Controller) is pressed
    /// </summary>
    /// <param name="obj"></param>
    private void PauseAction(InputAction.CallbackContext obj)
    {
        if (!openInventory)
        {
            Pause();
        }
    }
    
    /// <summary>
    /// Function that can be accessed to pause or unpause the game 
    /// </summary>
    public void Pause()
    {
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);
    }

    
    /// <summary>
    /// Reads input for player to open inventory
    /// </summary>
    /// <param name="obj"></param>
    private void InventoryAction(InputAction.CallbackContext obj)
    {
        if (!isPaused)
        {
            OpenInventory();
        }
    }

    /// <summary>
    /// Function that enables the players inputs 
    /// </summary>
    public void EnablePlayerControls()
    {
        playerInput.Player.Dash.started += DoDash;
        playerInput.Player.RangedAttack.started += DoRanged;
        playerInput.Player.MeleeAttack.started += DoMelee;
        moveAction = playerInput.Player.Movement;
        gamepadLookAction = playerInput.Player.GamepadLook;
        mouseLookAction = playerInput.Player.MouseLook;
        playerInput.Player.Enable();
    }

    /// <summary>
    /// Function that enables the inputs for all UI and menus
    /// </summary>
    public void EnableUIControls()
    {
        playerInput.UI.MenuSelect.started += MenuSelect;
        playerInput.UI.Exit.started += PauseAction;
        playerInput.UI.Inventory.started += InventoryAction;
        playerInput.UI.Enable();
    }

    /// <summary>
    /// Sets the boolean for if the player is able to attack
    /// </summary>
    /// <param name="attackPermission">Boolean that will set the canAttack boolean</param>
    public void SetCanAttack(bool attackPermission)
    {
        canAttack = attackPermission;
    }

    /// <summary>
    /// Function that disables the controls of the player
    /// </summary>
    public void DisablePlayerControls()
    {
        playerInput.Player.Dash.canceled -= DoDash;
        playerInput.Player.RangedAttack.canceled -= DoRanged;
        playerInput.Player.MeleeAttack.canceled -= DoMelee;
        playerInput.Player.Disable();
    }

    /// <summary>
    /// Disables all controls for UI and menus
    /// </summary>
    public void DisableUIControls()
    {
        playerInput.UI.MenuSelect.canceled -= MenuSelect;
        playerInput.UI.Exit.canceled -= PauseAction;
        playerInput.UI.Inventory.started -= InventoryAction;
        playerInput.UI.Disable();
    }

    /// <summary>
    /// Opens user inventory
    /// </summary>
    public void OpenInventory()
    {
        openInventory = !openInventory;
        inventoryMenu.SetActive(openInventory);
    }

    /// <summary>
    /// Coroutine that handles delays between attacks
    /// </summary>
    /// <param name="timer">How long the in between attack is in seconds</param>
    /// <returns>arious wait for seconds in between combos</returns>
    IEnumerator AttackDelays(float timer)
    {
        yield return new WaitForSeconds(timer);
        SetCanAttack(true);
    }
    
    /// <summary>
    /// Coroutine called when attacked to measure time between attacks
    /// </summary>
    /// <returns>Various wait for seconds in between combos</returns>
    IEnumerator ResetMixtapeAttack(float timer)
    {
        yield return new WaitForSeconds(timer);
        mixtapeInventory.ResetCombo();
    }
    
    /// <summary>
    /// Coroutine called in the do dash function that sets timing status for the dash
    /// </summary>
    /// <returns>Various wait for seconds in between cooldowns</returns>
    IEnumerator Dash()
    { 
        moveStatus = MoveStatus.Dashing;
        gameObject.layer = 10;
        dashModel.SetActive(true);
        rigidBody.useGravity = false;
        playerHealth.vulnerable = false;
        abilityManager.ReduceAbilityGuage(abilityManager.dashAbilityCost);
        dashCooldownTimer = 0;
        yield return new WaitForSeconds(dashTime);
        dashModel.SetActive(false);
        gameObject.layer = 3;
        playerHealth.vulnerable = true;
        rigidBody.useGravity = false;
        moveStatus = MoveStatus.Moving;
    }

    /// <summary>
    /// Coroutine called when the projectile function is called, handles cooldowns
    /// </summary>
    /// <returns>Various wait for seconds in between cooldowns</returns>
    IEnumerator Projectile()
    {
        abilityManager.ReduceAbilityGuage(abilityManager.rangedAbilityCost);
        //Instantiate projectile and give it the proper velocity
        GameObject projectile = Instantiate(rangedPrefab, rangedSpawnPoint.position, rangedSpawnPoint.rotation);
        projectile.GetComponent<Rigidbody>().velocity = rangedSpawnPoint.forward * rangedPrefabSpeed;
        projectile.GetComponent<Projectile>().dType = mixtapeInventory.damageType;
        abilityManager.ResetAbilityRecharge();
        yield return new WaitForSeconds(0.3f);
        attackStatus = AttackStatus.None;
        mixtapeInventory.OnTapeChange(true);
    }

    /// <summary>
    /// Coroutine called when the melee function is called, handles cooldowns
    /// </summary>
    /// <returns>Various wait for seconds in between cooldowns</returns>
    IEnumerator Melee()
    {
        //Sets the collider to be active and pushes player forward as if they're lunging
        meleeBox.SetActive(true);
        meleeBox.GetComponent<MeleeCollider>().damageType = mixtapeInventory.damageType;
        abilityManager.ReduceAbilityGuage(abilityManager.meleeAbilityCost);
        rigidBody.AddForce(attackDirection.normalized * 12, ForceMode.Impulse);
        DisablePlayerControls();
        abilityManager.ResetAbilityRecharge();
        mixtapeInventory.OnTapeChange(false);    // this here might be problematic but not too sure
        yield return new WaitForSeconds(meleeAttackDuration);
        meleeBox.SetActive(false);
        attackStatus = AttackStatus.None;
        EnablePlayerControls();
    }

    #endregion
    
}
