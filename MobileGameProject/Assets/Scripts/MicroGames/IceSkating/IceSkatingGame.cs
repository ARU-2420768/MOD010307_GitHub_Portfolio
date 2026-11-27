using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MicrogameCourse.Framework;
using MicrogameCourse.ProgressionFramework;
using System;
using UnityEngine.InputSystem;


namespace MicrogameCourse.Microgames
{

    public sealed class IceSkatingGame : MicrogameBehaviour
    {

        private static IceSkatingGame iceSkatingGame;
        public static IceSkatingGame Instance => iceSkatingGame;

        [Header("Medal Success Levels - Thresholds")]
        [SerializeField] private string gameName = "Ice Skating";
        [SerializeField] private GameConfigurationData gameConfig;

        [Header("Scene References")]
        [SerializeField] private RectTransform playArea;
        [SerializeField] private TextMeshProUGUI progressText;
        [SerializeField] private TextMeshProUGUI feedbackText;
        [SerializeField] private GameObject playArea3D;

        private MedalThresholdsData thresholds;
        private Camera mainCamera;
        public int score = 0;

        private void Awake()
        {
            mainCamera = Camera.main;
            thresholds = gameConfig.medalThresholds;
            iceSkatingGame = this;
        }

        void Start()
        {
            Debug.Log("ICE SKATING SCENE STARTED");
        }

        public override void Begin(MicrogameSession session)
        {
            base.Begin(session);
        }

        private void Update()
        {
            if (!IsRunning) return;
        }

        private void LogSkatingInput(string source, SkillAction action, Vector2? pos = null)
        {
            var rm = RoutineManager.Instance;
            string posText = pos.HasValue ? pos.Value.ToString() : "N/A";
            Debug.Log($"[InputDebug][IceSkatingGame] {source} -> {action} | pos={posText} | IsRunning={IsRunning} | rmExists={rm != null}");
        }

        public override void OnPointerPressed()
        {    
            LogSkatingInput("OnPointerPressed", SkillAction.Tap, Pointer.current?.position.ReadValue());
            RoutineManager.Instance.RegisterAction(SkillAction.Tap);
        }

        public override void OnDrag(Vector2 screenPosition)
        {
            LogSkatingInput("OnDrag", SkillAction.Swipe, screenPosition);
            //RoutineManager.Instance.RegisterAction(SkillAction.Swipe);
        }

        public override void OnDragRelease(Vector2 screenPosition)
        {
            LogSkatingInput("OnDragRelease", SkillAction.Swipe, screenPosition);
            RoutineManager.Instance?.RegisterAction(SkillAction.Swipe);
        }

        public override void OnTwoFingerTap()
        {
            LogSkatingInput("OnTwoFingerTap", SkillAction.TwoFingerTap);
            RoutineManager.Instance.RegisterAction(SkillAction.TwoFingerTap);
        }

        public override void OnTwoFingerDrag(Vector2 screenPosition)
        {
            LogSkatingInput("OnTwoFingerDrag", SkillAction.TwoFingerDrag, screenPosition);
            RoutineManager.Instance.RegisterAction(SkillAction.TwoFingerDrag);
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

        public void CheckForCompletion()
        {
            if(MedalManager.Instance == null)
                Debug.Log($"Medal Manager is created from the main menu - start from there first");

            MedalType medal = CalculateMedal();

            MedalManager.Instance.AwardMedal(gameName, medal);
            Win();
        }




    }
}
