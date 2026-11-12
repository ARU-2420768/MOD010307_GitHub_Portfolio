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
        [SerializeField] private GameObject whiteCircle;
        [SerializeField] private LineRenderer aimLine;

        private bool isDragging;
        private Vector2 dragCurrentPosition;
        private Vector2 dragStartPosition;
        private Vector2 dragFinishPosition;
        
        private Camera mainCamera;
        private MedalThresholdsData thresholds;
        private Vector2 centerTarget;

        private void Awake()
        {
            mainCamera = Camera.main;
            thresholds = gameConfig.medalThresholds;
            // centerTarget is set to world position of white target
            centerTarget = whiteCircle.transform.position;

        }

        public override void Begin(MicrogameSession session)
        {
            base.Begin(session);
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            stone.OnStoneStopped += HandleStoneStopped;

            aimLine.positionCount = 2;

            aimLine.SetPosition(0, new Vector3(1, 2, -4));
            aimLine.SetPosition(1, new Vector3(0, 0, -4));

        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void HandleStoneStopped()
        {
            Debug.Log("Turn Complete");
            stone.IsAbleToPlay = false;
        }

        public override void OnPointerPressed()
        {
            if(stone.IsMoving)
                return;

            if(stone.IsAbleToPlay)
                return;


            dragStartPosition = Pointer.current.position.ReadValue();
            dragCurrentPosition = dragStartPosition;

            isDragging = true;
        }

        public override void OnDrag(Vector2 position)
        {
            if (!isDragging)
                return;

            dragCurrentPosition = position;

            Vector2 dragVector = dragStartPosition - dragCurrentPosition;

            Vector2 launchForce = dragVector * 0.05f;

            float predictionScale = 1f;

            Vector3 start = stone.transform.position;

            Vector3 end = start + new Vector3(launchForce.x,  launchForce.y, -5f) * predictionScale;

            aimLine.positionCount = 2;
            aimLine.SetPosition(0, start);
            aimLine.SetPosition(1, end);
        }


        public override void OnDragRelease(Vector2 position)
        {
            if (!isDragging)
                return;

            isDragging = false;
            
            dragFinishPosition = position;

            Vector2 dragVector = dragStartPosition - dragFinishPosition;

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