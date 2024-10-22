using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Class that is responsible for controlling the player health and managing its abilities
/// </summary>
public class PlayerController : Singleton<PlayerController>
{
    #region Variables
    #region General Player Info
    [Tooltip("The player's tag")]
    public const string PlayerTag = "Player";

    [Tooltip("Enum that represents the movement state of the player")]
    private enum MoveStatus
    {
        Idle,
        Moving,
        Dashing,
    }

    [Tooltip("Enum representing the direction the player is moving in")]
    public enum MoveDirection
    {
        Idle,
        Down,
        Right,
        Left,
        Up
    }

    [Tooltip("MoveDirection instance")]
    [SerializeField] public MoveDirection moveDirection;

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
    #endregion

    #region Movement Variables
    [Header("Movement Variables")]

    [Tooltip("Magnitude of appliedForce applied to player Rigidbody for movement")]
    [SerializeField]
    private float movementForce = 1f;

    [Tooltip("Maximum speed that the player is allowed to move generally")]
    [SerializeField]
    private float maxMoveSpeed = 5f;

    [Tooltip("Direction to apply the force of movement when moving")]
    private Vector3 forceDirection = Vector3.zero;
    
    [Tooltip("Boolean stating whether or not the player is moving with full control or on a grid")]
    public bool gridBasedControl;
    
    [Tooltip("Collider used to check if this player is on the ground")]
    [SerializeField]
    private Collider groundCheck;
    #endregion

    #region Aiming Variables

    [Header("Aiming Variables")] [Tooltip("LayerMask that is assigned for help in player aiming")] [SerializeField]
    private LayerMask lookLayerMask;
    #endregion
    
    #region Dashing Variables
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

    [Tooltip("Default Material reference of the player")]
    [SerializeField] private Material defaultMaterial;
    [Tooltip("Dashing Material reference of the player")]
    [SerializeField] private Material dashMaterial;
    [Tooltip("Reference to renderer of player model(body)")]
    [SerializeField] private Renderer playerRenderer;

    [Tooltip("Timer that tracks how long it has been since last dash")]
    private float dashCooldownTimer;

    [Tooltip("Direction to apply the force of dashing when moving")]
    private Vector3 dashDirection = Vector3.zero;

    [Tooltip("Dash Sound Effect")]
    [SerializeField] private AK.Wwise.Event dashSoundEffect;

    [SerializeField] private AK.Wwise.Event dashFartSoundEffect;
    #endregion

    [Header("Camera")]
    [Tooltip("Reference to the camera focusing on the player")]
    [SerializeField]
    private Camera playerCamera;

    #region Attack Variables
    [Header("General Attack/Combo Variables")]
    [Tooltip("Damage type of the player for the entire level")]
    [SerializeField] private Health.DamageType playerLevelDamageType;
    [Tooltip("Audio Event for ranged attacks")]
    [SerializeField] private AK.Wwise.Event rangedEvent;
    [Tooltip("Audio Event for melee attacks")]
    [SerializeField] private AK.Wwise.Event meleeEvent;

    [SerializeField] private LayerMask knockbackLayerMask;
    
    [Tooltip("Number of successive attacks in a combo")]
    public int successiveAttacks;
    
    [Tooltip("Bool defining if the player can attack")]
    public bool canAttack;

    [Tooltip("Bool defining if an attack has been performed by the player")]
    private bool attackPerformed;
    
    [Tooltip("Bool defining if the player can attack")]
    private bool comboActive;

    [Tooltip("Time in which the player has to continue the combo before it resets")]
    private float comboContinuationTime = .8f;

    [Tooltip("Time taken to reset the combo")]
    private float oneHitComboResetTime = .2f;
    
    [Tooltip("Time taken to reset the combo")]
    private float twoHitComboResetTime = .4f;
    
    [Tooltip("Time taken to reset the combo")]
    private float threeHitComboResetTime = .6f;

    [Tooltip("Coroutine that handles the timing of the current attack")]
    private IEnumerator attackDelayCoroutine = null;


    [Tooltip("Coroutine that handles the timing of continuing the combo")]
    private IEnumerator comboContinuationCoroutine = null;

    [Tooltip("Coroutine that handles the timing of the combo cooldown")]
    private IEnumerator comboCoolDownCoroutine = null;
    
    #region Ranged Attack Variables
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
    
    [Tooltip("Time that the ranged attack lasts for in seconds")]
    private float rangedAttackDuration = 0.3f;
    #endregion

    #region Melee Attack Variables
    [Header("Melee Attack Variables")]
    [Tooltip("Damage inflicted by the player's melee attack")]
    public float meleeDamage = 10f;

    [Tooltip("Time that the melee attack lasts for in seconds")]
    private float meleeAttackDuration = 0.4f;

    [Tooltip("GameObject representing the hitbox of the melee attack")]
    [SerializeField]
    private GameObject meleeBox;
    #endregion
    #endregion

    #region UI Variables
    [Header("UI Variables")]
    [Tooltip("Pause menu")]
    [SerializeField] private GameObject pauseMenu;

    [Tooltip("Pause menu active state")]
    [SerializeField] private bool isPaused;
    #endregion
    
    [Header("Script References")]
    [Tooltip("Inventory for the player's mixtapes")]
    [SerializeField]
    private MixtapeInventory mixtapeInventory;

    [Tooltip("C# Class generated from the input action map")]
    [SerializeField]
    private PlayerHealth playerHealth;

    [Tooltip("C# Class generated from the input action map")]
    private PlayerInput playerInput;

    [Tooltip("Gamepad object used to detect controller input")]
    private Gamepad gamepad;

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

    [Tooltip("A set of sources locking the player's movement")]
    private HashSet<MonoBehaviour> movementLockSources = new HashSet<MonoBehaviour>();


    [Header("Animation")] 
    [Tooltip("Player Animator")]
    [SerializeField] private Animator playerAnimator;

    [Header("Interaction")] 
    [Tooltip("Boolean stating if the player is able to interact with something")]
    public bool canInteract;

    [Tooltip("Current interactable object that the player can interact with")]
    public IInteractable interactable;
    #endregion

    #region Unity Methods
    /// <summary>
    /// Method called on scene startup
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        rigidBody = GetComponent<Rigidbody>();
        playerInput = new PlayerInput();
        SetCanAttack(false);
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainIceArea" && !FlagManager.instance.GetFlag("enemyWave1Completed"))
        {
            DisablePlayerControls();
        }
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

    /// <summary>
    /// Unity function that is called as fast as is allowed
    /// </summary>
    private void Update()
    {
        //Assigns gamepad if it is null
        gamepad ??= Gamepad.current;
    }

    /// <summary>
    /// Function called once every frame, generally 60 frames per second
    /// </summary>
    private void FixedUpdate()
    {

        dashCooldownTimer += Time.deltaTime;

        if (IsMoving && !IsMovementLocked())
        {
            playerAnimator.SetBool("isRunning", true);
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

            //Calculates movement differently if we are moving in a grid-based fashion
            if (gridBasedControl)
            {
                Vector2 movementDirection = moveAction.ReadValue<Vector2>();
                if (Mathf.Abs(movementDirection.magnitude) > 0.001f)
                {
                    // we are moving in a direction
                    groundCheck.enabled = true;

                    if (Mathf.Abs(movementDirection.y) >= Mathf.Abs(movementDirection.x))
                    {
                        // We are moving up / down
                        moveDirection = movementDirection.y < 0 ? MoveDirection.Down : MoveDirection.Up;
                    }

                    else
                    {
                        // We are moving right / left
                        moveDirection = movementDirection.x < 0 ? MoveDirection.Left : MoveDirection.Right;
                    }
                }


                if(moveDirection == MoveDirection.Up || moveDirection == MoveDirection.Down)
                {
                    forceDirection += moveAction.ReadValue<Vector2>().y * appliedForce * Vector3.forward;
                }
                else if (moveDirection == MoveDirection.Right || moveDirection == MoveDirection.Left)
                {
                    forceDirection += moveAction.ReadValue<Vector2>().x * appliedForce * Vector3.right;
                }
                else
                {
                    forceDirection = Vector3.zero;
                }
            }
            else
            {
                //Reading the input given by the player and moving away from the camera
                forceDirection += moveAction.ReadValue<Vector2>().x * appliedForce * GetCameraRight(playerCamera);
                forceDirection += moveAction.ReadValue<Vector2>().y * appliedForce * GetCameraForward(playerCamera);
            }
            
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
            playerAnimator.SetBool("isRunning", false);
            moveStatus = MoveStatus.Idle;
            rigidBody.velocity = Vector3.zero;
            groundCheck.enabled = false;
            moveDirection = MoveDirection.Idle;
        }

        //Function that has the player look in the direction the player is inputting
        Look();
    }
    #endregion

    #region Custom Methods
    
    #region Looking Methods
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
    #endregion
    
    #region Input Handling Methods
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
        meleeBox.GetComponent<MeleeCollider>().enabled = false;
        if (canAttack && attackStatus == AttackStatus.None)
        {
            SetCanAttack(false);
            comboActive = true;
            attackPerformed = true;
            successiveAttacks++;
            attackStatus = AttackStatus.Ranged;
            StartCoroutine(Projectile());
        }
    }
    /// <summary>
    /// Function that is called upon pressing any of the Melee Attack inputs
    /// </summary>
    /// <param name="obj">Input callback context for the melee attack</param>
    private void DoMelee(InputAction.CallbackContext obj)
    {
        if (canAttack && attackStatus == AttackStatus.None)
        {
            SetCanAttack(false);
            successiveAttacks++;
            attackPerformed = true;
            comboActive = true;
            attackPerformed = true;
            attackStatus = AttackStatus.Melee;
            StartCoroutine(Melee());
        }
    }
    
    /// <summary>
    /// Function that is called upon pressing any of the Dash inputs
    /// </summary>
    /// <param name="obj">Input callback context for the dash</param>
    private void DoDash(InputAction.CallbackContext obj)
    {
        if (IsMovementLocked()) //Cannot dash with a movement lock in place
            return;
        
        if (IsValidDash())
        {
            int randInt = Random.Range(0, 100);
            if (randInt == 99)
            {
                dashFartSoundEffect.Post(this.gameObject);

            }
            else
            {
                dashSoundEffect.Post(this.gameObject);

            }
            StartCoroutine(Dash());
        }
    }

    /// <summary>
    /// Function that is called upon pressing any of the Interact inputs
    /// </summary>
    /// <param name="obj">Input callback context for the dash</param>
    private void DoInteract(InputAction.CallbackContext obj)
    {
        if (canInteract && interactable != null)
        {
            interactable.Interact();
        }
    }

    /// <summary>
    /// Handles interacting when the player's main controls are disabled
    /// </summary>
    /// <param name="obj"></param>
    private void UIInteract(InputAction.CallbackContext obj)
    {
        if (canInteract && interactable != null)
        {
            interactable.Interact();
        }
    }
    
    /// <summary>
    /// Function that is called when escape(KB) or start(Controller) is pressed
    /// </summary>
    /// <param name="obj"></param>
    private void PauseAction(InputAction.CallbackContext obj)
    {
        Pause();
    }
    #endregion
    
    #region Player Control Permissions
    /// <summary>
    /// Function that enables the players inputs 
    /// </summary>
    public void EnablePlayerControls()
    {
        playerInput.Player.Dash.started += DoDash;
        playerInput.Player.RangedAttack.started += DoRanged;
        playerInput.Player.MeleeAttack.started += DoMelee;
        playerInput.Player.Interact.started += DoInteract;
        moveAction = playerInput.Player.Movement;
        gamepadLookAction = playerInput.Player.GamepadLook;
        mouseLookAction = playerInput.Player.MouseLook;
        playerInput.Player.Enable();
    }

    /// <summary>
    /// Function that enables the inputs for all UI and menus
    /// </summary>
    private void EnableUIControls()
    {
        playerInput.UI.MenuSelect.started += MenuSelect;
        playerInput.UI.Exit.started += PauseAction;
        playerInput.UI.Interact.started += UIInteract;
        playerInput.UI.Enable();
    }

    /// <summary>
    /// Function that enables all player attack controls
    /// </summary>
    public void EnablePlayerAttackControls()
    {
        playerInput.Player.MeleeAttack.Enable();
        playerInput.Player.RangedAttack.Enable();
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
        playerInput.Player.Interact.canceled -= DoInteract;
        playerInput.Player.Disable();
    }

    /// <summary>
    /// Disables all controls for UI and menus
    /// </summary>
    private void DisableUIControls()
    {
        playerInput.UI.MenuSelect.canceled -= MenuSelect;
        playerInput.UI.Exit.canceled -= PauseAction;
        playerInput.UI.Interact.canceled -= UIInteract;
        playerInput.UI.Disable();
    }

    /// <summary>
    /// Function that disables all attack controls
    /// </summary>
    public void DisablePlayerAttackControls()
    {
        playerInput.Player.MeleeAttack.Disable();
        playerInput.Player.RangedAttack.Disable();
    }
    #endregion
    
    #region Attack Combo Methods
    /// <summary>
    /// Coroutine that handles delay of attacks
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns></returns>
    IEnumerator AttackDelay(float seconds)
    {
        if (successiveAttacks >= 3)
        {
            yield return new WaitForSeconds(seconds);
            playerAnimator.SetBool("FuckOutaShooting", true);
            attackDelayCoroutine = null;
            comboContinuationCoroutine = null;
            ResetCombo();
        }
        
        else
        {
            yield return new WaitForSeconds(seconds);
            attackStatus = AttackStatus.None;
            attackPerformed = false;
            SetCanAttack(true);
            if (comboContinuationCoroutine == null)
            {
                comboContinuationCoroutine = ComboContinueDelay(comboContinuationTime);
                StartCoroutine(comboContinuationCoroutine);
            }
        }
        attackDelayCoroutine = null;
    }

    /// <summary>
    /// Coroutine that handles listening for input 
    /// </summary>
    /// <param name="seconds">Seconds to listen to input</param>
    /// <returns></returns>
    IEnumerator ComboContinueDelay(float seconds)
    {
        if (comboActive)
        {
            yield return new WaitForSeconds(seconds);
            if (!attackPerformed)
            {
                playerAnimator.SetBool("FuckOutaShooting", true);
                comboContinuationCoroutine = null;
                ResetCombo();
            }
        }
        comboContinuationCoroutine = null;
    }

    /// <summary>
    /// Coroutine that handles the cooldown of the combo system
    /// </summary>
    /// <param name="seconds">Seconds that the cooldown would last</param>
    /// <returns></returns>
    private IEnumerator ComboCooldown(float seconds)
    {
        successiveAttacks = 0;
        SetCanAttack(false);
        yield return new WaitForSeconds(seconds);
        comboActive = false;
        attackPerformed = false;
        SetCanAttack(true);
        playerAnimator.SetBool("FuckOutaShooting", false);
        comboCoolDownCoroutine = null;
    }
    
    /// <summary>
    /// Function to reset attack combo
    /// </summary>
    private void ResetCombo()
    {
        float comboResetTime = successiveAttacks switch
        {
            1 => oneHitComboResetTime,
            2 => twoHitComboResetTime,
            3 => threeHitComboResetTime,
            _ => 0
        };
        if (comboCoolDownCoroutine == null)
        {
            attackStatus = AttackStatus.None;
            comboCoolDownCoroutine = ComboCooldown(comboResetTime);
            StartCoroutine(comboCoolDownCoroutine);
        }
    }
    #endregion
    
    #region Player Action Methods
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
            if (dashCooldownTimer >= dashCooldownThreshold)
            {
                return true;
            }
        }
        //If we do not hit any ground
        return false;
    }


    /// <summary>
    /// Function that can be accessed to pause or unpause the game 
    /// </summary>
    public void Pause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0;
            AkSoundEngine.SetState("PauseMenu", "InPauseMenu");

        }
        else
        {
            Time.timeScale = 1;
            AkSoundEngine.SetState("PauseMenu", "NotInPauseMenu");

        }
        pauseMenu.SetActive(isPaused);
    }
    
    /// <summary>
    /// Coroutine called in the do dash function that sets timing status for the dash
    /// </summary>
    /// <returns>Various wait for seconds in between cooldowns</returns>
    IEnumerator Dash()
    { 
        moveStatus = MoveStatus.Dashing;
        gameObject.layer = 10;
        playerRenderer.material = dashMaterial;
        rigidBody.useGravity = false;
        playerHealth.vulnerable = false;
        dashCooldownTimer = 0;
        yield return new WaitForSeconds(dashTime);
        playerRenderer.material = defaultMaterial;
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
        if (attackDelayCoroutine != null)
        {
            StopCoroutine(attackDelayCoroutine);
            attackDelayCoroutine = null;
        }
        playerAnimator.SetTrigger("rangedAttack");
        rangedEvent.Post(this.gameObject);
        yield return new WaitForSeconds(0.05f);
        //Instantiate projectile and give it the proper velocity
        GameObject projectile = Instantiate(rangedPrefab, rangedSpawnPoint.position, rangedSpawnPoint.rotation);
        projectile.GetComponent<Rigidbody>().velocity = rangedSpawnPoint.forward * rangedPrefabSpeed;
        projectile.GetComponent<Projectile>().DType = playerLevelDamageType;
        attackDelayCoroutine = AttackDelay(rangedAttackDuration);
        StartCoroutine(attackDelayCoroutine);
        yield return null;
    }

    /// <summary>
    /// Coroutine called when the melee function is called, handles cooldowns
    /// </summary>
    /// <returns>Various wait for seconds in between cooldowns</returns>
    IEnumerator Melee()
    {
        if (attackDelayCoroutine != null)
        {
            StopCoroutine(attackDelayCoroutine);
            attackDelayCoroutine = null;
        }
        playerAnimator.SetTrigger("meleeAttack");
        meleeEvent.Post(this.gameObject);
        meleeBox.GetComponent<MeleeCollider>().damageType = playerLevelDamageType;
        rigidBody.AddForce(attackDirection.normalized * 12, ForceMode.Impulse);
        DisablePlayerControls();
        attackDelayCoroutine = AttackDelay(meleeAttackDuration);
        StartCoroutine(attackDelayCoroutine);
        EnablePlayerControls();
        yield return null;
    }
    
    ///// <summary>
    ///// Function that knocks the player back in a certain direction
    ///// </summary>
    ///// <param name="direction">Direction the player is knocked back</param>
    ///// <param name="magnitude">Magnitude the player is knocked back</param>
    //public void Knockback(Vector3 direction, float magnitude)
    //{
    //    rigidBody.velocity = Vector3.zero;
    //    // Does the ray intersect any objects excluding the player layer
    //    if (Physics.Raycast(transform.position, direction, out RaycastHit hit, 7, knockbackLayerMask))
    //    {
    //        float distance = Vector3.Distance(hit.transform.position, transform.position);
    //        if (distance >= 1)
    //        {
    //            rigidBody.AddForce(direction.normalized * magnitude / (distance), ForceMode.Acceleration);
    //        }
    //    }
    //    else
    //    {
    //        rigidBody.AddForce(direction.normalized * magnitude, ForceMode.Impulse);
    //    }
    //}
    #endregion
    
    #region MovementLocks

    
    /// <summary>
    /// Register a source that is preventing the player from moving. The player's rigidbody is set to kinematic while movement is locked
    /// </summary>
    /// <param name="lockSource">The source to add</param>
    public void AddMovementLock(MonoBehaviour lockSource)
    {
        movementLockSources.Add(lockSource);

        rigidBody.isKinematic = true;
    }

    /// <summary>
    /// Removes a source that is preventing the player from moving. Resets the player's rigidbody to no longer be kinematic if the last lock is removed
    /// </summary>
    /// <param name="lockSource">The source to remove</param>
    public void RemoveMovementLock(MonoBehaviour lockSource)
    {
        movementLockSources.Remove(lockSource);

        if (!IsMovementLocked())
            rigidBody.isKinematic=false;
    }

    /// <summary>
    /// Returns whether any movement locks are currently on the player
    /// </summary>
    /// <returns>Whether any movement locks are currently on the player</returns>
    private bool IsMovementLocked()
    {
        return movementLockSources.Count > 0;
    }

    #endregion
    
    #endregion
    
}
