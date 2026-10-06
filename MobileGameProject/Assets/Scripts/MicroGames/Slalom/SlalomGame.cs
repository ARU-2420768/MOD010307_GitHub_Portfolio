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


        [Header("Gates Prefabs")]
        [SerializeField] private GameObject startGatesPrefab;
        [SerializeField] private GameObject gatesPrefab;


        private Camera mainCamera;

        private void Awake()
        {
            mainCamera = Camera.main;
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

        private void InitialiseFirstNewGates()
        {
            for (int i = 0; i < 1; i++)
            {
                GameObject gate = Instantiate(gatesPrefab, playArea);

                float x = (i % 2 == 0) ? -1f : 1f;

                gate.transform.localPosition = new Vector3(-200 * x, 0, 0);
            }

        }

    }
}