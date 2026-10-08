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
        [SerializeField] private GameObject playArea3D;


        [Header("Gates Prefabs")]
        [SerializeField] private GameObject startGatesPrefab;
        [SerializeField] private GameObject gatesPrefab;
        [SerializeField] private GameObject fastGatesPrefab;


        private Camera mainCamera;
        private GameObject newPrefab;

        private readonly float[] Kitzbuhel = { 0f, -1f, 0f, -1f, 1f, 1f, 0f, 1f, -1, 0 };
        private int gateNumber = 0;

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
            Instantiate(startGatesPrefab, new Vector3(Kitzbuhel[gateNumber], -2.5f + gateNumber, 0), Quaternion.identity);
            gateNumber++;
            for (int i = 1; i < 4; i++)
            {
                Instantiate(startGatesPrefab, new Vector3(Kitzbuhel[gateNumber], -2.5f + (2 * gateNumber), 0), Quaternion.identity);
                gateNumber++;
            }
        }

        public void CreateNewGate()
        {
            if (!IsRunning) return;
            Instantiate(startGatesPrefab, new Vector3(Kitzbuhel[gateNumber % Kitzbuhel.Length] , 3.5f, 0), Quaternion.identity);
            gateNumber++;
        }

    }
}