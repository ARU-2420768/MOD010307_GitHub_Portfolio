using UnityEngine;

[CreateAssetMenu(menuName = "Framework/Ice Skating/Judge")]
public class JudgeData : ScriptableObject
{
    public string judgeName;

    [Range(0f, 1f)]
    public float strictness = 0.5f;

    public SkillCategory favouriteCategory;

    [Range(0f, 2f)]
    public float favouriteCategoryMultiplier = 1.25f;

    [Header("Appearance")]
    public Sprite Face;
    public Sprite LeftEye;
    public Sprite LeftEyeBrow;
    public Sprite RightEye;
    public Sprite RightEyeBrow;
    public Sprite Nose;
    public Sprite Mouth;
    public Sprite Hair;
    public Sprite Neck;
    public Sprite Trousers;
    public Sprite Shirt;
    public Sprite LeftArm;
    public Sprite LeftHand;
    public Sprite RightArm;
    public Sprite RightHand;
    }