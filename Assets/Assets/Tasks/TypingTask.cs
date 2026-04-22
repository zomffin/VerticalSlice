using UnityEngine;

[CreateAssetMenu(fileName = "TypingTask", menuName = "Scriptable Objects/Typing Task")]
public class TypingTask : Task
{
    [TextArea(15,20)]
    public string displayText; 

}
