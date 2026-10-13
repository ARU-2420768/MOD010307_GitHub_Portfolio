using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MicrogameCourse.Framework;
using Unity.VisualScripting.Dependencies.Sqlite;
using MicrogameCourse.ProgressionFramework;
using System;

namespace MicrogameCourse.Microgames
{

    public sealed class ShootingGame : MicrogameBehaviour
    {

        [Header("Medal Success Levels - Thresholds")]
        [SerializeField] private string gameName = "Shooting";
        [SerializeField] private MedalThresholdsData thresholds;

        [Header("Scene References")]
        [SerializeField] private RectTransform playArea;
        [SerializeField] private RectTransform target;
        [SerializeField] private Image targetImage;
        [SerializeField] private TextMeshProUGUI progressText;
        [SerializeField] private TextMeshProUGUI feedbackText;
        [SerializeField] private GameObject playArea3D;


        private Camera mainCamera;
        public int score = 0;

        private void Awake()
        {
            mainCamera = Camera.main;
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
            //Debug.Log("Gate Passed");
            ShowFeedback("Shots Completed", Color.green);
            UpdateProgress();
            CheckForCompletion();
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
            MedalType medal = CalculateMedal();
            MedalManager.Instance.AwardMedal(gameName, medal);
            Win();
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
