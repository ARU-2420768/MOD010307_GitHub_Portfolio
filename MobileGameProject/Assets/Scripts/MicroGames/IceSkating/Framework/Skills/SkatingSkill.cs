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

    [Header("Gameplay")]
    [Range(1f, 6f)]
    public float duration;

    [Range(0f, 10f)]
    public float baseScore;
}
