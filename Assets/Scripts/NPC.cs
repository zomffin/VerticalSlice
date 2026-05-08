using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Random = UnityEngine.Random;

public class NPC : MonoBehaviour
{

    [SerializeField] private Vector3 _startLocation;
    [SerializeField] private float _speed;
    [SerializeField] private float _checkDistance; 
    
    private Player _player;

    private List<GameObject> _resources;
    private Vector3 _target;
    private string _targetName;
    private GameObject _heldObject;
    private bool _movingBack; 
    
    void Start()
    {
        _player = Locator.Instance.Player;

    }

    void OnEnable()
    {
        NewTarget(); 
    }

    void OnDisable()
    {
        this.transform.position = _startLocation;
        _target = _startLocation;
        _heldObject = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (_movingBack)
        {
            movingBack();
        }
        else
        {
            gettingItem(); 
        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == _targetName)
        {
            _target = _startLocation; 
            _movingBack = true;
        }
        else
        {
            
        }
    }

    void movingBack()
    {
        if (!_heldObject)
        {
            NewTarget();
            _movingBack = false; 
        }
        
        transform.position = Vector3.MoveTowards(transform.position, _target, _speed * Time.deltaTime);
        _heldObject.transform.position = transform.position;

        if (Vector3.Distance(transform.position, _target) < _checkDistance)
        {
            _movingBack = false;
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
        if (_resources.Count == 0 || _resources == null)
        {
            return; 
        }
        
        _movingBack = false; 
        int index = Random.Range(0, _resources.Count);
        int count = 0; 
        while (_resources[index] == null && count < 10)
        {
            _resources.RemoveAt(index); 
            index = Random.Range(0, _resources.Count);
            count++;
        }
        _target = _resources[index].transform.position;
        _targetName = _resources[index].name; 
        _heldObject = _resources[index];
    }

    public void PopulateList(List<GameObject> items)
    {
        _resources = items; 
    }
}
