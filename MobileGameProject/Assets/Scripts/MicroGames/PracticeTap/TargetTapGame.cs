using MicrogameCourse.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MicrogameCourse.Microgames
{
    /// <summary>Week 2 practice microgame: tap the shrinking target quickly to score points.</summary>
    public sealed class TargetTapGame : MicrogameBehaviour
    {
        [Header("Scene references")]
        [SerializeField] private RectTransform playArea;
        [SerializeField] private RectTransform target;
        [SerializeField] private Image targetImage;
        [SerializeField] private TextMeshProUGUI progressText;
        [SerializeField] private TextMeshProUGUI feedbackText;

        [Header("Rules")]
        [Tooltip("Points needed to win before the timer runs out.")]
        [SerializeField, Min(1)] private int scoreToWin = 10;

        [Header("Shrinking target")]
        [SerializeField, Min(10f)] private float startSize = 240f;
        [SerializeField, Min(10f)] private float minimumSize = 100f;
        [Tooltip("How many pixels the target loses from its width and height every second.")]
        [SerializeField, Range(0f, 300f)] private float shrinkPerSecond = 80f;

        [Header("Presentation")]
        [SerializeField] private Color safeColour = new Color(0.20f, 0.80f, 0.40f);
        [SerializeField] private bool showReactionTime = true;

        private int score;
        private float reactionTimer;
        private float currentSize;

        public override void Begin(MicrogameSession session)
        {
            base.Begin(session);
            score = 0;
            SetTargetColour(safeColour);
            ShowFeedback("Go!");
            UpdateProgress();
            ShowNextTarget();
        }

        private void Update()
        {
            if (!IsRunning) return;

            reactionTimer += Time.deltaTime;
            SetTargetSize(currentSize - shrinkPerSecond * Time.deltaTime);

            if (IsTargetTooSmall())
            {
                ShowFeedback("Too slow!");
                ShowNextTarget();
            }
        }

        public void TapTarget()
        {
            if (!IsRunning) return;

            AddScore(1);

            if (showReactionTime)
                ShowFeedback($"Hit! {reactionTimer:0.00}s");
            else
                ShowFeedback("Hit!");

            if (score >= scoreToWin)
                Win();
            else
                ShowNextTarget();
        }

        private void ShowNextTarget()
        {
            SetTargetSize(startSize);
            target.anchoredPosition = GetRandomPosition(playArea, startSize);
            reactionTimer = 0f;
        }

        private void AddScore(int amount)
        {
            score += amount;
            UpdateProgress();
        }

        private void SetTargetSize(float size)
        {
            currentSize = size;
            target.sizeDelta = new Vector2(size, size);
        }

        private void SetTargetColour(Color colour)
        {
            targetImage.color = colour;
        }

        private void ShowFeedback(string message)
        {
            feedbackText.text = message;
        }

        private void UpdateProgress()
        {
            progressText.text = $"Score: {score} / {scoreToWin}";
        }

        private bool IsTargetTooSmall()
        {
            return currentSize <= minimumSize;
        }

        private Vector2 GetRandomPosition(RectTransform area, float itemSize)
        {
            float maxX = (area.rect.width - itemSize) * 0.5f;
            float maxY = (area.rect.height - itemSize) * 0.5f;
            float x = Random.Range(-maxX, maxX);
            float y = Random.Range(-maxY, maxY);
            return new Vector2(x, y);
        }
    }
}