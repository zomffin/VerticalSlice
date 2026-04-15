using Unity.VisualScripting;
using UnityEngine;
public enum TaskType
{
    Typing
}

[CreateAssetMenu(fileName = "Task", menuName = "Scriptable Objects/Task")]

public class Task : ScriptableObject
{
    public TaskType _taskType; 
    public int _difficulty; 
}
