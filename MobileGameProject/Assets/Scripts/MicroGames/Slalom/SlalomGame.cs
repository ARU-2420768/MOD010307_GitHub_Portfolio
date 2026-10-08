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
            Instantiate(startGatesPrefab, new Vector3(0, -2, 0), Quaternion.identity);

            for (int i = 1; i < 1; i++)
            {
                float x = Random.value < 0.5f ? -1f : 1f;
//                Instantiate(gatesPrefab, new Vector3(1 * x, i, 0), Quaternion.identity);
                Instantiate(gatesPrefab, new Vector3(-1, i, 0), Quaternion.identity);
                //GameObject gate = Random.value < 0.5f ? Instantiate(gatesPrefab, playArea):Instantiate(fastGatesPrefab, playArea);

                //float x = Random.value < 0.5f ? -1f : 1f;

                //gate.transform.localPosition = new Vector3(-200 * x, -500 + (i * 250), 0);
            }

        }

        public void CreateNewGate()
        {
            float x = Random.value < 0.5f ? -1f : 1f;
            Instantiate(gatesPrefab, new Vector3(1 * x, 1, 0), Quaternion.identity);
            //GameObject gate = Instantiate(gatesPrefab, playArea);
            
            //gate.transform.localPosition = new Vector3(-200 * x, 750, 0);
        }

    }
}