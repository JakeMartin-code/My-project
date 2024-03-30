using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float lookSensitivity = 5f;  // Increased sensitivity for snappier movement

    private Vector2 moveInput;
    private Rigidbody rb;
    private bool isGrounded;
    private NewControls controls;


    public WeaponBehavior activeWeapon;
    public FPSCamera1 fPSCamera1;
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

    private PlayerLook look;

    public Slider xpBarSlider;
    public Slider healthBar;
    private float xpUpdateSpeed = 0.5f; 
    // Reference to the XP bar slider in the UI
    //public TextMeshProUGUI xpText; // Text to display current XP and XP required for next level



    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controls = new NewControls();

        // Movement Controls
        controls.PlayerControlls.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.PlayerControlls.Move.canceled += _ => moveInput = Vector2.zero;
        controls.PlayerControlls.Jump.performed += _ => Jump();


        // Camera Controls
        //controls.PlayerControlls.Look.performed += ctx => Look(ctx.ReadValue<Vector2>());
        //

        look = GetComponent<PlayerLook>();

        //Player ability controls
        controls.PlayerControlls.Invisibility.performed += ctx => GoInvisible();

        //weapon controlls
        controls.WeaponControlls.Fire.performed += _ => Fire();
        controls.WeaponControlls.Reload.performed += _ => Reload();
        controls.WeaponControlls.SwitchWeapon.performed += _ => SwitchWeapon();
        playerLevelText.SetText("level " + playerLevel.ToString());
        playerLevelTextSkillTree.SetText("Level " + playerLevel.ToString());

        Cursor.lockState = CursorLockMode.Locked;
       
    }



    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Start()
    {
   
    
        currentHealth = maxHealth;
        playerHealth.SetText("Health " + currentHealth.ToString());
        invisibileText.SetText("you are visible");
        GetFirstWeapon();
        UpdateXPBar();
        UpdateHealthBar();


    }

   private void GetFirstWeapon()
    {
        activeWeapon = weaponManager.GetEquippedWeapon();
    }

    private void Update()
    {
        Move();
        ClassAbility();
    }

    private void Move()
    {
        // Move the player based on the input direction
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y) * moveSpeed * Time.deltaTime;
        transform.Translate(moveDirection, Space.Self);
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void ClassAbility()
    {
        // Your class ability code here
    }

    private void LateUpdate()
    {
        look.Look(controls.PlayerControlls.Look.ReadValue<Vector2>());
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
        bool next = true; // Set to true for switching to the next weapon, false for the previous
        if (weaponManager != null)
        {
            weaponManager.SwitchWeapon(next);
            activeWeapon = weaponManager.GetEquippedWeapon(); // Update activeWeapon
        }
     
    }

    public void IncreaseSpeed(float multiplier)
    {
     
        moveSpeed *= multiplier;
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
        if(isInvisunlocked)
        {
            if(isInvisible == false)
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
            // Handle player death (e.g., respawn logic or game over)
            Debug.Log("Player died!");
        }
    }

    public void GainXP(int amount)
    {
        currentXP += amount;
       
        Debug.Log("XP Required for Next Level: " + CalculateXPToNextLevel());
        UpdateXPBar();
        while (currentXP >= CalculateXPToNextLevel())
        {
            LevelUp();
        }
    }


    int CalculateXPToNextLevel()
    {
        // Use a formula to calculate the XP required for the next level
        // Adjust the formula to fit your progression curve
        return Mathf.FloorToInt(Mathf.Pow(playerLevel, xpMultiplier) * 100);
    }

    void LevelUp()
    {
        // Increase the player's level and update any relevant stats
        playerLevel++;
        currentXP -= xpToNextLevel; // Subtract excess XP
        skillTree.skillPoints++;
        skillTree.skillPointText.SetText("skill point " + skillTree.skillPoints.ToString());

        playerLevelText.SetText("level " + playerLevel.ToString());
        playerLevelTextSkillTree.SetText("level " + playerLevel.ToString());
        UpdateXPBar();
        // You can add additional logic here for updating player stats, abilities, etc.
    }

    void UpdateXPBar()
    {
        // Update the XP bar slider value based on current XP and XP required for next level
        xpToNextLevel = CalculateXPToNextLevel();
        xpBarSlider.maxValue = xpToNextLevel;
        xpBarSlider.value = Mathf.Max(currentXP, 0);
        //xpText.text = "Level " + playerLevel + " | XP: " + currentXP + " / " + xpToNextLevel;
    }

    void UpdateHealthBar()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }


    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }


}
