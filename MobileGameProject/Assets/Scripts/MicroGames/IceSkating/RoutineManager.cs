using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Collections;
using System;

public class RoutineManager : MonoBehaviour
{

    private static RoutineManager routineManager;

    public static RoutineManager Instance => routineManager;

    [SerializeField] private float maxDuration = 15f;
    [SerializeField] private TextMeshProUGUI skillTimerCount;

    [SerializeField] private Transform skillChoices;
    [SerializeField] private GameObject gradingFramePrefab;
    [SerializeField] private Button skillsStartButton;

    [SerializeField] private Transform skillsSelectionArea;
    [SerializeField] private Transform skillMovesArea;

    [SerializeField] private TextMeshProUGUI skillNameText;
    [SerializeField] private TextMeshProUGUI actionText;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private TextMeshProUGUI feedbackText;

    [SerializeField] private Transform actionTimerFillObject;

    [SerializeField] private Image actionTimerFill;
    
    [SerializeField] private float actionWindow = 1f;


    private List<SkatingSkill> selectedSkills = new();
    public int SelectedSkillCount => selectedSkills.Count;
    
    private Coroutine currentTimer;
    private float CurrentDuration = 0;
    public float currentDuration => CurrentDuration;

    private SkatingState currentState;
    private int currentSkillIndex;
    private int currentActionIndex;

    private SkatingSkill currentSkill;  
    private SkillAction currentAction;

    private Coroutine currentTimerCoroutine;
    private float currentActionTime;

    private int successfulActions;
    private int failedActions;

    private void Awake()
    {
        routineManager = this;
        currentState = SkatingState.BuildingRoutine;
    }

    private void Start()
    {
        UpdateSkillTimer();
        UpdateStartButton();
    }

    private void UpdateSkillTimer()
    {
        skillTimerCount.text = $"{currentDuration:F0} / {maxDuration:F0} seconds";
    }

    private void UpdateStartButton()
    {
        skillsStartButton.interactable = SelectedSkillCount  > 0;
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
        UpdateStartButton();
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
        UpdateStartButton();
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

    public void StartRoutine()
    {
        if(selectedSkills.Count == 0)
            return;

        Debug.Log("Starting Routine");

        skillsSelectionArea.gameObject.SetActive(false);
        //skillMovesArea.gameObject.SetActive(true);
        skillsStartButton.gameObject.SetActive(false);
        skillNameText.gameObject.SetActive(true);
        actionText.gameObject.SetActive(true);
        progressText.gameObject.SetActive(true);
        feedbackText.gameObject.SetActive(true);
        skillTimerCount.gameObject.SetActive(false);
        actionTimerFillObject.gameObject.SetActive(true);



        currentSkillIndex = 0;
        currentActionIndex = 0;

        successfulActions = 0;
        failedActions = 0;

        currentState = SkatingState.Countdown;

        StartCoroutine(BeginRoutine());
    }

    private IEnumerator BeginRoutine()
    {
        Debug.Log("3");
        yield return new WaitForSeconds(1);

        Debug.Log("2");
        yield return new WaitForSeconds(1);

        Debug.Log("1");
        yield return new WaitForSeconds(1);

        StartNextSkill();
    }

    private void StartNextSkill()
    {

        Debug.Log("StartNextSkill");

        if(currentSkillIndex >= selectedSkills.Count)
        {
            currentState = SkatingState.Results;
            ShowResults();
            return;
        }

        currentActionIndex = 0;

        currentSkill = selectedSkills[currentSkillIndex];
        skillNameText.text = currentSkill.skillName;


        Debug.Log($"Starting {currentSkill.skillName}");

        StartNextAction();
    }


    private void StartNextAction()
    {
        if(currentActionIndex >= currentSkill.requiredActions.Length)
        {
            currentState = SkatingState.NextSkill;

            currentSkillIndex++;

            StartNextSkill();

            return;
        }

        actionText.text = $"Action: {currentSkill.requiredActions[currentActionIndex].ToString()}";
        progressText.text = $"{currentActionIndex + 1} / {currentSkill.requiredActions.Length}";

        currentAction = currentSkill.requiredActions[currentActionIndex];

        Debug.Log($"Perform: {currentAction}");

        currentState = SkatingState.WaitingForAction;

        if(currentTimerCoroutine != null)
        {
            StopCoroutine(currentTimerCoroutine);
        }

        currentTimerCoroutine = StartCoroutine(ActionTimeout());

    }

    private IEnumerator ActionTimeout()
    {
        currentActionTime = GetActionWindow();

        float remainingTime = currentActionTime;

        actionTimerFill.fillAmount = 1f;

        while(remainingTime > 0f)
        {
            remainingTime -= Time.deltaTime;

            float percentRemaining = remainingTime / currentActionTime;
            actionTimerFill.fillAmount = percentRemaining;

            if(percentRemaining > 0.6f)
            {
                actionTimerFill.color = Color.green;
            }
            else if(percentRemaining > 0.3f)
            {
                actionTimerFill.color = Color.yellow;
            }
            else
            {
                actionTimerFill.color = Color.red;
            }

            yield return null;
        }

        actionTimerFill.fillAmount = 0f;

        if(currentState != SkatingState.WaitingForAction)
            yield break;

        Debug.Log("Timed Out");

        failedActions++;

        feedbackText.color = Color.red;
        feedbackText.text = "TOO SLOW";

        currentActionIndex++;

        StartNextAction();
    }

    public void RegisterAction(SkillAction action)
    {

        if (currentState != SkatingState.WaitingForAction)
            return;

        if(currentTimerCoroutine != null)
        {
            StopCoroutine(currentTimerCoroutine);
        }

        actionTimerFill.fillAmount = 0f;

        Debug.Log($"Expected: {currentAction}");
        Debug.Log($"Received: {action}");

        if(currentTimer != null)
        {
            StopCoroutine(currentTimer);
        }

        currentState = SkatingState.EvaluatingAction;

        bool success = action == currentAction;

        if (success)
        {
            successfulActions++;

            Debug.Log("Correct Action");
        }
        else
        {
            failedActions++;

            Debug.Log("Incorrect Action");
        }

        currentActionIndex++;

        StartNextAction();
    }

    private void ShowResults()
    {
        currentState = SkatingState.Results;

        float accuracy = (float)successfulActions / (successfulActions + failedActions);

        Debug.Log("Routine Complete");

        Debug.Log($"Successes: {successfulActions}");
        Debug.Log($"Failures: {failedActions}");
    }


    private float GetActionWindow()
    {
        return 2f;
        switch(currentSkill.skillDifficulty)
        {
            case Difficulty.Easy:
                return 1.5f;

            case Difficulty.Intermediate:
                return 1.25f;

            case Difficulty.Advanced:
                return 1f;

            case Difficulty.Expert:
                return 0.75f;

            default:
                return 1f;
        }
    }

}
