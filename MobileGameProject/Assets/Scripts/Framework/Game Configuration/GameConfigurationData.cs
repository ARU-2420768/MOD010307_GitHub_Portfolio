using MicrogameCourse.ProgressionFramework;
using UnityEngine;

[CreateAssetMenu(menuName = "Framework/Game Configuration")]
public class GameConfigurationData : ScriptableObject
{
    public string gameName;
    public string description;

    public InputProfileData inputProfile;
    public MovementProfileData  movementProfile;
    public MedalThresholdsData medalThresholds;
    
}
