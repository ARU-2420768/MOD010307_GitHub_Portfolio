using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillDisplay : MonoBehaviour
{
    [SerializeField] private Image background;

    [SerializeField] private RectTransform barsBackground;
    [SerializeField] private RectTransform jumpBar;
    [SerializeField] private RectTransform spinBar;
    [SerializeField] private RectTransform twistBar;
    [SerializeField] private RectTransform artisticBar;

    [SerializeField] private TextMeshProUGUI skillName;

    private SkatingSkill skatingSkill;

    private SkillDisplayMode mode;

    public void Initialise(SkatingSkill skill, SkillDisplayMode displayMode)
    {
        skatingSkill = skill;
        mode = displayMode;

        GetComponent<Button>().onClick.RemoveAllListeners();

        GetComponent<Button>().onClick.AddListener(OnClick);

        background.color =
            DifficultyColours.Colours[skill.skillDifficulty];

        skillName.text = skill.skillName;

        float height = barsBackground.rect.height;

        jumpBar.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Vertical,
            height * skill.jumpSkill);

        spinBar.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Vertical,
            height * skill.spinSkill);

        twistBar.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Vertical,
            height * skill.twistSkill);

        artisticBar.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Vertical,
            height * skill.artisticSkill);
    }

    public void OnClick()
    {
        Debug.Log($"Clicked {skatingSkill.skillName}");

        switch(mode)
        {
            case SkillDisplayMode.Select:
                RoutineManager.Instance.AddSkill(skatingSkill);
                break;

            case SkillDisplayMode.Remove:
                RoutineManager.Instance.RemoveSkill(skatingSkill);
                Destroy(gameObject);
                break;
        }
    }

}