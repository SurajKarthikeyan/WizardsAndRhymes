using System;
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

    private PlayerInput playerInput;
    private InputAction move;

    private Rigidbody rb;
    [SerializeField]
    private float movementForce = 1f;
    [SerializeField]
    private float maxDashSpeed = 10f;
    [SerializeField]
    private float maxMoveSpeed = 5f;
    private Vector3 forceDirection = Vector3.zero;

    [SerializeField]
    private Camera playerCamera;

    private bool canDash;

    public float dashCooldown = 0.5f;

    public GameObject abilitySliderGO;

    private Slider abilitySlider;

    private Vector3 uiOffsetVec = new Vector3(-0.1f, -0.45f, -1.25f);

    public GameObject sliderFill;
    public enum MoveStatus
    {
        Moving,
        Dashing
    }

    private MoveStatus moveStatus;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = new PlayerInput();
        canDash = true;
        abilitySlider = abilitySliderGO.GetComponent<Slider>();
    }

    private void OnEnable()
    {
        playerInput.Player.Dash.started += DoDash;
        move = playerInput.Player.Movement;
        playerInput.Player.Enable();
    }

    private void OnDisable()
    {
        playerInput.Player.Dash.started -= DoDash;
        playerInput.Player.Disable();
    }

    private void FixedUpdate()
    {
        abilitySliderGO.transform.position = transform.position + uiOffsetVec;

        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    abilitySlider.value -= 2;
        //}

        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    abilitySlider.value += 2;
        //}

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

    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }

    private void DoDash(InputAction.CallbackContext obj)
    {
        if (canDash)
        {
            print("Starting dash");
            moveStatus = MoveStatus.Dashing;
            rb.AddForce(forceDirection, ForceMode.Impulse);
            StartCoroutine(Dash());
        }
    }
    IEnumerator Dash()
    {
        yield return new WaitForSeconds(0.5f);
        moveStatus = MoveStatus.Moving;
        print("Ending dash");
        yield return new WaitForSeconds(0.5f);
        canDash = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
