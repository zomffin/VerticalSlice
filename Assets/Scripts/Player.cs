using NUnit.Framework.Interfaces;
using UnityEngine;

enum PlayerState
{
    Moving, 
    Carrying,
    Typing
}

public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask _moveMask;
    [SerializeField] private LayerMask _interactMask; 
    [SerializeField] private Camera _camera; 
    
    private PlayerState _playerState;
    private float _deltaMove; // for move towards which isnt being used (yet)
    private Vector3 _mousePosition;

    public delegate void StartTyping();
    public event StartTyping typingEvent;
        
    // player starts moving, with the cursor confined to the window
    void Start()
    {
        _playerState = PlayerState.Moving;
        Cursor.visible = false; 
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        UpdateState();

        RunState(); 
        
    }
    
    // click to interact with objects 
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
                case "PickUp":
                    if (_playerState == PlayerState.Carrying)
                    {
                        _playerState = PlayerState.Moving;
                    }
                    else
                    {
                        _playerState = PlayerState.Carrying;
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
                carryingState(); 
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

    private void carryingState()
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
}
