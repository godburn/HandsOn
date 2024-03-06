using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "SoQuestion", menuName = "VSCI/Question" )]
public class SoQuestion : ScriptableObject {
    public string label = "Question ID";
    [TextArea(3,3)]
    [SerializeField] public string note = "";
    [TextArea(8,8)]
    [SerializeField] public string Question;
    [SerializeField] public List<string> options;
    [SerializeField] public int correctOption = 0;
    [TextArea(8,8)]
    [SerializeField] public string answerCorrect = "Correct";
    [TextArea(8,8)]
    [SerializeField] public string answerIncorrect = "Incorrect";

    [SerializeField] public Sprite sprite;
}
