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
        
    }

    public void OnType()
    {
        int clip = Random.Range(0, _typingClips.Length);
        _typewriter.resource = _typingClips[clip];
        _typewriter.Play();
    }
    
    
}
