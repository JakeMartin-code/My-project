using UnityEngine;
using UnityEngine.InputSystem;

// Interface for states
public interface IGameState
{
    void EnterState();
    void UpdateState();
    void ExitState();
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
        // Additional initialization specific to inventory state
    }

    public void UpdateState()
    {
        // Update logic specific to inventory state
    }

    public void ExitState()
    {
        inventoryCanvas.SetActive(false);
        // Cleanup or save state-specific data if needed
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
        // Additional initialization specific to perk tree state
    }

    public void UpdateState()
    {
        // Update logic specific to perk tree state
    }

    public void ExitState()
    {
        perkTreeCanvas.SetActive(false);
        // Cleanup or save state-specific data if needed
    }
}

// Gameplay State
public class GameplayState : IGameState
{
    public void EnterState()
    {
        // Enter gameplay state logic
    }

    public void UpdateState()
    {
        // Update logic specific to gameplay state
    }

    public void ExitState()
    {
        // Exit gameplay state logic
    }

    public void ToggleState()
    {
        // Implement any specific logic needed upon toggling back to gameplay state
        Debug.Log("Returning to Gameplay State");
        // Perform any necessary actions such as deactivating UI elements, resetting variables, etc.
    }
}

public class StateMachineManager : MonoBehaviour
{
 
        private IGameState currentState;
        public GameObject inventoryCanvas;
        public GameObject perkTreeCanvas;

        private NewControls controls;

        private void Awake()
        {
            controls = new NewControls();
            controls.Enable();

            // Assign callbacks for input actions
            controls.UI.OpenInventory.performed += ctx => ToggleToInventoryState();
            controls.UI.OpenPerkTree.performed += ctx => ToggleToPerkTreeState();
            // Add more input actions for other menus or states as needed
        }

        private void Start()
        {
            TransitionToState(new GameplayState()); // Set initial state to gameplay
        }

        public void ToggleToInventoryState()
        {
            if (currentState is InventoryState)
            {
                Debug.Log("Closing Inventory State");
                currentState.ExitState(); // Close the inventory state if pressed again
                TransitionToState(new GameplayState()); // Return to the GameplayState
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
                currentState.ExitState(); // Close the perk tree state if pressed again
                TransitionToState(new GameplayState()); // Return to the GameplayState
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
            currentState.ExitState(); // Close the non-Gameplay state if pressed again
            TransitionToState(new GameplayState()); // Return to the GameplayState
        }
        else
        {
            if (currentState != null && !(currentState is GameplayState))
            {
                currentState.ExitState(); // Exit the current state before transitioning to the new state
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

        Debug.Log("Current State: " + currentState.GetType().Name); // Print current state's name to the console
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
