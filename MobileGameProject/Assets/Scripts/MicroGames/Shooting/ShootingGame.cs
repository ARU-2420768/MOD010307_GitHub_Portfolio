using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MicrogameCourse.Framework;
using MicrogameCourse.ProgressionFramework;
using System;

namespace MicrogameCourse.Microgames
{

    public sealed class ShootingGame : MicrogameBehaviour
    {


        [Header("Game Configuration")]
        [SerializeField] private string gameName = "Shooting";
        [SerializeField] private GameConfigurationData gameConfig;
        

        [Header("Scene References")]
        [SerializeField] private RectTransform playArea;
        [SerializeField] private RectTransform target;
        [SerializeField] private Image targetImage;
        [SerializeField] private TextMeshProUGUI progressText;
        [SerializeField] private TextMeshProUGUI feedbackText;
        [SerializeField] private GameObject playArea3D;


        private Camera mainCamera;
        public int score = 0;
        private MedalThresholdsData thresholds;

        private void Awake()
        {
            mainCamera = Camera.main;
            thresholds = gameConfig.medalThresholds;
            score = 5;
        }

        public override void Begin(MicrogameSession session)
        {
            base.Begin(session);
        }

        private void Update()
        {
            if (!IsRunning) return;
        }

        public override void OnPointerPressed()
        {
            ShotFired();
        }

        public void ShotFired()
        {
            Debug.Log("Shot Fired");
            ShowFeedback("Shots Completed", Color.green);
            UpdateProgress();
            CheckForCompletion();
            score++;
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


        private void CheckForCompletion()
        {
            if (score > gameConfig.medalThresholds.gold) 
            {
                MedalType medal = CalculateMedal();
                MedalManager.Instance.AwardMedal(gameName, medal);
                Win();
            }
        }


        public void ShowFeedback(string message, Color? colour = null)
        {
            feedbackText.color = colour ?? Color.white;
            feedbackText.text = message;
        }

        public void UpdateProgress()
        {
            progressText.gameObject.SetActive(true);
            progressText.text = $"Shots Success: {score} / Bronze 2 / Silver 4 / Gold 5";
        }


    }
}
