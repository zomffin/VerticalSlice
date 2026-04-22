using System;
using System.Transactions;
using TMPro;
using Unity.VisualScripting;
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
    [SerializeField] private int _startingPaper; 
    
    [Header("UI stuff")]
    [SerializeField] private TextMeshProUGUI inkUI;
    [SerializeField] private TextMeshProUGUI deleteUI;

    private int _currInk; 
    private int _currDelete;
    private int _currPaper; 
    
    private string _taskPassage;
    [SerializeField] private string _currentPassage = "";

    private int _place;

    private Player _player;
    private GameObject _gameManager; 
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player = Locator.Instance.Player;
        _gameManager = Locator.Instance.gameObject; 
        
        _currInk = _startingInk;
        _currDelete = _startingDelete;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            EjectPaper(); 
            _player.FinishTyping();
            --_currPaper; 
        }
        
        if (Input.inputString.Length > 0)
        {
            
            foreach (char c in Input.inputString)
            {
                if (c == '\b') // has backspace/delete been pressed?
                {
                    if (_currentPassage.Length != 0 && _currDelete > 0)
                    {
                        if (_currentPassage[_currentPassage.Length - 1] == '>')
                        {
                            DeleteCode(false);
                            DeleteCode(true);
                            _paper.text = _currentPassage; 
                        }
                        else
                        {
                            _currentPassage = _currentPassage.Substring(0, _currentPassage.Length - 1);
                            _paper.text = _currentPassage; 
                        }

                        _place--; 
                        _currDelete--; 
                    }
                }
                else if ((c == '\n') || (c == '\r')) // enter/return
                {
                    Debug.Log("player hit enter");
                }
                else if (_currInk > 0)
                {

                    if (_place < _taskPassage.Length && _taskPassage[_place] == c)
                    {
                        _currentPassage += c; 
                        _paper.text = _currentPassage;
                        _place++; 
                    }
                    else
                    {
                        _currentPassage += "<color=red>" + c + "</color>";
                        _paper.text = _currentPassage;
                        _place++; 
                    }
                    
                    _currInk--;
                }
            }  
            
            SetUI();
        }
        
        
    }
    
    public void SetTask(string task)
    {
        _taskPassage = task;
        _place = 0;
        Debug.Log("task was set to:" + _taskPassage);
    }
    

    private void SetUI()
    {
        inkUI.text = "Ink: " + _currInk; 
        deleteUI.text = "Whiteout: " + _currDelete;
    }

    public void AddInk(int num)
    {
        _currInk += num;
    }

    public void AddWhiteOut(int num)
    {
        _currDelete += num;
    }

    public void AddPaper(int num)
    {
        _currPaper += num;
    }

    private void EjectPaper()
    {
        CustomEvent.Trigger(_gameManager, "finishTask", _currentPassage, _taskPassage);
        _currentPassage = "";
        _paper.text = ""; 
    }

    private void DeleteCode(bool second)
    {
        int index = _currentPassage.LastIndexOf('<');
        if (index == 0)
        {
            _currentPassage = ""; 
        }
        else if (_currentPassage[index - 1] != '>' && !second)
        {
            _currentPassage = _currentPassage.Substring(0, index - 1); 
        }
        else
        {
            _currentPassage = _currentPassage.Substring(0, index); 
        }
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
