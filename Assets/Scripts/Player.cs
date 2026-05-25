using System.Collections.Generic;
using NUnit.Framework.Interfaces;
using TMPro;
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
    [SerializeField] private TextMeshProUGUI _playerStatus;
    
    [SerializeField] private LayerMask _moveMask;
    [SerializeField] private LayerMask _interactMask; 
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _typingCam; 

    [Header("Animation Stuff")]
    [SerializeField] private List<Texture2D> _idling;
    [SerializeField] private List<Texture2D> _carrying;
    [SerializeField] private List<Texture2D> _typing;
    [SerializeField] private Renderer _renderer;
    [Range(0,24)][SerializeField] private int _fps; 
    private float currentIndex;
    private List<Texture2D> _currentAnimation; 
    
    
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

        _currentAnimation = _idling; 
    }

    void Update()
    {
        //UpdateState(); currently does nothing
        
        updateStateUI();
        
        RunState(); 
        
        //animation stuff 
        Animate();
        
    }
    
    // click to interact with objects 
    void OnClick()
    {
        if (_playerState == PlayerState.Typing)
        {
            _playerState = PlayerState.Moving;
            typingEvent?.Invoke(false);
            _typingCam.SetActive(false);
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
                        _playerState = PlayerState.Typing;
                        typingEvent?.Invoke(true);
                        _typingCam.SetActive(true);
                        CustomEvent.Trigger(_gameManager, "typing", true);
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
        _typingCam.SetActive(false);
        CustomEvent.Trigger(_gameManager, "typing", false);
    }

    public void TakeItem(GameObject item)
    {
        if (_playerState != PlayerState.Carrying || item != _heldItem.gameObject)
        {
            return;
        }
        
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

    public void TakeItem()
    {
        if (_playerState == PlayerState.Carrying)
        {
            _playerState = PlayerState.Moving;
            _heldItem = null;
        }
        else
        {
            Debug.Log("Take item triggered when player isnt carrying (overloaded method)");
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

    private void updateStateUI()
    {
        switch (_playerState)
        {
            case PlayerState.Moving:
                _playerStatus.text = "You are moving"; 
                break;
            case PlayerState.Typing:
                _playerStatus.text = "You are typing";
                break;
            case PlayerState.Carrying:
                _playerStatus.text = "You are carrying";
                break;
            default:
                _playerStatus.text = "You are... doing something not accounted for!";
                break;
        }
        
        
    }

    private void Animate()
    {
        currentIndex += _fps * Time.deltaTime;

        var i = (int)currentIndex;

        if (i > _currentAnimation.Count - 1)
        {
            currentIndex = 0;
            i = 0;
        }

        // _BaseMap for URP Lit  _MainTex for built in RP
        _renderer.material.SetTexture("_BaseMap", _currentAnimation[i]);
    }
}
