using System;
using System.Transactions;
using TMPro;
using UnityEditorInternal;
using UnityEngine;

public class Typing : MonoBehaviour
{
    [Header("Paper stuff")]
    [SerializeField] private TextMeshPro _paper;
    [SerializeField] private Transform _position;
    
    [Header("Resource stuff")]
    [SerializeField] private int _startingInk;
    [SerializeField] private int _startingDelete;
    
    [Header("UI stuff")]
    [SerializeField] private TextMeshProUGUI inkUI;
    [SerializeField] private TextMeshProUGUI deleteUI;

    private int _currInk; 
    private int _currDelete;
    
    private string _taskPassage; 
    private string _currentPassage = "";

    private int _place;

    private Player _player; 
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player = Locator.Instance.Player; 
        
        _currInk = _startingInk;
        _currDelete = _startingDelete;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.inputString.Length > 0)
        {
            
            foreach (char c in Input.inputString)
            {
                if (c == '\b') // has backspace/delete been pressed?
                {
                    if (_currentPassage.Length != 0 && _currDelete > 0)
                    {
                        _currentPassage = _currentPassage.Substring(0, _currentPassage.Length - 1);
                        _paper.text = _currentPassage;
                        
                        _currDelete--; 
                        deleteUI.text = "Whiteout: " + _currDelete;
                    }
                }
                else if ((c == '\n') || (c == '\r')) // enter/return
                {
                    Debug.Log("player hit enter");
                }
                else if (_currInk > 0)
                {
                    _currentPassage += c; 
                    _paper.text = _currentPassage;

                    _currInk--;
                    inkUI.text = "Ink: " + _currInk; 
                }
            }    
        }
        
    }
    
    public void SetTask(string task)
    {
        _taskPassage = task;
        Debug.Log("task was set to:" + _taskPassage);
    }

    //everyhing in game dev is hard and im crying 
    
    /*private void OnTriggerEnter(Collider other)
    {
        Debug.Log("on trigger enter triggered");
        
        if (other.name.Contains("Paper"))
        {
            _paper = other.GetComponentInChildren<TextMeshPro>(); 
            _currentPassage =  _paper.text;
            other.transform.position = _position.position;
            other.transform.rotation = _position.rotation;
            //other.GetComponent<Rigidbody>().isKinematic = false;
            other.GetComponent<Rigidbody>(). = false; 
            _player.TakeItem();
        }
        else
        {
            Debug.Log("hit with a collider that isnt paper");
        }
    }*/

    /*private void OnTriggerExit(Collider other)
    {
        Debug.Log("On trigger exit triggered");
        
        if (other.name.Contains("Paper"))
        {
            _paper = null;
            _currentPassage = "";
            //other.GetComponent<Rigidbody>().isKinematic = true;
        }
    }*/
}
