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
        [SerializeField] private GameConfigurationData gameConfig;

        [Header("Scene References")]
        [SerializeField] private RectTransform playArea;
        [SerializeField] private TextMeshProUGUI progressText;
        [SerializeField] private TextMeshProUGUI feedbackText;
        [SerializeField] private GameObject playArea3D;

        [Header("Gates Prefabs")]
        [SerializeField] public Vector3 lastGateSpawnLocation = new Vector3(0f,0.45f,0f);
        [SerializeField] public float gateMoveSpeed = 1f;
        [SerializeField] public float gateMaxMoveSpeed = 3f;
        [SerializeField] public float gateMinMoveSpeed = 1f;
        
        private Camera mainCamera;
        public int score,gatesCleared;
        private MedalThresholdsData thresholds;
        public bool IsGameRunning;
        

        private void Awake()
        {
            mainCamera = Camera.main;
            thresholds = gameConfig.medalThresholds;
        }

        //public override void Begin(MicrogameSession session)
        //{
        //    base.Begin(session);
        //    IsRunning = true;
        //}

        public override void End()
        {
            base.End();
            MedalType medal = CalculateMedal();
            MedalManager.Instance.AwardMedal(gameName, medal);
            if (medal != MedalType.Unranked)
                Win();
            else
                Lose();
            base.IsRunning = false;
        }

        private void Start()
        {
        }

        private void Update()
        {
            if (!IsRunning) return;
            lastGateSpawnLocation += Mathf.Clamp(gateMoveSpeed,gateMinMoveSpeed,gateMaxMoveSpeed) * Time.deltaTime * Vector3.up;
        }


        public void GateMissed()
        {
            ShowFeedback("Gate Missed !", Color.red);
            Debug.Log($"Score: {score}");
            UpdateProgress();
            CheckForCompletion();
        }
        public void GatePassed()
        {
            ShowFeedback("Gate Passed !", Color.green);
            gatesCleared++;
            Debug.Log($"Score: {score}");

            score++;
            UpdateProgress();
            CheckForCompletion();
        }

        private void CheckForCompletion()
        {
            Debug.Log($"CheckForCompletion score:{score}");
            if (score > gameConfig.medalThresholds.gold) 
            {
                MedalType medal = CalculateMedal();
                Debug.Log($"MedalManager.Instance = {MedalManager.Instance}");
                MedalManager.Instance.AwardMedal(gameName, medal);
                Win();
            }
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

        public void CreateNewGate()
        {
            if (!IsRunning) return;
        }

        public void ShowFeedback(string message, Color? colour = null)
        {
            feedbackText.color = colour ?? Color.white;
            feedbackText.text = message;
        }

        public void UpdateProgress()
        {
            progressText.gameObject.SetActive(true);
            progressText.text = $"Gates Success: {score} / {gameConfig.medalThresholds.gold}";
        }

    }
}