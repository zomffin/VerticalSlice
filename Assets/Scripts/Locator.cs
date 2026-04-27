using Unity.VisualScripting;
using UnityEngine;


public class Locator : MonoBehaviour
{
    public static Locator Instance { get; private set; }
    public Player Player { get; private set; }
    
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this; 
        
        GameObject playerObj = GameObject.FindWithTag("Player");
        Player = playerObj.GetComponent<Player>();
        
        DontDestroyOnLoad(this.gameObject);

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
