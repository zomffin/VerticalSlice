using NUnit.Framework;
using UnityEngine;

public class bag : MonoBehaviour
{
    [SerializeField] private Typing _typing;

    [SerializeField] private int _ink;

    [SerializeField] private int _whiteout;

    [SerializeField] private int _paper; 
    
    private Player _player; 
    void Start()
    {
        _player = Locator.Instance.Player;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("PickUp"))
        {
            return; 
        }

        _player.TakeItem(); 
        
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
            Debug.Log("somethin did not work ehre....");
        }
        
        Destroy(other.gameObject);
    }
}
