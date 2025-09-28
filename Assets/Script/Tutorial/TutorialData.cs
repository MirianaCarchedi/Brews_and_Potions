using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialData", menuName = "Tutorial/TutorialData")]
public class TutorialData : ScriptableObject
{
    public List<TutorialStep> steps = new List<TutorialStep>();
}
