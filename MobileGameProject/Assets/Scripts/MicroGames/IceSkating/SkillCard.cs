using UnityEngine;
public class SkillCard : MonoBehaviour
{
    private SkatingSkill skill;

    public void Initialise(SkatingSkill skatingSkill)
    {
        skill = skatingSkill;
    }

    public void OnClick()
    {
        RoutineManager.Instance.AddSkill(skill);
    }
}