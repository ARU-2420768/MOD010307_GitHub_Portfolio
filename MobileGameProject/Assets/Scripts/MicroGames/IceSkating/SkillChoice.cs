using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillChoice : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI skillName;

    public void Initialise(SkatingSkill skill)
    {
        skillName.text = skill.skillName;

        background.color = DifficultyColours.Colours[skill.skillDifficulty];
    }
}