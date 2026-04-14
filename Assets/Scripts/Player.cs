using NUnit.Framework.Interfaces;
using UnityEngine;

enum PlayerState
{
    Moving, 
    Typing
}

public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Camera _camera; 
    
    private PlayerState _playerState;
    private float _deltaMove;
    private Vector3 _mousePosition; 
        
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerState = PlayerState.Moving;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();

        RunState(); 
        
    }

    private void UpdateState()
    {
        
    }

    private void RunState()
    {
        switch (_playerState)
        {
            case PlayerState.Moving:
                movingState();
                break;
            case PlayerState.Typing:
                
                break;
            default:
                Debug.Log("error in run state");
                break; 
        }
    }

    private void movingState()
    {
        RaycastHit hit;
        if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hit, 100, _layerMask))
        {
            this.transform.position = hit.point;
            //this.transform.position = new Vector3(transform.position.x, transform.position.z, transform.position.y + 1); 

            //Debug.Log(hit.point);
        }
        else
        {
            
        }
    }


    private void typingState()
    {
        
    }
}
