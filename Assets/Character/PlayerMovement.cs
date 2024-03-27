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
    public float accelerationTime = 0.1f; // Time it takes to reach full speed
    public float decelerationTime = 0.2f; // Time it takes to stop from full speed
    private Vector3 moveVelocity;
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float jumpForce = 5f;
    public float slideSpeed = 8f; 
    public float slideDuration = 1f; 
    private bool isSprinting = false;
    private bool isSliding = false;

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
    [SerializeField] private WeaponManager weaponManager;
    public int maxHealth = 100;
    public int currentHealth;
    public int currentXP = 0;
    public int playerLevel = 1;
    public TextMeshProUGUI playerLevelText;
    public TextMeshProUGUI playerLevelTextSkillTree;
    public TextMeshProUGUI playerHealth;
    public TextMeshProUGUI invisibileText;
    public bool isInvisunlocked;
    public bool isInvisible = false;



    public float xpMultiplier = 1.2f;
    public int perkPoints = 0;
    public int xpToNextLevel;



    public Slider xpBarSlider;
    public Slider healthBar;
    private float xpUpdateSpeed = 0.5f;

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
        defaultYPos = cam.transform.localPosition.y; // Initialize default Y position for head bob

        currentHealth = maxHealth;
        playerHealth.SetText("Health " + currentHealth.ToString());
        invisibileText.SetText("you are visible");
        GetFirstWeapon();
        UpdateXPBar();
        UpdateHealthBar();
    }

    void SetupControls()
    {
        controls.PlayerInput.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.PlayerInput.Move.canceled += ctx => moveInput = Vector2.zero;
        controls.PlayerInput.Jump.performed += ctx => Jump();
        controls.PlayerInput.Sprint.performed += ctx => isSprinting = true;
        controls.PlayerInput.Sprint.canceled += ctx => isSprinting = false;
        controls.PlayerInput.Slide.performed += ctx => StartSlide();
        controls.PlayerInput.Look.performed += ctx => Look(ctx.ReadValue<Vector2>());
        controls.PlayerInput.Interact.performed += ctx => InteractKeyPressed();
        //weapon controlls
        controls.WeaponControlls.Fire.performed += _ => Fire();
        controls.WeaponControlls.Reload.performed += _ => Reload();
        controls.WeaponControlls.SwitchWeapon.performed += _ => SwitchWeapon();
        playerLevelText.SetText("level " + playerLevel.ToString());
        playerLevelTextSkillTree.SetText("Level " + playerLevel.ToString());

       // Cursor.lockState = CursorLockMode.Locked;
    }

    private void InteractKeyPressed()
    {
        OnInteractKeyPressed?.Invoke();
    }

    private void GetFirstWeapon()
    {
        activeWeapon = weaponManager.GetEquippedWeapon();
    }



    void OnEnable()
    {
        controls.Enable();
        EventsManager.instance.onXPGained += GainXP;
    }

    void OnDisable()
    {
        controls.Disable();
        EventsManager.instance.onXPGained -= GainXP;
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
        Vector3 targetVelocity = (transform.right * moveInput.x + transform.forward * moveInput.y).normalized * (isSprinting ? sprintSpeed : walkSpeed);
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

    IEnumerator Slide()
    {
        isSliding = true;
        float originalHeight = capsuleCollider.height;
        capsuleCollider.height = originalHeight / 2; 

        float slideEndTime = Time.time + slideDuration;
        while (Time.time < slideEndTime)
        {
            Vector3 slideDirection = transform.forward * slideSpeed;
            rb.velocity = new Vector3(slideDirection.x, rb.velocity.y, slideDirection.z);
            yield return null;
        }

        capsuleCollider.height = originalHeight; 
        isSliding = false;
        isSprinting = false; 
    }

    void StartSlide()
    {
        if (!isSliding && IsGrounded() && isSprinting) 
        {
            StartCoroutine(Slide());
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




    private void Fire()
    {
        if (activeWeapon != null)
        {
            activeWeapon.Fire();
        }
    }

    private void Reload()
    {
        if (activeWeapon != null & activeWeapon.isReloading == false)
        {
            activeWeapon.Reload();

        }
    }


    private void SwitchWeapon()
    {
        bool next = true; 
        if (weaponManager != null)
        {
            weaponManager.SwitchWeapon(next);
            activeWeapon = weaponManager.GetEquippedWeapon(); 
        }
        else
        {
            Debug.LogError("WeaponManager reference is null! Unable to switch weapons.");
        }
    }

    public void IncreaseSpeed(float multiplier)
    {
      
        Debug.Log("this increase speed function is called ");
        sprintSpeed *= multiplier;
    }

    public void Bunkerdown(int health)
    {
        currentHealth += health;
        playerHealth.SetText("Health " + currentHealth.ToString());
    }

    public void UnlockInvisibility()
    {
        isInvisunlocked = true;
    }

    public void GoInvisible()
    {
        if (isInvisunlocked)
        {
            if (isInvisible == false)
            {
                isInvisible = true;
                invisibileText.SetText("you are invisible");

            }
            else
            {
                isInvisible = false;
                invisibileText.SetText("you are visible");
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthBar();
        //playerHealth.SetText("Health " + playerHealth.ToString());
        if (currentHealth <= 0)
        {
          
            Debug.Log("Player died!");
        }
    }

    public void GainXP(int amount)
    {
        currentXP += amount;
        Debug.Log("Player gained " + amount + " XP!");
        Debug.Log("XP Required for Next Level: " + CalculateXPToNextLevel());
        UpdateXPBar();
        while (currentXP >= CalculateXPToNextLevel())
        {
            LevelUp();
        }
    }


    int CalculateXPToNextLevel()
    {
        
        return Mathf.FloorToInt(Mathf.Pow(playerLevel, xpMultiplier) * 100);
    }

    void LevelUp()
    {
      
        playerLevel++;
        currentXP -= xpToNextLevel; 
        skillTree.skillPoints++;
        skillTree.skillPointText.SetText("skill point " + skillTree.skillPoints.ToString());

        playerLevelText.SetText("level " + playerLevel.ToString());
        playerLevelTextSkillTree.SetText("level " + playerLevel.ToString());
        UpdateXPBar();
        EventsManager.instance.LevelUp(playerLevel);

    }

    void UpdateXPBar()
    {
       
        xpToNextLevel = CalculateXPToNextLevel();
        xpBarSlider.maxValue = xpToNextLevel;
        xpBarSlider.value = Mathf.Max(currentXP, 0);
   
    }

    void UpdateHealthBar()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

}
