using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float accelerationTime = 0.1f; 
    public float decelerationTime = 0.2f; 
    private Vector3 moveVelocity;
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float jumpForce = 5f;

    //sliding
    public bool isSlideEnabled = false;
    public float slideSpeed = 8f; 
    public float slideDuration = 1f; 
    private bool isSprinting = false;
    public bool isSliding = false;

    //dashing
    public bool isDashingEnabled = false;
    public bool isDamageDashingEnabled = false;
    public bool isDashing = false;
    public int dashDamage = 5;
    public float dashSpeed = 5;
    public float dashDuration = 1;
    public float dashFOV;
    [SerializeField] private Collider dashDamageCollider;

    [Header("Camera Settings")]
    public Camera cam;
    public float sensitivity = 100f; 
    private float xRotation = 0f;

    public bool enableHeadBob = true;
    public float bobFrequency = 5f;
    public float bobHorizontalAmplitude = 0.1f;
    public float bobVerticalAmplitude = 0.1f;
    private float defaultYPos = 0f;
    private float timer = 0f;

    private PlayerInputs controls;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;
    private bool isGrounded = true;

    [Header("Player Settings")]
    public WeaponBehavior activeWeapon;
    public SkillTreeManager skillTree;
    public bool invisibilityCrouchPerkActive = false;
    public bool invisibilityDashPerkActive = false;



    public TextMeshProUGUI playerLevelText;
    public TextMeshProUGUI playerLevelTextSkillTree;
    public TextMeshProUGUI playerHealth;
    public TextMeshProUGUI invisibileText;
    public bool isInvisunlocked;
    public bool isInvisible = false;

    [Header("Crouch Settings")]
    public float crouchSpeed = 2.5f; 
    private bool isCrouching = false;
    private float originalHeight;
    public float crouchHeightMultiplier = 0.5f; 


    public static event Action OnInteractKeyPressed;

    void Awake()
    {
        controls = new PlayerInputs();
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        SetupControls();
    }

    void Start()
    {
        defaultYPos = cam.transform.localPosition.y; 


        invisibileText.SetText("you are visible");
        dashDamageCollider.gameObject.SetActive(false);
    
 
    }

    void SetupControls()
    {
        controls.PlayerInput.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.PlayerInput.Move.canceled += ctx => moveInput = Vector2.zero;
        controls.PlayerInput.Jump.performed += ctx => Jump();
        controls.PlayerInput.Sprint.performed += ctx => isSprinting = true;
        controls.PlayerInput.Sprint.canceled += ctx => isSprinting = false;
        controls.PlayerInput.Slide.performed += ctx => StartSlide();
        controls.PlayerInput.Dash.performed += ctx => StartDash();
        controls.PlayerInput.Look.performed += ctx => Look(ctx.ReadValue<Vector2>());
        controls.PlayerInput.Interact.performed += ctx => InteractKeyPressed();
        controls.PlayerInput.Crouch.performed += ctx => ToggleCrouch();
        controls.PlayerInput.Dash.performed -= ctx => StartDash();
    


    }

    private void InteractKeyPressed()
    {
        OnInteractKeyPressed?.Invoke();
    }




    void OnEnable()
    {
        controls.Enable();
 
    }

    void OnDisable()
    {
        controls.Disable();

    }

    void Update()
    {
        Look(lookInput);

        if (enableHeadBob && moveInput != Vector2.zero && isGrounded && !isSliding)
        {
            BobHead();
        }
    }

    void FixedUpdate()
    {
        if (!isSliding) 
        {
            Move();
        }
    }

    void Move()
    {
        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;
        if (isCrouching)
            currentSpeed = crouchSpeed; 

        Vector3 targetVelocity = (transform.right * moveInput.x + transform.forward * moveInput.y).normalized * currentSpeed;
        float currentAccelerationTime = moveInput == Vector2.zero ? decelerationTime : accelerationTime;
        rb.velocity = Vector3.SmoothDamp(rb.velocity, new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.z), ref moveVelocity, currentAccelerationTime);
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    void Jump()
    {
        if (IsGrounded() && !isSliding)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void ActivateSlideAbility(float speed, float duration)
    {
        slideSpeed = speed;
        slideDuration = duration;
        isSlideEnabled = true; 
    }

    public void ActivateDashAbility(float speed, float duration)
    {
        dashSpeed = speed;
        dashDuration = duration;
        isDashingEnabled = true; 
      
    }

    public void ActivateDamageDashAbility(int damage)
    {
        isDamageDashingEnabled = true;
        dashDamage = damage;
        dashDamageCollider.enabled = true;
        Debug.Log("damage dash unlocked " + isDamageDashingEnabled);
    }

    public void DeactivateDamageDashAbility(int damage)
    {
        isDamageDashingEnabled = false;
        dashDamageCollider.enabled = false;
        Debug.Log("damage dash disabled " + isDamageDashingEnabled);
    }

    public void ToggleCrouch()
    {
        isCrouching = !isCrouching;
        if (isCrouching)
        {
            originalHeight = capsuleCollider.height;
            capsuleCollider.height *= crouchHeightMultiplier;
            if (invisibilityCrouchPerkActive)
            {
                StartInvisibility(); 
            }
        }
        else
        {
            capsuleCollider.height = originalHeight;
            StopInvisibility(); 
        }
    }


    IEnumerator Slide()
    {
        isSliding = true;
        float originalHeight = capsuleCollider.height;
        capsuleCollider.height = originalHeight * crouchHeightMultiplier;

        Vector3 slideDirection = transform.forward * slideSpeed;
        float slideEndTime = Time.time + slideDuration;
        while (Time.time < slideEndTime)
        {
            rb.velocity = new Vector3(slideDirection.x, rb.velocity.y, slideDirection.z);
            yield return null;
        }

       
        isCrouching = true;
        capsuleCollider.height = originalHeight * crouchHeightMultiplier; 
        isSliding = false;
        isSprinting = false; 
    }

    IEnumerator Dash()
    {
        isDashing = true;
        int originalLayer = gameObject.layer;
        gameObject.layer = LayerMask.NameToLayer("PlayerNoEnemyCollision");

     
        Vector3 dashDirection = transform.forward * dashSpeed;
        rb.velocity = new Vector3(dashDirection.x, rb.velocity.y, dashDirection.z);

      
        float originalFOV = cam.fieldOfView;
        float targetFOV = originalFOV * dashFOV; 
        float dashStartTime = Time.time;

        dashDamageCollider.gameObject.SetActive(true);



        if(invisibilityDashPerkActive)
        {
            StartDashInvisibility(5);
        }

        while (Time.time < dashStartTime + dashDuration)
        {
            float elapsed = (Time.time - dashStartTime) / dashDuration;
            cam.fieldOfView = Mathf.Lerp(targetFOV, originalFOV, elapsed); 

         

            yield return null;
        }



        cam.fieldOfView = originalFOV;
        gameObject.layer = originalLayer;
        dashDamageCollider.gameObject.SetActive(false);
        isDashing = false;
        isSprinting = false;
    }


    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Triggered with: {other.name}");
        if (isDashing && other.CompareTag("Enemy"))
        {
            Debug.Log("Attempting to deal damage...");
            EnemyManager enemy = other.GetComponent<EnemyManager>();
            if (enemy != null)
            {
                enemy.TakeDamage(dashDamage, other.transform.position, WeaponPlaystyle.singleTarget);
                Debug.Log($"Dealt {dashDamage} damage to {other.name}");
            }
        }
    }

    void StartSlide()
    {
        if (!isSliding && IsGrounded() && isSprinting && isSlideEnabled && !isCrouching) 
        {
            StartCoroutine(Slide());
        }
    }

    void StartDash()
    {
        if (!isSliding && !isDashing && IsGrounded() && isSprinting && isDashingEnabled)
        {
            
                StartCoroutine(Dash());
            
            
        }
    }

    public void Look(Vector2 lookInput)
    {
        float mouseX = lookInput.x * sensitivity * Time.deltaTime;
        float mouseY = lookInput.y * sensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.transform.localEulerAngles = new Vector3(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void BobHead()
    {
        if (rb.velocity.magnitude > 0.1f)
        {
           
            timer += Time.deltaTime * bobFrequency;
            float horizontalOffset = Mathf.Sin(timer) * bobHorizontalAmplitude;
            float verticalOffset = Mathf.Cos(timer * 2) * bobVerticalAmplitude + defaultYPos;

       
            cam.transform.localPosition = new Vector3(horizontalOffset, verticalOffset, cam.transform.localPosition.z);
        }
        else
        {
            timer = 0;
            cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, new Vector3(0, defaultYPos, cam.transform.localPosition.z), Time.deltaTime * bobFrequency);
        }
    }


    public void IncreaseSpeed(float multiplier)
    { 
        sprintSpeed *= multiplier;
    }

    public void StartInvisibility()
    {
        if (!isInvisible)
        {
            isInvisible = true;
            invisibileText.SetText("you are invisible");
        
        }
    }

    public void StopInvisibility()
    {
        if (isInvisible)
        {
            isInvisible = false;
            invisibileText.SetText("you are visible");
       
        }
    }

    public void EnableCrouchInvisibilityPerk()
    {
        invisibilityCrouchPerkActive = true;
    }

    public void StartDashInvisibility(float duration)
    {
        if (isDashing) 
        {
            StartCoroutine(DashInvisibility(duration));
            Debug.Log("starting invis dash");
        }
    }

    private IEnumerator DashInvisibility(float duration)
    {
        StartInvisibility();
        yield return new WaitForSeconds(duration);
        StopInvisibility();
    }
}
