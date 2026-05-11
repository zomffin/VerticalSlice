using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Event", menuName = "Scriptable Objects/Events/Event")]
public class Event : ScriptableObject
{
    [Header("Tasks")]
    public bool orderMatters;
    public List<Task> tasks;
    
    [Header("Trigger Info")]
    public bool roundAdditive;
    public int roundTrigger;

    public bool hasTrigger;
    public string trigger;

    public bool isOneAtATime; 

    [Header("Following Event")]
    public List<Event> nextEvents;
}
