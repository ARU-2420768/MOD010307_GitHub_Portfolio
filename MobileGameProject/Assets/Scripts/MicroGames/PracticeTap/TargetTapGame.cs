using System.Collections;
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
        [SerializeField] private bool decoy = false;

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
        [SerializeField] private Color decoyColour = Color.red;
        [SerializeField] private bool showReactionTime = true;
        [SerializeField] private int decoyPenalty = -2;

        private int score;
        private float reactionTimer;
        private float currentSize;

        public override void Begin(MicrogameSession session)
        {
            base.Begin(session);
            score = 0;
            SetTargetColour(safeColour);
            ShowFeedback("Go!", safeColour);
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
                ShowFeedback("Too slow!", Color.red);
                ShowNextTarget();
            }
        }

        public void TapTarget()
        {
            if (!IsRunning) return;

            int adjustScore = (decoy)? decoyPenalty: CalculatePoints(reactionTimer);

            AddScore(adjustScore);

            if (showReactionTime)
                ShowFeedback($"{GetRating(adjustScore)} - {reactionTimer:0.00}s score adjusted: {adjustScore:+#;-#;0}", Color.cadetBlue);
            else
                ShowFeedback("Hit!", Color.cadetBlue);

            if (score >= scoreToWin)
                Win();
            else
                ShowNextTarget();
        }



        private void ShowNextTarget()
        {
            SetTargetSize(startSize);
            target.anchoredPosition = GetRandomPosition(playArea, startSize);

            decoy = !decoy ? Random.value < 0.5f: decoy = false; 

            if (decoy)
            {
                SetTargetColour(decoyColour);
            }
            else
            {
                SetTargetColour(safeColour);
            }
            reactionTimer = 0f;
        }

        private int CalculatePoints(float reactionTime)
        {
            switch (reactionTimer)
            {
                case <= 0.5f:
                    return 3;
                case <= 1f:
                    return 2;
                default:                    
                    return 1;
            }
        }

        private string GetRating(int points)
        {
            switch (points)
            {
                case 3:
                    return "Perfect!";
                case 2:
                    return "Great!";
                case 1:
                    return "Good!";
                default:                    
                    return "You Hit The Decoy!";
            }
        }

        private void AddScore(int amount)
        {
            score += amount;
            score = (score> 0) ? score : 0 ;

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

        private void ShowFeedback(string message, Color? colour = null)
        {
            feedbackText.color = colour ?? Color.white;
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