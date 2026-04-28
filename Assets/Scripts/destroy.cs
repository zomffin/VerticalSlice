using Unity.VisualScripting;
using UnityEngine;

public class destroy : MonoBehaviour
{
    private Player _player; 
    void Start()
    {
        _player = Locator.Instance.Player;
    }
    
    void OnTriggerEnter(Collider other)
    {
        _player.TakeItem();
        Destroy(other.GameObject());
    }
}
