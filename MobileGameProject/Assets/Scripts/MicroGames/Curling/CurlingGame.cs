using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MicrogameCourse.Framework;
using MicrogameCourse.ProgressionFramework;
using UnityEngine.InputSystem;
using UnityEditor.Build;


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

        [SerializeField] private StoneManager stone;

        private bool isDragging;
        private Vector2 dragCurrentPosition;
        private Vector2 dragStartPosition;
        private Vector2 dragFinishPosition;
        

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

        public override void OnPointerPressed()
        {
            if(stone.IsMoving)
                return;

            dragStartPosition = Pointer.current.position.ReadValue();
            dragCurrentPosition = dragStartPosition;

            isDragging = true;
            Debug.Log("Drag Started");
        }

        public override void OnDrag(Vector2 position)
        {
            if (!isDragging)
                return;

            dragCurrentPosition = position;
        }

        public override void OnDragRelease(Vector2 position)
        {
            if (!isDragging)
                return;

            isDragging = false;
            
            dragFinishPosition = position;

            Vector2 dragVector = dragStartPosition - dragFinishPosition;

            Debug.Log($"Launch Force: {dragVector}");

            stone.Launch(dragVector * 0.05f);
        }
 


        public override void OnTwoFingerDrag(Vector2 position)
        {
            //Debug.Log("Curling Game - OnTwoFingerDrag");
        }

        public override void OnTwoFingerDragRelease(Vector2 position)
        {
            //Debug.Log("Curling Game - OnTwoFingerDragRelease");
        }

    }
}