using UnityEngine;
using TMPro;
using MicrogameCourse.Framework;
using MicrogameCourse.ProgressionFramework;
using UnityEngine.InputSystem;
using System.Collections.Generic;

namespace MicrogameCourse.Microgames
{

    public class CurlingGame : MicrogameBehaviour
    {


        [Header("Game Configuration")]
        [SerializeField] private GameConfigurationData gameConfig;

        [Header("Scene References")]
        [SerializeField] private RectTransform playArea;
        [SerializeField] private GameObject playArea3D;
        [SerializeField] private TextMeshProUGUI progressText;
        [SerializeField] private TextMeshProUGUI feedbackText;
        [SerializeField] private StoneManager stone;
        [SerializeField] private GameObject whiteCircle;
        [SerializeField] private LineRenderer aimLine;
        [SerializeField] private Rigidbody ghostStonePrefab;
        [SerializeField] private GameObject predictionMarker;

        [SerializeField] private float predictionMaxTime = 20f;
        [SerializeField] private float predictionStopVelocity = 0.05f;
        [SerializeField] private int predictionMaxSteps = 2000;

        private readonly List<StoneManager> stones = new();

        private Rigidbody ghost;
        private SimulationMode previousMode;

        private bool isDragging;
        private Vector2 dragCurrentPosition;
        private Vector2 dragStartPosition;
        private Vector2 dragFinishPosition;
        
        private Camera mainCamera;
        private MedalThresholdsData thresholds;
        private Vector2 centerTarget;

        public bool hasGameStarted = false;

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

        public void StartGame()
        {
            hasGameStarted = true;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            stones.Add(stone);

            stone.OnStoneStopped += HandleStoneStopped;



        }

public void StartSimulation()
{
    // Reset real stone
    stone.rb.linearVelocity = Vector3.zero;
    stone.rb.angularVelocity = Vector3.zero;
    stone.rb.Sleep();

    // Create ghost only once
    if (ghost == null)
    {
        ghost = Instantiate(ghostStonePrefab, playArea3D.transform);
        ghost.gameObject.name = "PredictionGhost";
        // Optional but recommended: remove StoneManager from ghost at runtime
        var sm = ghost.GetComponent<StoneManager>();
        if (sm != null) Destroy(sm);
    }

    ghost.gameObject.SetActive(true);

    Vector3 predicted = PredictEndPosition(
        stone.transform.position,
        new Vector3(0f, 3f, 0f)
    );

    Debug.Log($"Predicted = {predicted}");

    // Launch the real stone so we can compare
    stone.Launch(new Vector2(0f, 3f));
}

        public void StartSimulation1()
        {
            stone.rb.linearVelocity = Vector3.zero;
            stone.rb.angularVelocity = Vector3.zero;
            stone.rb.Sleep(); // puts it into sleep mode if supported by physics settings

            ghost = Instantiate(ghostStonePrefab, playArea3D.transform);
            ghost.gameObject.SetActive(false);

            Vector3 predicted =
                PredictEndPosition(
                    stone.transform.position,
                    new Vector3(0, 4, 0));

            Debug.Log($"Predicted = {predicted}");
            stone.Launch(new Vector2(0, 10));
        }

        // Update is called once per frame
        void Update()
        {

            
        }

        private void HandleStoneStopped()
        {
            Debug.Log($"Actual = {stone.transform.position}");
            stone.IsAbleToPlay = false;
        }

        public override void OnPointerPressed()
        {
            if(!hasGameStarted)
                return;

            Debug.Log("OnPointerPressed");
            if(stone.IsMoving)
                return;
            Debug.Log("stone.IsMoving");
            if(stone.IsAbleToPlay)
                return;
            Debug.Log("stone.IsAbleToPlay");



            dragStartPosition = Pointer.current.position.ReadValue();
            dragCurrentPosition = dragStartPosition;

            isDragging = true;
        }

        public override void OnDrag(Vector2 position)
        {
            if(!hasGameStarted)
                return;
            if (!isDragging)
                return;

            dragCurrentPosition = position;

            Vector2 dragVector = dragStartPosition - dragCurrentPosition;

            // Match your real launch:
            Vector2 launchForce2D = dragVector * 0.05f;

            // Convert to X-Y plane impulse (Z = 0)
            Vector3 impulse = new Vector3(launchForce2D.x, launchForce2D.y, 0f);

            Vector3 start = stone.transform.position;

            //Vector3 predictedEnd = PredictEndPosition(start,impulse);

            //predictionMarker.SetActive(true);
            //predictionMarker.transform.position = predictedEnd;

            //aimLine.positionCount = 2;
            //aimLine.SetPosition(0, start);
            //aimLine.SetPosition(1, predictedEnd);
        }
        public override void OnDragRelease(Vector2 position)
        {
            if(!hasGameStarted)
                return;
            if (!isDragging)
                return;

            isDragging = false;
            dragFinishPosition = position;

            Vector2 dragVector = dragStartPosition - dragFinishPosition;

            // Must match OnDrag
            Vector2 launchForce2D = dragVector * 0.05f;
            //stone.Launch(new Vector2(0, 4));
            //stone.Launch(launchForce2D);
        }

        private Vector3 PredictEndPosition(Vector3 startPosition, Vector3 impulse)
        {
            previousMode = Physics.simulationMode;
            Physics.simulationMode = SimulationMode.Script;

            // Freeze real stone during prediction
            bool wasKinematic = stone.rb.isKinematic;
            stone.rb.isKinematic = true;

            // Prepare ghost
            ghost.isKinematic = true;
            ghost.position = startPosition;
            ghost.rotation = stone.transform.rotation;
            Physics.SyncTransforms();

            ghost.isKinematic = false;
            ghost.linearVelocity = Vector3.zero;
            ghost.angularVelocity = Vector3.zero;
            ghost.WakeUp();

            // mass is 1 → setting velocity = Impulse
            ghost.linearVelocity = impulse;

            Debug.Log($"Ghost velocity after setup: {ghost.linearVelocity}");

            float t = 0f;
            int steps = 0;
            const float stopThreshold = 0.3f; // same as StoneManager

            while (t < predictionMaxTime &&
                steps < predictionMaxSteps &&
                ghost.linearVelocity.magnitude > stopThreshold)
            {
                Physics.Simulate(Time.fixedDeltaTime);
                t += Time.fixedDeltaTime;
                steps++;
            }

            Vector3 end = ghost.position;
            Debug.Log($"Steps: {steps} | Final velocity: {ghost.linearVelocity.magnitude:F3} | Predicted: {end}");

            // Restore
            stone.rb.isKinematic = wasKinematic;
            Physics.simulationMode = previousMode;

            return end;
        }

        private Vector3 PredictEndPosition_close(Vector3 startPosition, Vector3 impulse)
        {
            Vector3 pos = startPosition;
            Vector3 vel = impulse;               // mass == 1
            float dt = Time.fixedDeltaTime;
            float damping = ghost.linearDamping; // 0.1
            float stopThreshold = 0.3f;
            float maxTime = 20f;
            float t = 0f;

            while (t < maxTime && vel.magnitude > stopThreshold)
            {
                // Simple Euler integration with linear damping
                vel *= Mathf.Clamp01(1f - damping * dt);
                pos += vel * dt;
                t += dt;
            }

            return pos;
        }

        public override void OnTwoFingerDrag(Vector2 position)
        {
            //Debug.Log("Curling Game - OnTwoFingerDrag");
        }

        public override void OnTwoFingerDragRelease(Vector2 position)
        {
            //Debug.Log("Curling Game - OnTwoFingerDragRelease");
        }

        #region Helper Methods

        private bool AreAllStonesStopped()
        {
            foreach (StoneManager stone in stones)
            {
                if (stone.IsMoving)
                {
                    return false;
                }
            }
            return true;
        }

        #endregion


    }
}