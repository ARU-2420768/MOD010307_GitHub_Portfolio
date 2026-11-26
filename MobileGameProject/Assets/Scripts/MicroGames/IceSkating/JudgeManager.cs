using System.Collections.Generic;
using UnityEngine;

public class JudgeManager : MonoBehaviour
{
    private static JudgeManager judgeManager;

    public static JudgeManager Instance => judgeManager;

    private readonly Dictionary<JudgeScoreCategory, float>  scoreWeightings = new()
    {
        { JudgeScoreCategory.Execution, 0.5f },
        { JudgeScoreCategory.Difficulty, 0.3f },
        { JudgeScoreCategory.RoutineLength, 0.2f }
    };

    [SerializeField] private GameObject judgePrefab;
    [SerializeField] private Transform judgesArea;
    [SerializeField] private JudgeData[] Judges;

    public JudgeData[] judges => Judges;


    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        judgeManager = this;
    }
    private void Start()
    {

        Debug.Log("Judges Area Populated");
        foreach(JudgeData judge in Judges)
        {

            GameObject judgeObject = Instantiate(judgePrefab, judgesArea, false);

            judgeObject.GetComponent<NewJudgeScript>().Initialise(judge);
        }
    }

    public float GetWeighting(JudgeScoreCategory category)
    {
        return scoreWeightings[category];
    }
}
