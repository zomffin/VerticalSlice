using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _deltaMove;

    private Vector3 _mousePosition; 
        
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _mousePosition = Input.mousePosition;
        _mousePosition.z = 1; 
        
        Vector3.MoveTowards(this.transform.position, _mousePosition, _deltaMove);
    }
}
