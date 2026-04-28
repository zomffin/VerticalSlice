using NUnit.Framework.Interfaces;
using Unity.VisualScripting;
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
    private GameObject _gameManager;

    private Transform _heldItem; 

    public delegate void StartTyping(bool isTyping);
    public event StartTyping typingEvent;
        
    // player starts moving, with the cursor confined to the window
    void Start()
    {
        _playerState = PlayerState.Moving;
        //Cursor.visible = false; 
        Cursor.lockState = CursorLockMode.Confined;

        _gameManager = Locator.Instance.gameObject; 
    }

    void Update()
    {
        //UpdateState(); currently does nothing

        RunState(); 
        
    }
    
    // click to interact with objects 
    void OnClick()
    {
        if (_playerState == PlayerState.Typing)
        {
            _playerState = PlayerState.Moving;
            typingEvent?.Invoke(false);
            CustomEvent.Trigger(_gameManager, "typing", false);
            Debug.Log("went from typing to moving");
            return;
        }
        
        RaycastHit hit;
        if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hit, 100, _interactMask))
        {
            switch (hit.collider.gameObject.tag)
            {
                case "Type":
                    /*if (_playerState == PlayerState.Typing)
                    {
                        _playerState = PlayerState.Moving;
                        typingEvent?.Invoke(false);
                        CustomEvent.Trigger(_gameManager, "typing", false);
                        Debug.Log("went from typing to moving");
                    }
                    else
                    {*/
                        _playerState = PlayerState.Typing;
                        typingEvent?.Invoke(true);
                        CustomEvent.Trigger(_gameManager, "typing", true);
                        Debug.Log("went from moving to typing");
                    //}
                    break;
                case "PickUp":
                    if (_playerState == PlayerState.Carrying)
                    {
                        _playerState = PlayerState.Moving;
                    }
                    else
                    {
                        _playerState = PlayerState.Carrying;
                        _heldItem = hit.collider.gameObject.transform; 
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

    public void FinishTyping()
    {
        _playerState = PlayerState.Moving;
        typingEvent?.Invoke(false);
        CustomEvent.Trigger(_gameManager, "typing", false);
        Debug.Log("went from typing to moving");
    }

    public void TakeItem()
    {
        if (_playerState == PlayerState.Carrying)
        {
            _playerState = PlayerState.Moving;
            _heldItem = null;
        }
        else
        {
            Debug.Log("Take item triggered when player isnt carrying");
        }
    }

    private void UpdateState()
    {
        return;
    }

    private void RunState()
    {
        switch (_playerState)
        {
            case PlayerState.Moving:
                movingState();
                break;
            case PlayerState.Typing:
                typingState(); 
                break;
            case PlayerState.Carrying:
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
        /*string typed = Input.inputString;
        Debug.Log(typed); */
    }

    private void carryingState()
    {
        RaycastHit hit;
        if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hit, 100, _moveMask))
        {
            this.transform.position = hit.point;
            _heldItem.position = new Vector3(hit.point.x, hit.point.y + 0.5f, hit.point.z);
        }
        else
        {
            
        }
    }
}
