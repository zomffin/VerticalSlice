using Unity.VisualScripting;
using UnityEngine;

public class Locator : MonoBehaviour
{
    private static Locator _instance;
    public static Locator instance {get {return _instance;}}

    [SerializeField] public static Player _player;
    [SerializeField] public static GameObject gameManager;
    
    
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject); 
        }
        else
        {
            _instance = this; 
        }

        gameManager = this.gameObject; 
    }

    public static Player GetPlayer()
    {
        return _player; 
    }

    public static GameObject GetGameManager()
    {
        return gameManager;
    }

    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
