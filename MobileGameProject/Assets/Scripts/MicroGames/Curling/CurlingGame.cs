using UnityEngine;
using TMPro;
using MicrogameCourse.Framework;
using MicrogameCourse.ProgressionFramework;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System;

namespace MicrogameCourse.Microgames
{

    public class CurlingGame : MicrogameBehaviour
    {


        [Header("Game Configuration")]
        [SerializeField] private string gameName = "Curling";
        [SerializeField] private GameConfigurationData gameConfig;

        [Header("Scene References")]
        [SerializeField] private RectTransform playArea;
        [SerializeField] private GameObject playArea3D;
        [SerializeField] private TextMeshProUGUI progressText;
        [SerializeField] private TextMeshProUGUI feedbackText;
        [SerializeField] private StoneManager stone;
        [SerializeField] private StoneManager ghostStone;
        [SerializeField] private GameObject whiteCircle;
        [SerializeField] private CalculateScore calculateScore;
        [SerializeField] private TextMeshProUGUI score;
        [SerializeField] private TextMeshProUGUI player;
        [SerializeField] private Transform directionArrow;
        [SerializeField] private LineRenderer aimLine;
        [SerializeField] private Transform arrowHead;
        [SerializeField] private StoneManager stonePrefab;
        [SerializeField] private Material stone1;
        [SerializeField] private Material stone2;


        [SerializeField] private float predictionMaxTime = 20f;
        [SerializeField] private int predictionMaxSteps = 2000;

        [SerializeField] Vector3 stoneStartPosition;
        [SerializeField] private int totalShots = 16;


        [SerializeField] private List<StoneManager> stones = new();


        private int playerTurn;

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
            if (calculateScore == null)
                calculateScore = GetComponent<CalculateScore>();
            
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
            //stones.Add(stone);

            //stone.OnStoneStopped += HandleStoneStopped;
            //ghostStone.OnStoneStopped += HandleStoneStopped;

            playerTurn = UnityEngine.Random.Range(1, 3);
            UpdatePlayerTurn();
            StartNewStone();
            //arrowHead.transform.localPosition = aimLine.transform.localPosition + aimLine.GetPosition(1) + new Vector3(0,0,-1);

        }

        public void StartNewStone()
        {
            stone = Instantiate(stonePrefab, playArea3D.transform);
            stone.GetComponent<Renderer>().material = playerTurn % 2 == 1 ? stone1 : stone2;
            stone.OnStoneStopped += HandleStoneStopped;
            stone.teamId = playerTurn;
            stones.Add(stone);
            directionArrow.gameObject.SetActive(true);
            progressText.text = $"Player {playerTurn}: Shot {MathF.Ceiling((float)stones.Count/2)} / {MathF.Ceiling((float)totalShots/2)}";
        }


        private void HandleStoneStopped()
        {

            Debug.Log($"Stone Stopped");
            //Debug.Log($"Main Stone Final Location: {stone.transform.localPosition}");
            stone.IsAbleToPlay = false;
            UpdateScoresAndTurn();
            if(stones.Count < totalShots)
            {
                StartNewStone();
            }
            else
            {
                EndGame();
            }
        }

        private void EndGame()
        {
            List<StoneManager> sortedStones = calculateScore.SortStones(stones);
            int teamID = calculateScore.WinningTeam(sortedStones[0]);
            progressText.text = $"Game Over - Player {teamID} Won!";
            MedalType medal = CalculateMedal();
            MedalManager.Instance.AwardMedal(gameName, medal);
            Debug.Log("End Game");
            Win();
            //base.IsRunning = false;
        }

        private MedalType CalculateMedal()
        {
            int score = CalculateScore();
            if(score >= thresholds.gold)
                return MedalType.Gold;
            if(score >= thresholds.silver)
                return MedalType.Silver;
            if(score >= thresholds.bronze)
                return MedalType.Bronze;
            return MedalType.Unranked;
        }

        public void UpdatePlayerTurn()
        {
            player.text = $"Turn: Player {playerTurn}";
        }

        public void UpdateScoresAndTurn()
        {
            List<StoneManager> sortedStones = calculateScore.SortStones(stones);
            int teamID = calculateScore.WinningTeam(sortedStones[0]);
            if(CalculateScore() == 0)
            {
                score.text = "No Score";    
            }
            else
            {
                score.text = $"Score: Player {teamID} - {CalculateScore()}";
            }
            playerTurn = (playerTurn % 2) + 1;
            UpdatePlayerTurn();
        }

        public int CalculateScore()
        {
            List<StoneManager> sortedStones = calculateScore.SortStones(stones);

            Debug.Log($"Winning Team ID: {calculateScore.WinningTeam(sortedStones[0])}");
            Debug.Log($"Score: {calculateScore.Calculate(sortedStones)}");
            return calculateScore.Calculate(sortedStones);

        }

        public void StartSimulation()
        {
            //ghostStone.transform.localPosition = stoneStartPosition;
            Vector3 vectorForce = new Vector3(0f, 4f, 0f);
            
            Vector3 predictedEnd = PredictEndPosition(vectorForce);
            Debug.Log($"Predicted End Local Pos: {predictedEnd}");

            //stone.Launch(vectorForce);
            //ghostStone.Launch(vectorForce);

        }


        private Vector3 PredictEndPosition(Vector3 impulse)
        {
            // Control unity physics
            previousMode = Physics.simulationMode;
            Physics.simulationMode = SimulationMode.Script;

            // save isKinematic of main stone to true - store old value
            // this is to stop the stone moving while we are simulating physics
            bool wasKinematic = stone.rb.isKinematic;
            stone.rb.isKinematic = true;

            // move ghost stone to start position
            ghostStone.rb.isKinematic = false;
            ghostStone.rb.linearVelocity = Vector3.zero;
            ghostStone.rb.angularVelocity = Vector3.zero;
            ghostStone.transform.localPosition = stoneStartPosition;
            Physics.SyncTransforms();

            // Apply impulse and step once so linearVelocity is > 0 before entering the while loop
            ghostStone.rb.AddForce(impulse, ForceMode.Impulse);
            Physics.Simulate(Time.fixedDeltaTime);

            float t = 0f;
            int steps = 0;
            float stopThreshold = 0.3f;


            //  loop through physics simulation until max time, max steps and the stopthreshold has been reached
            while(t < predictionMaxTime && steps < predictionMaxSteps && ghostStone.rb.linearVelocity.magnitude > stopThreshold)
            {
                Physics.Simulate(Time.fixedDeltaTime);
                t += Time.fixedDeltaTime;
                steps ++;
            }

            Vector3 end = ghostStone.transform.localPosition;
            Debug.Log($"Ghost Position final rest: {end} (after {steps} steps)");

            ghostStone.rb.linearVelocity = Vector3.zero;
            ghostStone.rb.angularVelocity = Vector3.zero;
            ghostStone.rb.isKinematic = true;

            stone.rb.isKinematic = wasKinematic;

            // return physics control to unity
            Physics.simulationMode = previousMode;
            return end;
        }



        // Update is called once per frame
        void Update()
        {

            
        }



        public override void OnPointerPressed()
        {
            Debug.Log("Curling OnPointerPressed");
            if(!hasGameStarted)
                return;
            if(stone == null)
                return;

            //Debug.Log("OnPointerPressed");
            if(stone.IsMoving)
                return;
            //Debug.Log("stone.IsMoving");
            if(stone.IsAbleToPlay)
                return;
            //Debug.Log("stone.IsAbleToPlay");



            dragStartPosition = Pointer.current.position.ReadValue();
            dragCurrentPosition = dragStartPosition;

            isDragging = true;
        }

        public override void OnDrag(Vector2 position)
        {

            Debug.Log("Curling Game OnDrag");
            if(!hasGameStarted)
                return;
            if (!isDragging)
                return;




            dragCurrentPosition = position;


            Vector2 dragVector = dragStartPosition - dragCurrentPosition;


            Color c = (dragVector.magnitude <= 160f)? Color.green: Color.red;

            aimLine.startColor = c;
            aimLine.endColor = c;

            // Stop the stone being aimed down the screen - 
            dragVector.y = Mathf.Max(0f, dragVector.y);

            // Match your real launch:
            Vector2 launchForce2D = dragVector * 0.05f;

            // Convert to X-Y plane impulse (Z = 0)
            Vector3 impulse = new Vector3(launchForce2D.x, launchForce2D.y, 0f);

            Vector3 start = stone.transform.position;

            aimLine.SetPosition(1, impulse);
            arrowHead.transform.localPosition = aimLine.transform.localPosition + aimLine.GetPosition(1) + new Vector3(0,0,-1);

            // make the arrowHead point in the same direction as the aimline
            Vector3 direction = (aimLine.GetPosition(1) - aimLine.GetPosition(0)).normalized;
            arrowHead.transform.up = direction;

            //Vector3 predictedEnd = PredictEndPosition(impulse);

            //predictionMarker.SetActive(true);
            //predictionMarker.transform.localPosition = predictedEnd;

            //aimLine.positionCount = 2;
            //aimLine.SetPosition(0, start);
            //aimLine.SetPosition(1, predictedEnd);
        }
        public override void OnDragRelease(Vector2 position)
        {
            Debug.Log("Curling Game OnDrag Release");

            if(!hasGameStarted)
                return;
            if (!isDragging)
                return;

            isDragging = false;
            dragFinishPosition = position;

            Vector2 dragVector = dragStartPosition - dragFinishPosition;

            float maxDrag = 5f;
            float currentMagnitude = dragVector.magnitude;
            if (currentMagnitude > 0f)
                {
                    float effectiveMagnitude = maxDrag * (1f - Mathf.Exp(-currentMagnitude / maxDrag));

                    // Apply the smoothed magnitude back to the original drag direction
                    //dragVector = dragVector.normalized * effectiveMagnitude;
                }



            // Must match OnDrag
            Vector2 launchForce2D = dragVector * 0.05f;
            //stone.Launch(new Vector2(0, 4));
            directionArrow.gameObject.SetActive(false);
            stone.Launch(launchForce2D);
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

        #region Inspector Validation

        private void OnValidate()
        {
            if (totalShots % 2 != 0)
            {
                totalShots++;
            }
        }
        #endregion

    }
}