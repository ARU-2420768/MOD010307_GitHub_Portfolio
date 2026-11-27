using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using MicrogameCourse.Microgames;



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
    [SerializeField] private TextMeshProUGUI JudgesScore;

    [SerializeField] private Transform actionTimerFillObject;

    [SerializeField] private Image actionTimerFill;

    [SerializeField] private RectTransform skillMovesPlayArea;
    [SerializeField] private GameObject tapPrefab;
    [SerializeField] private GameObject swipePrefab;
    [SerializeField] private GameObject twoFingerTapPrefab;
    [SerializeField] private GameObject twoFingerDragPrefab;


    private RectTransform currentTapTarget;
    private SwipeAction currentSwipeAction;
    private TwoFingerTapAction currentTwoFingerTapAction;
    

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

        Debug.Log($"[RoutineManager] StartRoutine called | State={currentState} | SelectedCount={selectedSkills.Count} | Duration={currentDuration}/{maxDuration}");

        if(selectedSkills.Count == 0)
        {
            Debug.LogWarning("[RoutineManager] StartRoutine aborted: selectedSkills is empty.");
            return;
        }

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
        Debug.Log("[RoutineManager] Starting BeginRoutine coroutine...");
        StartCoroutine(BeginRoutine());
    }

    private IEnumerator BeginRoutine()
    {
        
        Debug.Log("3");
        skillsStartButton.gameObject.SetActive(true);
        skillsStartButton.GetComponent<TextMeshProUGUI>().text = "3";
        yield return new WaitForSeconds(1);

        Debug.Log("2");
        skillsStartButton.GetComponent<TextMeshProUGUI>().text = "2";
        yield return new WaitForSeconds(1);

        Debug.Log("1");
        skillsStartButton.GetComponent<TextMeshProUGUI>().text = "1";
        yield return new WaitForSeconds(1);

        skillsStartButton.gameObject.SetActive(false);
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

        ClearExistingTargets();

        switch (currentAction)
        {
            case SkillAction.Tap:
                SpawnTapTarget();
                break;
            case SkillAction.Swipe:
                SpawnSwipeTarget();
                break;
            case SkillAction.TwoFingerTap:
                SpawnTwoFingerTapTarget();
                break;
           default:
                break;
        }

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

    private void SpawnTapTarget()
    {
        Debug.Log("SpawnTapTarget");
        GameObject target = Instantiate(tapPrefab, skillMovesPlayArea);

        currentTapTarget =  target.GetComponent<RectTransform>();
    }

    private void SpawnSwipeTarget()
    {
        Debug.Log("SpawnSwipeTraget");
        GameObject swipeObject  = Instantiate(swipePrefab, skillMovesPlayArea);

        currentSwipeAction =  swipeObject.GetComponent<SwipeAction>();

        PositionSwipeMarkers();
        
    }


    private void SpawnTwoFingerTapTarget()
    {
        Debug.Log("SpawnTwoFingerTapTarget");
        GameObject twoFingerTapObject =  Instantiate(twoFingerTapPrefab, skillMovesPlayArea);
        currentTwoFingerTapAction = twoFingerTapObject.GetComponent<TwoFingerTapAction>();

        PositionTwoFingerTapMarkers();
    }

    private void PositionTwoFingerTapMarkers()
    {
        RectTransform leftRect = currentTwoFingerTapAction.BoxClick1;

        RectTransform rightRect = currentTwoFingerTapAction.BoxClick2;

        Rect areaRect = skillMovesPlayArea.rect;

        float halfWidth = leftRect.rect.width * 0.5f;

        float halfHeight = leftRect.rect.height * 0.5f;

        Vector2 centrePosition =  new Vector2(Random.Range(areaRect.xMin + 150f, areaRect.xMax - 150f), Random.Range(areaRect.yMin + halfHeight, areaRect.yMax - halfHeight));

        leftRect.anchoredPosition = centrePosition + Vector2.left * 80f;

        rightRect.anchoredPosition = centrePosition + Vector2.right * 80f;
    }

    private void PositionSwipeMarkers()
    {
        RectTransform startRect = currentSwipeAction.StartMarker;

        RectTransform endRect = currentSwipeAction.EndMarker;

        Rect areaRect = skillMovesPlayArea.rect;

        float startHalfWidth = startRect.rect.width * 0.5f;

        float startHalfHeight = startRect.rect.height * 0.5f;

        Vector2 startPosition = new Vector2(UnityEngine.Random.Range(areaRect.xMin + startHalfWidth, areaRect.xMax - startHalfWidth),
                                    UnityEngine.Random.Range(areaRect.yMin + startHalfHeight, areaRect.yMax - startHalfHeight));

        Vector2 direction = UnityEngine.Random.insideUnitCircle.normalized;

        float swipeDistance = 250;

        Vector2 endPosition = startPosition + direction * swipeDistance;

        float endHalfWidth = endRect.rect.width * 0.5f;

        float endHalfHeight = endRect.rect.height * 0.5f;

        endPosition.x = Mathf.Clamp(endPosition.x, areaRect.xMin + endHalfWidth, areaRect.xMax - endHalfWidth);
        endPosition.y = Mathf.Clamp(endPosition.y, areaRect.yMin + endHalfHeight, areaRect.yMax - endHalfHeight);

        startRect.anchoredPosition = startPosition;

        endRect.anchoredPosition = endPosition;
    }

    public void ValidateTapAction()
    {
        Debug.Log("ValidateTapAction");
        if(currentAction != SkillAction.Tap)
            return;

        Vector2 pointerPosition = Pointer.current.position.ReadValue();           

        bool insideTarget =
            RectTransformUtility.RectangleContainsScreenPoint(
                currentTapTarget,
                pointerPosition);

        if(insideTarget)
        {
            RegisterAction(SkillAction.Tap);
        }

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

        // check this is in the correct location
        bool success = action == currentAction;

        if (success)
        {
            feedbackText.text = "Action Correct";
            successfulActions++;

            Debug.Log("Correct Action");
        }
        else
        {
            feedbackText.text = "Action Wrong";
 
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

        feedbackText.text = "Routine Complete";


        Debug.Log("Routine Complete");

        Debug.Log($"Successes: {successfulActions}");
        Debug.Log($"Failures: {failedActions}");

        Dictionary<JudgeData, float> scores =  CalculateJudgesScores(selectedSkills, accuracy);

        DisplayResults(scores);
    }


    public void DisplayResults(Dictionary<JudgeData, float> scores)
    {
        ClearExistingTargets();
        JudgesScore.text = "Judges Scores:";
        float finalScore = 0f;
        foreach(KeyValuePair<JudgeData, float> score in scores)
        {
            finalScore += score.Value;
            JudgesScore.text += $"\n {score.Key.judgeName} :{score.Value:F1}";
        }
        JudgesScore.text += $"\nFinal Score: {finalScore:F1}";
        StartCoroutine(EndGame());

    }

    public Dictionary<JudgeData,float>  CalculateJudgesScores(IReadOnlyList<SkatingSkill> routine, float accuracy)
    {
        Dictionary<JudgeData,float> scores = new();

        foreach(var judge in JudgeManager.Instance.judges)
        {
            scores.Add(judge, CalculateJudgeScore(judge, routine, accuracy));
        }

        return scores;
    }


    public float CalculateJudgeScore(
        JudgeData judge,
        IReadOnlyList<SkatingSkill> routine,
        float accuracy)
    {
        float executionScore =
            accuracy * 10f;

        float difficultyScore =
            CalculateDifficultyScore(
                judge,
                routine);

        float baseScore =
            (
                executionScore * 0.625f
            )
            +
            (
                difficultyScore * 0.375f
            );

        float routineLengthModifier =
            CurrentDuration / maxDuration;

        float finalScore =
            baseScore *
            routineLengthModifier;

        finalScore *=
            Mathf.Lerp(
                1.2f,
                0.8f,
                judge.strictness);

        return Mathf.Clamp(
            finalScore,
            0f,
            10f);
    }


   private float CalculateDifficultyScore(JudgeData judge, IReadOnlyList<SkatingSkill> routine)
    {

        float difficultyPoints = 0f;
        float maximumDifficultyPoints = 0f;

        foreach(var skill in routine)
        {
            float contribution = skill.difficultyScore * skill.duration;

            foreach(var value in skill.skillValues)
            {
                if(value.category == judge.favouriteCategory)
                {
                    contribution *= judge.favouriteCategoryMultiplier;
                    break;
                }
            }

            difficultyPoints += contribution;

            maximumDifficultyPoints += 4f * skill.duration;
        }

        if(maximumDifficultyPoints <= 0f)
            return 0f;

        return Mathf.Clamp((difficultyPoints / maximumDifficultyPoints) * 10f, 0f, 10f);
    }

    private float GetActionWindow()
    {
        return 3f;
        switch(currentSkill.skillDifficulty)
        {
            case Difficulty.Easy:
                return 3f;
//                return 1.5f;

            case Difficulty.Intermediate:
                return 2.5f;
//                return 1.25f;

            case Difficulty.Advanced:
                return 2f;
//                return 1f;

            case Difficulty.Expert:
                return 1.5f;
//                return 0.75f;

            default:
                return 1f;
        }
    }

    private void ClearExistingTargets()
    {
        foreach(Transform child in skillMovesPlayArea)
        {
            Destroy(child.gameObject);
        }
    }

    private IEnumerator EndGame()
    {
        Debug.Log("End Game in 3");
        yield return new WaitForSeconds(1);

        Debug.Log("End Game in 2");
        yield return new WaitForSeconds(1);

        Debug.Log("End Game in 1");
        yield return new WaitForSeconds(1);

        skillsStartButton.gameObject.SetActive(false);
        IceSkatingGame.Instance.CheckForCompletion();
    }




}
