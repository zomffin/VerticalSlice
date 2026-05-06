using Unity.VisualScripting;
using UnityEngine;

public class destroy : MonoBehaviour
{
    private Player _player;
    private GameObject _manager;
    void Start()
    {
        _player = Locator.Instance.Player;
        _manager = Locator.Instance.gameObject; 
    }
    
    void OnTriggerEnter(Collider other)
    {
        _player.TakeItem();
        //CustomEvent.Trigger(_manager, "checkList", other.GameObject());
        Destroy(other.GameObject());
    }
}
