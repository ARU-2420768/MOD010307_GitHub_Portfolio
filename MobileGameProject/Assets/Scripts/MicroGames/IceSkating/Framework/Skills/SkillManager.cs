using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{

    [SerializeField] private SkatingSkill[] skills = new SkatingSkill[9];
    [SerializeField] private GameObject skillPrefab;
    [SerializeField] private RectTransform skillsSelection;

    float xPadding = (355f * 0.5f) / Screen.width;
    float yPadding = (355f * 0.5f) / Screen.height;

    private Vector2[] positions;



    private Camera mainCamera;

    private void Awake()
    {



    }

    private void Start()
    {
        Debug.Log($"Skills Array has : {skills.Length} elements");

        int columns = 3;
        int rows = 3;

        float cellWidth = skillsSelection.rect.width / columns;
        float cellHeight = skillsSelection.rect.height / rows;

        for (int i = 0; i < columns * rows; i++)
        {
            GameObject skill = Instantiate(skillPrefab, skillsSelection);
            RectTransform rect = skill.GetComponent<RectTransform>();

            int row = i / columns;
            int col = i % columns;

            float x = (-skillsSelection.rect.width * 0.5f) + (cellWidth * 0.5f) + (col * cellWidth);
            float y = (skillsSelection.rect.height * 0.5f) - (cellHeight * 0.5f) - (row * cellHeight);

            rect.anchoredPosition = new Vector2(x, y);
            
            skill.GetComponent<Image>().color = DifficultyColours.Colours[skills[i].skillDifficulty];

            Transform backGround = skill.transform.Find("Background");

            Transform jump = skill.transform.Find("Background/Jump");
            Transform spin = skill.transform.Find("Background/Spin");
            Transform twist = skill.transform.Find("Background/Twist");
            Transform artistic = skill.transform.Find("Background/Artistic");

            RectTransform backGroundRect = backGround.GetComponent<RectTransform>();

            RectTransform jumpRect = jump.GetComponent<RectTransform>();
            RectTransform spinRect = spin.GetComponent<RectTransform>();
            RectTransform twistRect = twist.GetComponent<RectTransform>();
            RectTransform artisticRect = artistic.GetComponent<RectTransform>();

            jumpRect.sizeDelta = new Vector2(jumpRect.sizeDelta.x, skills[i].jumpSkill * rect.transform.localScale.y);
            spinRect.sizeDelta = new Vector2(spinRect.sizeDelta.x, skills[i].spinSkill * rect.transform.localScale.y);
            twistRect.sizeDelta = new Vector2(twistRect.sizeDelta.x, skills[i].twistSkill * rect.transform.localScale.y);
            artisticRect.sizeDelta = new Vector2(artisticRect.sizeDelta.x, skills[i].artisticSkill * rect.transform.localScale.y);

            jumpRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, backGroundRect.rect.height * skills[i].jumpSkill);
            spinRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, backGroundRect.rect.height * skills[i].spinSkill);
            twistRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, backGroundRect.rect.height * skills[i].twistSkill);
            artisticRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, backGroundRect.rect.height * skills[i].artisticSkill);

            Transform name = skill.transform.Find("SkillName");
            name.GetComponent<TextMeshProUGUI>().text = skills[i].skillName;

        }
    }
}
