using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public EnemyManager enemyManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Find or assign references to other managers and game objects
        enemyManager = Object.FindFirstObjectByType<EnemyManager>();

      
    }
}
