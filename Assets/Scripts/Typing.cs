using System;
using System.Transactions;
using TMPro;
using Unity.VisualScripting;
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
    [SerializeField] private TextMeshProUGUI paperUI;

    private int _currInk; 
    private int _currDelete;
    private int _currPaper;

    private int _currCorrect;
    private int _currIncorrect; 
    
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
        _currPaper =  _startingPaper;

        SetUI(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && _currPaper > 0)
        {
            EjectPaper(); 
            _player.FinishTyping();
        }
        else
        {
            CheckGameOver(); 
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
                            _currIncorrect--; 
                        }
                        else
                        {
                            _currentPassage = _currentPassage.Substring(0, _currentPassage.Length - 1);
                            _paper.text = _currentPassage;
                            _currCorrect--; 
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
                        _currCorrect++; 
                    }
                    else
                    {
                        _currentPassage += "<color=red>" + c + "</color>";
                        _paper.text = _currentPassage;
                        _place++;
                        _currIncorrect++;
                    }
                    
                    _currInk--;
                }
            }  
            
            SetUI();
            CheckGameOver(); 

        }
        
        
    }
    
    public void SetTask(string task)
    {
        _taskPassage = task;
        _place = 0;
        //Debug.Log("task was set to:" + _taskPassage);
    }

    private void CheckGameOver()
    {
        // put more complex check, shouldn't be a game over if there's no more delete unless theres a mistake ?
        if (_currDelete <= 0 || _currInk <= 0 || _currPaper <= 0 && _place < _taskPassage.Length)
        {
            Debug.Log("something is less or equal to 0 ");
            if (_currInk <= 0)
            {
                CustomEvent.Trigger(_gameManager, "checkGameOver", 0);
            } else if (_currDelete <= 0)
            {
                CustomEvent.Trigger(_gameManager, "checkGameOver", 1);
            } else if (_currPaper <= 0)
            {
                CustomEvent.Trigger(_gameManager, "checkGameOver", 2);
            }
        }
    }

    private void SetUI()
    {
        inkUI.text = "Ink: " + _currInk; 
        deleteUI.text = "Whiteout: " + _currDelete;
        paperUI.text = "Paper: " + _currPaper;
    }

    public void AddInk(int num)
    {
        _currInk += num;
        SetUI(); 
    }

    public void AddWhiteOut(int num)
    {
        _currDelete += num;
        SetUI(); 
    }

    public void AddPaper(int num)
    {
        _currPaper += num;
        SetUI(); 
    }

    private void EjectPaper()
    {
        // float _percCorrect = (_currCorrect / (float)_taskPassage.Length); 
        int _total = _currCorrect + _currIncorrect;
        float _percCorrect; 

        if (_total <= _taskPassage.Length)
        {
            _percCorrect = (_currCorrect - _currIncorrect) / (float)_taskPassage.Length; 
        }
        else
        {
            _percCorrect = _currCorrect / (float)_total;
        }
        

        Debug.Log("perc correct: " + _percCorrect);
        Debug.Log("current correct: " + _currCorrect + "/ " + _total);
        CustomEvent.Trigger(_gameManager, "finishTask", _percCorrect, _currentPassage);
        _currentPassage = "";
        _paper.text = "";
        _currCorrect = 0;
        _currPaper--; 
        SetUI();
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

    public int[] GetResourceCount()
    {
        int[] resources = {_currInk, _currDelete,_currPaper};
        return resources;
    }
    
    
}
