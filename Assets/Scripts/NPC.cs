using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Random = UnityEngine.Random;

enum NPCState
{
    Getting,
    Returning, 
    Idle
}

public class NPC : MonoBehaviour
{

    [SerializeField] private Vector3 _startLocation;
    [SerializeField] private float _speed;
    [SerializeField] private float _checkDistance; 
    
    private Player _player;
    
    private NPCState _state;

    private List<GameObject> _resources;
    private Vector3 _target;
    private string _targetName;
    private GameObject _heldObject;
    //private bool _movingBack; 
    
    void Start()
    {
        _player = Locator.Instance.Player;
        //_state = NPCState.Idle;
        _state = NPCState.Getting;
    }

    void OnEnable()
    {
        Debug.Log("enabled, shgouldbe getting.....");
        _state = NPCState.Getting; 
        NewTarget(); 
    }

    void OnDisable()
    {
        //_state = NPCState.Idle;
        this.transform.position = _startLocation;
        _target = _startLocation;
        _heldObject = null;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("_state: " + _state);
        switch (_state)
        {
            case NPCState.Getting:
                gettingItem();
                break;
            case NPCState.Returning:
                movingBack();
                break;
            case NPCState.Idle:

                break;
            default:
                Debug.Log("something wrong with NPC state");
                break; 
        }
        
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == _targetName)
        {
            _target = _startLocation; 
            _state = NPCState.Returning;
        }
    }

    void movingBack()
    {
        if (!_heldObject)
        {
            NewTarget();
            _state = NPCState.Getting;
        }
        
        transform.position = Vector3.MoveTowards(transform.position, _target, _speed * Time.deltaTime);
        _heldObject.transform.position = transform.position;

        if (Vector3.Distance(transform.position, _target) < _checkDistance)
        {
            _state = NPCState.Getting;
            NewTarget(); 
        } 
    }

    void gettingItem()
    {
        if (_heldObject == null)
        {
            NewTarget(); 
        }
        
        _target = _heldObject.transform.position;
        transform.position = Vector3.MoveTowards(transform.position, _target, _speed * Time.deltaTime);
        
        
    }
        
    void NewTarget()
    {
        if (_resources.Count <= 0 || _resources == null)
        {
            _state = NPCState.Idle;
            return; 
        }
        
        _state = NPCState.Getting;
        int index = Random.Range(0, _resources.Count);
        int count = 0; 
        
        while (_resources[index] == null && count < 10)
        {
            _resources.RemoveAt(index); 
            index = Random.Range(0, _resources.Count);
            count++;

            if (_resources.Count <= 0)
            {
                _state = NPCState.Idle;
                return; 
            }
        }
        
        _target = _resources[index].transform.position;
        _targetName = _resources[index].name; 
        _heldObject = _resources[index];
    }

    public void PopulateList(List<GameObject> items)
    {
        Debug.Log("Populate list called");
        _resources = items; 
    }
}
