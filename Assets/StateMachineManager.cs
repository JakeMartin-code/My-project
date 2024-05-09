using UnityEngine;
using UnityEngine.InputSystem;

// Interface for states
public interface IGameState
{
    void EnterState();
    void UpdateState();
    void ExitState();
}

public class MainMenuState : IGameState
{
    private GameObject mainMenuCanvas;
    private StateMachineManager stateMachineManager;

    public MainMenuState(GameObject canvas, StateMachineManager manager)
    {
        mainMenuCanvas = canvas;
        stateMachineManager = manager;
    }

    public void EnterState()
    {
        mainMenuCanvas.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
     
    }

    public void UpdateState()
    {
 
    }

    public void ExitState()
    {
        mainMenuCanvas.SetActive(false);
    }
}


// Inventory State
public class InventoryState : IGameState
{
    private GameObject inventoryCanvas;

    public InventoryState(GameObject canvas)
    {
        inventoryCanvas = canvas;
    }

    public void EnterState()
    {
        inventoryCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
      
    }

    public void UpdateState()
    {
       
    }

    public void ExitState()
    {
        inventoryCanvas.SetActive(false);

    }
}

// Perk Tree State
public class PerkTreeState : IGameState
{
    private GameObject perkTreeCanvas;

    public PerkTreeState(GameObject canvas)
    {
        perkTreeCanvas = canvas;
    }

    public void EnterState()
    {
        perkTreeCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }

    public void UpdateState()
    {
    
    }

    public void ExitState()
    {
        perkTreeCanvas.SetActive(false);
      
    }
}

public class DeathState : IGameState
{
    private StateMachineManager stateMachineManager;

    public DeathState(StateMachineManager manager)
    {
        stateMachineManager = manager;
    }

    public void EnterState()
    {
        ResetGame();
        stateMachineManager.TransitionToState(new MainMenuState(stateMachineManager.mainMenuCanvas, stateMachineManager));
    }
    public void UpdateState()
    {
        
    }

    public void ExitState()
    {
        
    }

    private void ResetGame()
    {
        // Reset Player Stats
        PlayerStats playerStats = GameObject.FindObjectOfType<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.ResetPlayerStats();
            Debug.Log("reset stats deat state");
        }

        // Reset Perks
        PerkTreeManager perkManager = GameObject.FindObjectOfType<PerkTreeManager>();
        if (perkManager != null)
        {
            perkManager.ResetPerks();
            Debug.Log("reset player stats deat state");
        }

    
    }
}

// Gameplay State
public class GameplayState : IGameState
{
    private GameObject playerUICanvas;

    public GameplayState(GameObject canvas)
    {
        playerUICanvas = canvas;
    }

    public void EnterState()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        playerUICanvas.SetActive(true);
    }

    public void UpdateState()
    {

    }

    public void ExitState()
    {
       
    }

    public void ToggleState()
    {
      
        Debug.Log("Returning to Gameplay State");
        
    }
}

public class StateMachineManager : MonoBehaviour
{

    private IGameState currentState;
    public GameObject inventoryCanvas;
    public GameObject perkTreeCanvas;
    public GameObject mainMenuCanvas;
    public GameObject playerUICanvas;

    private NewControls controls;

    private void Awake()
    {
        controls = new NewControls();
        controls.Enable();
        controls.UI.OpenInventory.performed += ctx => ToggleToInventoryState();
        controls.UI.OpenPerkTree.performed += ctx => ToggleToPerkTreeState();
        EventsManager.instance.onPlayerDeath += ToggleDeathState;

    }

    private void Start()
    {
        TransitionToState(new MainMenuState(mainMenuCanvas, this));

    }

    private void OnDestroy()
    {

        EventsManager.instance.onPlayerDeath -= ToggleDeathState;
    }

    private void ToggleDeathState()
    {
 
        TransitionToState(new DeathState(this));
    }

    public void ToggleToGamePlayState()
    {
        TransitionToState(new GameplayState(playerUICanvas));
    }

    public void ToggleToInventoryState()
    {
        if (currentState is InventoryState)
        {
            Debug.Log("Closing Inventory State");
            currentState.ExitState();
            TransitionToState(new GameplayState(playerUICanvas)); 
        }
        else
        {
            TransitionToState(new InventoryState(inventoryCanvas));
        }
    }

    public void ToggleToPerkTreeState()
    {
        if (currentState is PerkTreeState)
        {
            Debug.Log("Closing Perk Tree State");
            currentState.ExitState(); 
            TransitionToState(new GameplayState(playerUICanvas)); 
        }
        else
        {
            TransitionToState(new PerkTreeState(perkTreeCanvas));
        }
    }

    public void ToggleToPerkTreeScene()
    {
        if (currentState is PerkTreeState)
        {
            Debug.Log("Closing Perk Tree State");
            currentState.ExitState(); 
            TransitionToState(new GameplayState(playerUICanvas)); 
        }
        else
        {
            TransitionToState(new PerkTreeState(perkTreeCanvas));
        }
    }

    public void ToggleToState(IGameState targetState)
    {
        if (currentState == targetState && !(currentState is GameplayState))
        {
            Debug.Log("Closing " + targetState.GetType().Name + " state");
            currentState.ExitState(); 
            TransitionToState(new GameplayState(playerUICanvas)); 
        }
        else
        {
            if (currentState != null && !(currentState is GameplayState))
            {
                currentState.ExitState(); 
            }
            TransitionToState(targetState);
        }
    }

    public void TransitionToState(IGameState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState();
        }

        currentState = newState;
        currentState.EnterState();


    }

    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; 
#endif
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateState();
        }
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
