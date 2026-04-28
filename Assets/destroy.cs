using Unity.VisualScripting;
using UnityEngine;

public class destroy : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Destroy(other.GameObject());
    }
}
