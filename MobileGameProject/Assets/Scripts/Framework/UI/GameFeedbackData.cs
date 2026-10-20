using UnityEngine;

[CreateAssetMenu(menuName = "Framework/Game Feedback")]
public class GameFeedbackData : ScriptableObject
{
    public string success;
    public Color successColour;
    public string failure;
    public Color failureColour;
}
