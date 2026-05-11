using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class bag : MonoBehaviour
{
    [SerializeField] private Typing _typing;

    [SerializeField] private int _ink;

    [SerializeField] private int _whiteout;

    [SerializeField] private int _paper; 
    
    private Player _player;
    private GameObject _manager; 
    
    void Start()
    {
        _player = Locator.Instance.Player;
        _manager = Locator.Instance.gameObject; 
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("PickUp"))
        {
            return; 
        }

        _player.TakeItem(other.gameObject); 
        
        if (other.name.Contains("Ink"))
        {
            _typing.AddInk(_ink);
        } 
        else if (other.name.Contains("Paper"))
        {
            _typing.AddPaper(_paper);
        } 
        else if (other.name.Contains("Whiteout"))
        {
            _typing.AddWhiteOut(_whiteout);
        }
        else
        {
            Debug.Log("somethin did not work ehre.... touch dis: " + other.gameObject.name);
        }
        
        //CustomEvent.Trigger(_manager, "checkList", other.GameObject());
        Destroy(other.gameObject);
    }
}
