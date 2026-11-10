using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MicrogameCourse.Framework;
using MicrogameCourse.ProgressionFramework;
using UnityEngine.InputSystem;


namespace MicrogameCourse.Microgames
{

    public sealed class ShootingGame : MicrogameBehaviour
    {


        [Header("Game Configuration")]
        [SerializeField] private string gameName = "Shooting";
        [SerializeField] private GameConfigurationData gameConfig;
        [SerializeField] private WindManager windManager;

        [Header("Scene References")]
        [SerializeField] private RectTransform playArea;
        [SerializeField] private RectTransform target;
        [SerializeField] private Image targetImage;
        [SerializeField] private TextMeshProUGUI progressText;
        [SerializeField] private TextMeshProUGUI feedbackText;
        [SerializeField] private GameObject playArea3D;
        [SerializeField] private GameObject bulletHole;
        [SerializeField] private GameObject HitIndicatorPlate;
        [SerializeField] private GameObject CrossHair;
        [SerializeField] private Camera TargetZoomCamera;
        [SerializeField] private Material greenMaterial;


        private Camera mainCamera;
        private Vector3 TargetZoomCameraOriginalPosition;
        public int shotsFired = 0;
        public int maxShotsAllowed = 5;
        public int score = 0;
        private MedalThresholdsData thresholds;

        private void Awake()
        {
            mainCamera = Camera.main;
            thresholds = gameConfig.medalThresholds;
        }

        private void Start()
        {
            TargetZoomCameraOriginalPosition = TargetZoomCamera.transform.localPosition;
        }

        public override void Begin(MicrogameSession session)
        {
            base.Begin(session);
        }

        private void Update()
        {
            if (!IsRunning) return;
            if (score > 0)
            {
                //TargetZoomCamera.transform.localPosition = TargetZoomCameraOriginalPosition + Random.insideUnitSphere * 5f;
                //TargetZoomCamera.GetComponent<ZoomCameraManager>().Shake(0.1f, score);
            }
        }

        public override void OnPointerPressed()
        {
            //ShotFired();
        }


        public override void OnDrag(Vector2 delta)
        {
            Debug.Log("ShootingGame.OnDrag called");
        }

        public override void OnDirectionalInput(Vector2 direction, GameObject button)
        {
            switch(button.tag)
            {
                case "Turret":
                    MoveWindDirectionTargetAdjustment(direction, button);
                    break;
                case "TargetDirection":
                    MoveTarget(direction, button);
                    break;
            }
        }

        public void MoveTarget(Vector2 direction, GameObject button)
        {
            Debug.Log($"Target Direction Move Player: {direction}");
        }


        public override void OnStopMovement()
        {
            windManager.StopMovement();
        }

        public void MoveWindDirectionTargetAdjustment(Vector2 direction, GameObject button)
        {
            windManager.StartMovement(direction);
        }


        public override void OnButtonTap(GameObject button)
        {
            switch(button.tag)
            {
                case "Shoot":
                    ShotFiredInitiated(button);
                    break;
                case "Action":
                    ActionButtonInitiated(button);
                    break;
            }
        }


        public void ShotFiredInitiated(GameObject button)
        {
            Vector2 offset = Vector2.zero;
            GameObject shot = Instantiate(bulletHole, playArea3D.transform);
            shot.transform.position = CrossHair.transform.position + new Vector3(0, 0, shot.transform.position.z);
            // Add shot offset if scope isn't calibrated
            if(!windManager.IsScopeCalibrated)
            {
                offset = (Vector2)windManager.WindIndicatorActual.transform.localPosition - (Vector2)windManager.WindIndicatorAdjusted.transform.localPosition;
                shot.transform.position = CrossHair.transform.position + new Vector3(0, 0, shot.transform.position.z) + (Vector3)offset;
            }

            Vector3 rayDirection =
                TargetZoomCamera.transform.forward +
                TargetZoomCamera.transform.right * offset.x +
                TargetZoomCamera.transform.up * offset.y;

            Ray ray = new Ray(TargetZoomCamera.transform.position, rayDirection.normalized);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000f))
            {
                if (hit.collider.CompareTag("Target"))
                {
                    Debug.Log("Hit Target - now cover target with green HitIndicatorPlate");
                    hit.collider.transform.localScale = hit.collider.transform.localScale * 1.1f;
                    hit.collider.GetComponent<Renderer>().material = greenMaterial;
                    score++;
                }
            }

            shotsFired++;
            ShotFired();
        }

        public void ActionButtonInitiated(GameObject button)
        {
            Debug.Log("ShootingGame.OnButtonTap called - ActionButtonInitiated");

        }

        public void ShotFired()
        {
            Debug.Log($"Shots Fired: {shotsFired} / {maxShotsAllowed}");
            ShowFeedback($"Shots Fired: {shotsFired} / {maxShotsAllowed}", Color.green);
            //ShotHitTarget();
            UpdateProgress();
            CheckForCompletion();
        }

        public void ShotHitTarget()
        {
            Ray ray = new Ray(TargetZoomCamera.transform.position, TargetZoomCamera.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000f))
            {
                if (hit.collider.CompareTag("Target"))
                {
                    Debug.Log("Hit Target - now cover target with green HitIndicatorPlate");
                    hit.collider.transform.localScale = hit.collider.transform.localScale * 1.1f;
                    hit.collider.GetComponent<Renderer>().material = greenMaterial;
                    score++;
                }
            }
        }


        private MedalType CalculateMedal()
        {
            if(score >= thresholds.gold)
                return MedalType.Gold;
            if(score >= thresholds.silver)
                return MedalType.Silver;
            if(score >= thresholds.bronze)
                return MedalType.Bronze;
            return MedalType.Unranked;
        }


        private void CheckForCompletion()
        {
            if (shotsFired >= maxShotsAllowed) 
            {
                MedalType medal = CalculateMedal();
                Debug.Log($"Medal: {medal}");
                MedalManager.Instance.AwardMedal(gameName, medal);
                Win();
            }
        }


        public void ShowFeedback(string message, Color? colour = null)
        {
            feedbackText.color = colour ?? Color.white;
            feedbackText.text = message;
        }

        public void UpdateProgress()
        {
            progressText.gameObject.SetActive(true);
            progressText.text = $"Shots Success: {score} / Bronze 2 / Silver 4 / Gold 5";
        }


    }
}
