using UnityEngine;

[CreateAssetMenu(fileName = "TypingTask", menuName = "Scriptable Objects/Typing Task")]
public class TypingTask : ScriptableObject
{
    [TextArea(15,20)]
    public string text; 

}
