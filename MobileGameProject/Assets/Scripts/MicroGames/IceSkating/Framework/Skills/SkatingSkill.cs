using UnityEngine;

[CreateAssetMenu(menuName = "Framework/IceDancing/SkatingSkill")]
public class SkatingSkill : ScriptableObject
{
    public string skillName;
    public Difficulty skillDifficulty;
    public float jumpSkill;
    public float spinSkill;
    public float twistSkill;
    public float artisticSkill;
}
