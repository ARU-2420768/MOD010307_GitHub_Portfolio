using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MicrogameCourse.Framework;
using MicrogameCourse.ProgressionFramework;
using System;

namespace MicrogameCourse.Microgames
{

    public sealed class SlalomGame : MicrogameBehaviour
    {

        [Header("Medal Success Levels - Thresholds")]
        [SerializeField] private string gameName = "Slalom";
        [SerializeField] private MedalThresholdsData thresholds;
        

        [Header("Scene References")]
        [SerializeField] private RectTransform playArea;
        [SerializeField] private TextMeshProUGUI progressText;
        [SerializeField] private TextMeshProUGUI feedbackText;
        [SerializeField] private GameObject playArea3D;
        

        [Header("Gates Prefabs")]
        [SerializeField] private GameObject startGatesPrefab;
        [SerializeField] private GameObject gatesPrefab;
        [SerializeField] private GameObject fastGatesPrefab;
        [SerializeField] public float gateMoveSpeed = 1f;
        [SerializeField] public float gateMaxMoveSpeed = 3f;
        [SerializeField] public float gateMinMoveSpeed = 1f;
        
      

        private Camera mainCamera;
        public int score,gatesCleared;
        
        

        private GameObject[] Kitzbuhel_Gates;


        private readonly float[] Kitzbuhel = { 0f, -1f, 0f, -1f, 1f, 1f, 0f, 1f, -1, 0, 0f, -1f, 0f, -1f, 1f, 1f, 0f, 1f, -1, 0 };
        private int gateNumber = 0;

        private void Awake()
        {
            mainCamera = Camera.main;
            Kitzbuhel_Gates = new GameObject[]{gatesPrefab,gatesPrefab,gatesPrefab,fastGatesPrefab,gatesPrefab,fastGatesPrefab,gatesPrefab,fastGatesPrefab,gatesPrefab};
            //Kitzbuhel_Gates = new GameObject[]{gatesPrefab,gatesPrefab,gatesPrefab};
            
        }

        private MedalType CalculateMedal()
        {
            if(score >= thresholds.gold)
                return MedalType.Gold;
            if(score >= thresholds.silver)
                return MedalType.Silver;
            if(score >= thresholds.bronze)
                return MedalType.Bronze;
            return MedalType.Unranked;
        }

        public override void Begin(MicrogameSession session)
        {
            base.Begin(session);
        }

        private void Start()
        {
            InitialiseFirstNewGates();
        }

        private void Update()
        {
            if (!IsRunning) return;
        }

        public void GatePassed()
        {
            //Debug.Log("Gate Passed");
            ShowFeedback("Gate Passed !", Color.green);
            gatesCleared++;
            score++;
            UpdateProgress();
            CheckForCompletion();
        }

        private void CheckForCompletion()
        {
            //Debug.Log($"Score: {score},Gates Cleared: {gatesCleared}, Kitzbuhel Length: {Kitzbuhel_Gates.Length}");
            if(gatesCleared >= Kitzbuhel_Gates.Length)
            {
                MedalType medal = CalculateMedal();
                MedalManager.Instance.AwardMedal(gameName, medal);
                Win();
            }
        }

        private void InitialiseFirstNewGates()
        {
            Instantiate(startGatesPrefab, new Vector3(Kitzbuhel[gateNumber], -2.5f + gateNumber, 0), Quaternion.identity, playArea);
            gateNumber++;
            for (int i = 1; i < 6; i++)
            {
                Instantiate(Kitzbuhel_Gates[gateNumber], new Vector3(Kitzbuhel[gateNumber], -2.5f + (2 * gateNumber), 0), Quaternion.identity, playArea);
                gateNumber++;
            }
        }

        public void CreateNewGate()
        {
            if (!IsRunning) return;
            Instantiate(Kitzbuhel_Gates[gateNumber % Kitzbuhel_Gates.Length], new Vector3(Kitzbuhel[gateNumber % Kitzbuhel.Length] , 3.5f, 0), Quaternion.identity, playArea);
            gateNumber++;
        }

        public void ShowFeedback(string message, Color? colour = null)
        {
            feedbackText.color = colour ?? Color.white;
            feedbackText.text = message;
        }

        public void UpdateProgress()
        {
            progressText.gameObject.SetActive(true);
            progressText.text = $"Gates Success: {score} / {Kitzbuhel_Gates.Length}";
        }

    }
}