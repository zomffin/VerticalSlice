using System;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _camera;
    [SerializeField] private AudioSource _typewriter; 
    
    [SerializeField] private AudioClip[] _typingClips;

    [SerializeField] private Typing _typing;

    [SerializeField] private float _beginInterval;
    [SerializeField] private float _maxInterval;
    [SerializeField] private float _musicChance;

    private float _musicTimer;

    private bool _beginHasPassed; 
    //private Player _player;
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //_player = Locator.Instance.Player;

        _typing.TypingEvent += OnType; 
    }

    // Update is called once per frame
    void Update()
    {
        if (!_beginHasPassed)
        {
            if (_musicTimer <= _beginInterval)
            {
                _musicTimer += Time.deltaTime;
            }
            else
            {
                _beginHasPassed = true;
                _musicTimer = 0; 
                Debug.Log("begin interval has passed");
            }

            return;
        }
        checkForMusic();
    }

    public void OnType()
    {
        int clip = Random.Range(0, _typingClips.Length);
        _typewriter.resource = _typingClips[clip];
        _typewriter.Play();
    }

    public void checkForMusic()
    {
        if (_camera.isPlaying)
        {
            return;
        }
        
        if (_musicTimer >= _maxInterval)
        {
            _camera.Play(); 
            Debug.Log("played because of hitting max interval");
            _musicTimer = 0;
        }
        
        float chance = Random.value;
        if (_musicChance >= chance)
        {
            _camera.Play();
            _musicTimer = 0;
            Debug.Log("played because of music chance");
        }
        else
        {
            _musicTimer += Time.deltaTime;
        }
    }
}
