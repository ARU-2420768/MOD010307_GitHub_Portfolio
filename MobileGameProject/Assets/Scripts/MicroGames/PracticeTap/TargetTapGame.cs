using MicrogameCourse.Framework;
using TMPro;
using Unity.VisualScripting;
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
        [SerializeField, Min(1)] private int tapsToWin = 5;

        [Header("Shrinking target")]    
        [SerializeField] private float startSize = 240f;
        [SerializeField] private float minimumSize = 100f;
        [Tooltip("How many pixels the target loses from its width and height every second.")]
        [SerializeField, Range(0f, 300f)] private float shrinkPerSecond  = 80f;

        [Header("Presentation")]
        [SerializeField] private Color safeColour = new Color(0.20f, 0.80f, 0.40f);
        [SerializeField] private bool showReactionTime = true;


        private int score;
        private float reactionTimer;
        private int tapsRemaining;
        private float currentSize;

        public override void Begin(MicrogameSession session)
        {
            base.Begin(session);
            score = 0;
            targetImage.color = safeColour;
            //tapsRemaining = tapsToWin;
            feedbackText.text = "Go!";
            UpdateProgress();
            MoveTarget();
        }

        private void Update()
        {
            if (!IsRunning)
            {
                return;
            }

            reactionTimer += Time.deltaTime;
            currentSize -= shrinkPerSecond * Time.deltaTime;
            target.sizeDelta = new Vector2(currentSize, currentSize);

            if(currentSize <= minimumSize)
            {
                feedbackText.text = "Too slow";
                MoveTarget();
            }

        }


        public void TapTarget()
        {
            if (!IsRunning)
            {
                return;
            }

            score = score + 1;
            //tapsRemaining--;
            UpdateProgress();



            if (showReactionTime)
                feedbackText.text = $"Hit! {reactionTimer:0.00}s";
            else
                feedbackText.text = "Hit!";

            if (score >= scoreToWin)
            {
                Win();
            }
            else
            {
                MoveTarget();
            }
        }

        private void UpdateProgress()
        {
            progressText.text = $"Score: {score} / {scoreToWin}";
        }

        private void MoveTarget()
        {
            currentSize = startSize;
            target.sizeDelta = new Vector2(currentSize, currentSize);
            reactionTimer = 0f;

            float maxX = (playArea.rect.width - target.rect.width) * 0.5f;
            float maxY = (playArea.rect.height - target.rect.height) * 0.5f;
            float x = Random.Range(-maxX, maxX);
            float y = Random.Range(-maxY, maxY);
            target.anchoredPosition = new Vector2(x,y);
        }


    }

}