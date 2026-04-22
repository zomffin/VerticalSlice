using Unity.VisualScripting;
using UnityEngine;
public enum TaskType
{
    Typing
}

[CreateAssetMenu(fileName = "Task", menuName = "Scriptable Objects/Task")]

public class Task : ScriptableObject
{
    public string taskName; 
    public TaskType taskType; 
    public int difficulty;
    [TextArea(15, 20)] 
    public string rawText; 
}
