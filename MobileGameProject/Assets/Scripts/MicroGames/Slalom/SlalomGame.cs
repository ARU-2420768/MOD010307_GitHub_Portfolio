using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MicrogameCourse.Framework;
using MicrogameCourse.ProgressionFramework;
using System;
using UnityEngine.InputSystem;
using System.Collections;

namespace MicrogameCourse.Microgames
{
    // Created using SPT - 

    public sealed class SlalomGame : MicrogameBehaviour
    {

        [Header("Medal Success Levels - Thresholds")]
        [SerializeField] private string gameName = "Slalom";
        [SerializeField] private GameConfigurationData gameConfig;

        [Header("Scene References")]
        [SerializeField] private RectTransform playArea;
        [SerializeField] private TextMeshProUGUI progressText;
        [SerializeField] private TextMeshProUGUI feedbackText;
        [SerializeField] private GameObject playArea3D;

        [Header("Gates Prefabs")]
        [SerializeField] public Vector3 lastGateSpawnLocation = new Vector3(0f,0.45f,0f);
        [SerializeField] public float gateMoveSpeed = 1f;
        [SerializeField] public float gateDefaultMoveSpeed = 1f;
        [SerializeField] public float gateMaxMoveSpeed = 1.5f;
        [SerializeField] public float gateMinMoveSpeed = 0.5f;
        [SerializeField] private ParticleSystem gatePassEffect;
        
        private Camera mainCamera;
        public int score,gatesCleared;
        private MedalThresholdsData thresholds;
        public bool IsGameRunning;
        private bool isOffPiste;
        private Coroutine vibrationCoroutine;

        private void Awake()
        {
            mainCamera = Camera.main;
            thresholds = gameConfig.medalThresholds;
        }

        //public override void Begin(MicrogameSession session)
        //{
        //    base.Begin(session);
        //    IsRunning = true;
        //}

        public override void End()
        {
            base.End();
            MedalType medal = CalculateMedal();
            MedalManager.Instance.AwardMedal(gameName, medal);
            if (medal != MedalType.Unranked)
                Win();
            else
                Lose();
            base.IsRunning = false;
        }

        private void Start()
        {
        }

        private void Update()
        {
            if (!IsRunning) return;
            lastGateSpawnLocation += Mathf.Clamp(gateMoveSpeed,gateMinMoveSpeed,gateMaxMoveSpeed) * Time.deltaTime * Vector3.up;
        }

        public void GateTriggered(GameObject gate, string gateState)
        {
            if(gateState=="GateSuccess") GatePassed(gate);
            if(gateState=="GateFailure") GateMissed();

        }

        public void SlowdownTriggered(string areaState)
        {
            if(areaState=="Piste") Piste();
            if(areaState=="OffPiste") OffPiste();
        }
        public void Piste()
        {
            isOffPiste = false;
            gateMoveSpeed = gateDefaultMoveSpeed;
            if (vibrationCoroutine != null)
            {
                StopCoroutine(vibrationCoroutine);
                vibrationCoroutine = null;
            }
        }

        public void OffPiste()
        {

            if (isOffPiste) return;
            isOffPiste = true;
            gateMoveSpeed = gateMinMoveSpeed;
            vibrationCoroutine = StartCoroutine(VibrateWhileOffPiste());

        }

        private IEnumerator VibrateWhileOffPiste()
        {
            while (isOffPiste)
            {
                Handheld.Vibrate();
                yield return new WaitForSeconds(0.1f);
            }
        }

        public void GateMissed()
        {            
            UpdateProgress();
            CheckForCompletion();
        }
        public void GatePassed(GameObject gate)
        {
            
            gatesCleared++;

            if (gatePassEffect != null)
            {
                ParticleSystem effect = Instantiate(gatePassEffect, gate.transform.position, Quaternion.identity);
                Destroy(effect.gameObject, effect.main.duration + effect.main.startLifetime.constantMax);
            }

            score++;
            UpdateProgress();
            CheckForCompletion();
        }

        private void CheckForCompletion()
        {
            if (score > gameConfig.medalThresholds.gold) 
            {
                MedalType medal = CalculateMedal();
                MedalManager.Instance.AwardMedal(gameName, medal);
                Win();
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

        private int CalculateTargetMedal()
        {
            if(score >= thresholds.silver)
                return thresholds.gold;
            if(score >= thresholds.bronze)
                return thresholds.silver;
            return thresholds.bronze;
        }

        private MedalType CalculateTargetMedalType()
        {
            if(score >= thresholds.silver)
                return MedalType.Gold;
            if(score >= thresholds.bronze)
                return MedalType.Silver;
            return MedalType.Bronze;
        }

        public void CreateNewGate()
        {
            if (!IsRunning) return;
        }

        public void ShowFeedback(string message, Color? colour = null)
        {
            //feedbackText.color = colour ?? Color.white;
            //feedbackText.text = message;
        }

        public void UpdateProgress()
        {
            progressText.gameObject.SetActive(true);
            int medal = CalculateTargetMedal();
            MedalType medalTarget = CalculateTargetMedalType();
            progressText.text = $"Progress\nNext Medal Target: {medalTarget}\n{score} / {medal}";
        }

    }
}