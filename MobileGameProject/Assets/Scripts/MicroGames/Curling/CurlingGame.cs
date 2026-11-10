using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MicrogameCourse.Framework;
using MicrogameCourse.ProgressionFramework;
using UnityEngine.InputSystem;


namespace MicrogameCourse.Microgames
{

    public class CurlingGame : MicrogameBehaviour
    {


        [Header("Game Configuration")]
        [SerializeField] private GameConfigurationData gameConfig;

        [Header("Scene References")]
        [SerializeField] private RectTransform playArea;
        [SerializeField] private TextMeshProUGUI progressText;
        [SerializeField] private TextMeshProUGUI feedbackText;

        private Camera mainCamera;
        private MedalThresholdsData thresholds;
        private void Awake()
        {
            mainCamera = Camera.main;
            thresholds = gameConfig.medalThresholds;
        }

        public override void Begin(MicrogameSession session)
        {
            base.Begin(session);
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}