using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MicrogameCourse.Framework;

namespace MicrogameCourse.Microgames
{

    public sealed class SlalomGame : MicrogameBehaviour
    {
        [Header("Scene References")]
        [SerializeField] private RectTransform playArea;
        [SerializeField] private RectTransform target;
        [SerializeField] private Image targetImage;
        [SerializeField] private TextMeshProUGUI progressText;
        [SerializeField] private TextMeshProUGUI feedbackText;
        [SerializeField] private bool firstGate = true;


        private Camera mainCamera;

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        public override void Begin(MicrogameSession session)
        {
            base.Begin(session);
        }

        private void Update()
        {
            if (!IsRunning) return;
        }

    }
}