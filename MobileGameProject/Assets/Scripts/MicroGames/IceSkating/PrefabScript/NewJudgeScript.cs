using UnityEngine;

public class NewJudgeScript : MonoBehaviour
{

    [SerializeField] private SpriteRenderer Face;
    [SerializeField] private SpriteRenderer LeftEye;
    [SerializeField] private SpriteRenderer LeftEyeBrow;
    [SerializeField] private SpriteRenderer RightEye;
    [SerializeField] private SpriteRenderer RightEyeBrow;
    [SerializeField] private SpriteRenderer Nose;
    [SerializeField] private SpriteRenderer Mouth;
    [SerializeField] private SpriteRenderer Hair;
    [SerializeField] private SpriteRenderer Neck;
    [SerializeField] private SpriteRenderer Trousers;
    [SerializeField] private SpriteRenderer Shirt;
    [SerializeField] private SpriteRenderer LeftArm;
    [SerializeField] private SpriteRenderer LeftHand;
    [SerializeField] private SpriteRenderer RightArm;
    [SerializeField] private SpriteRenderer RightHand;

    private JudgeData judge;

    public void Initialise(JudgeData judgeData)
    {
        judge = judgeData;
        Face.sprite = judge.Face;
        LeftEye.sprite = judge.LeftEye;
        LeftEyeBrow.sprite = judge.LeftEyeBrow;
        RightEye.sprite = judge.RightEye;
        RightEyeBrow.sprite = judge.RightEyeBrow;
        Nose.sprite = judge.Nose;
        Mouth.sprite = judge.Mouth;
        Hair.sprite = judge.Hair;
        Neck.sprite = judge.Neck;
        Trousers.sprite = judge.Trousers;
        Shirt.sprite = judge.Shirt;
        LeftArm.sprite = judge.LeftArm;
        LeftHand.sprite = judge.LeftHand;
        RightArm.sprite = judge.RightArm;
        RightHand.sprite = judge.RightHand;
    }

}
