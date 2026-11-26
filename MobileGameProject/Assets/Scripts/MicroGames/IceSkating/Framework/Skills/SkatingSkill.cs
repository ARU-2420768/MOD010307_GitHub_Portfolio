using UnityEngine;

[CreateAssetMenu(menuName = "Framework/Ice Skating/SkatingSkill")]
public class SkatingSkill : ScriptableObject
{
    public string skillName;
    public Difficulty skillDifficulty;
    [Header("Scoring")]
    [Range(1f, 4f)]
    public float difficultyScore;
    public float jumpSkill;
    public float spinSkill;
    public float twistSkill;
    public float artisticSkill;

    public SkillCategoryValue[] skillValues;

    public SkillAction[] requiredActions;

    [Header("Gameplay")]
    [Range(1f, 6f)]
    public float duration;

    [Range(0f, 10f)]
    public float baseScore;
}
