using NUnit.Framework.Interfaces;
using UnityEngine;

enum PlayerState
{
    Moving, 
    Typing
}

public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask _moveMask;
    [SerializeField] private LayerMask _interactMask; 
    [SerializeField] private Camera _camera; 
    
    private PlayerState _playerState;
    private float _deltaMove;
    private Vector3 _mousePosition; 
        
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerState = PlayerState.Moving;
        Cursor.visible = false; 
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();

        RunState(); 
        
        
        
    }

    void OnClick()
    {
        RaycastHit hit;
        if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hit, 100, _interactMask))
        {
            switch (hit.collider.gameObject.tag)
            {
                case "Type":
                    if (_playerState == PlayerState.Typing)
                    {
                        _playerState = PlayerState.Moving;
                    }
                    else
                    {
                        _playerState = PlayerState.Typing;
                    }
                    break;
                default:
                    _playerState = PlayerState.Moving;
                    Debug.Log("interactable with incorrect tag");
                    break; 
            }
        }
        else
        {
            Debug.Log("no interactable here"); 
        }

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
        if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hit, 100, _moveMask))
        {
            this.transform.position = hit.point;
        }
        else
        {
            
        }
    }


    private void typingState()
    {
        
    }
}
