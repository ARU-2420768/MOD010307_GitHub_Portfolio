using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

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
        // max skills to fit into UI 3 columns, 2 rows
        if (selectedSkills.Count >= 6)
            return;

        // Use LINQ for skills count 
        if (selectedSkills.Count(s => s == skill) >= 2)
        {
            Debug.Log($"{skill.skillName} already selected twice");
            return;
        }

        Debug.Log($"Adding {skill.skillName}");
        if(!CanAddSkill(skill))
            return;

        selectedSkills.Add(skill);
        CurrentDuration += skill.duration;
        UpdateSkillTimer();
        CreateSkillChoice(skill);
    }

    private int GetSkillCount(SkatingSkill skill)
    {
        int count = 0;

        foreach (var selectedSkill in selectedSkills)
        {
            if (selectedSkill == skill)
                count++;
        }

        return count;
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
        choice.transform.localScale = Vector3.one * 0.75f;
        choice.GetComponent<SkillDisplay>().Initialise(skill, SkillDisplayMode.Remove);
    }

    public IReadOnlyList<SkatingSkill> GetRoutine()
    {
        return selectedSkills;
    }


}
