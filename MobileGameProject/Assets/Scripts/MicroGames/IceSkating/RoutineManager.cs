using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.YamlDotNet.Core;
using UnityEngine;

public class RoutineManager : MonoBehaviour
{

    private static RoutineManager routineManager;

    public static RoutineManager Instance => routineManager;

    [SerializeField] private float maxDuration = 15f;
    [SerializeField] private TextMeshProUGUI skillTimerCount;

    [SerializeField] private Transform skillChoices;
    [SerializeField] private GameObject gradingFramePrefab;

    private List<SkatingSkill> selectedSkills = new();
    
    private float CurrentDuration = 0;
    public float currentDuration => CurrentDuration;

    private void Awake()
    {
        routineManager = this;
    }

    private void Start()
    {
        UpdateSkillTimer();
    }

    private void UpdateSkillTimer()
    {
        skillTimerCount.text = $"{currentDuration:F0} / {maxDuration:F0} seconds";
    }

    public bool CanAddSkill(SkatingSkill skill)
    {
        return currentDuration + skill.duration <= maxDuration;
    }

    public void AddSkill(SkatingSkill skill)
    {
        Debug.Log($"Adding {skill.skillName}");
        if(!CanAddSkill(skill))
            return;

        selectedSkills.Add(skill);
        CurrentDuration += skill.duration;
        UpdateSkillTimer();
        CreateSkillChoice(skill);
    }

    public void RemoveSkill(SkatingSkill skill)
    {
        if (!selectedSkills.Contains(skill))
            return;

        selectedSkills.Remove(skill);

        CurrentDuration -= skill.duration;

        UpdateSkillTimer();
    }

    private void CreateSkillChoice(SkatingSkill skill)
    {
        GameObject choice = Instantiate(gradingFramePrefab, skillChoices);

        choice.GetComponent<SkillDisplay>().Initialise(skill, SkillDisplayMode.Remove);
    }

    public IReadOnlyList<SkatingSkill> GetRoutine()
    {
        return selectedSkills;
    }


}
