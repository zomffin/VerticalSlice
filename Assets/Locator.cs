using UnityEngine;

public class Locator : MonoBehaviour
{
    private static Locator _instance;
    public static Locator instance {get {return _instance;}}

    [SerializeField] private Player _player
    {
        get { return _player; }
    }
    
    
    
    void OnAwake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject); 
        }
        else
        {
            _instance = this; 
        }
        
        
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
