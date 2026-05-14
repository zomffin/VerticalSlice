using UnityEngine;

[CreateAssetMenu(fileName = "TypingTask", menuName = "Scriptable Objects/Tasks/Typing Task")]
public class TypingTask : Task
{
    public bool hasSpecialInstruction;
    public string specialInstruction; 
    
    [TextArea(15,20)]
    public string displayText; 

}
