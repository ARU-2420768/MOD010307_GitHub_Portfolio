using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using JetBrains.Annotations;


namespace MicrogameCourse.Framework
{
    public sealed class MicrogameSession : MonoBehaviour
    {
        private enum Phase {Ready, Playing, Result}

        [SerializeField] private MicrogameBehaviour game;
        [SerializeField] private GameObject readyPanel;
        [SerializeField] private GameObject playArea;

        [SerializeField] private GameObject playArea3D;
        [SerializeField] private GameObject resultPanel;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI resultText;
        [SerializeField] private TextMeshProUGUI countdownText;
        [SerializeField, Min(1f)] private float durationSeconds = 10f;
        [SerializeField] private float durationCountdownStep = 0.1f;
        [SerializeField] public bool isTimerStarted = false;

        private Phase currentPhase;
        private float remainingSeconds;

        private void Awake()
        {
            currentPhase = Phase.Ready;
            readyPanel.SetActive(true);
            playArea.SetActive(false);
            playArea3D.SetActive(false);
            resultPanel.SetActive(false);
            timerText.text = string.Empty;
        }

        public void StartGame()
        {
            if(currentPhase != Phase.Ready || game == null)
            {
                return;
            }
            remainingSeconds = durationSeconds;
            readyPanel.SetActive(false);
            playArea.SetActive(true);
            playArea3D.SetActive(true);
            resultPanel.SetActive(false);
            
            StartTimer();
            
            game.Begin(this);
        }


        public void StartTimer()
        {
            if(countdownText == null)
            {
                Debug.Log("No countdown text set in scene");
                return;    
            } 
            StartCoroutine(CountdownCoroutine());
        }

        private IEnumerator CountdownCoroutine()
        {
            string[] countdown = {"3", "2", "1", "GO!!!"};

            foreach(string item in countdown)
            {
                countdownText.text = item;
                yield return new WaitForSeconds(durationCountdownStep);
            }
            countdownText.text = "";
            currentPhase = Phase.Playing;
            isTimerStarted = true;
        }

        private void Update()
        {
            if(currentPhase != Phase.Playing)
            {
                return;
            }
            remainingSeconds = Mathf.Max(0f, remainingSeconds - Time.deltaTime);
            ShowTime();
            if(remainingSeconds <= 0f) Finish(false);

        }

        public void Finish(bool won)
        {
            if(currentPhase != Phase.Playing)
            {
                return;
            }

            currentPhase = Phase.Result;
            game.End();
            playArea.SetActive(false);
            resultPanel.SetActive(true);
            resultText.text = "You" + (won ? " Win!" : "r time is up!");
        }

        private void ShowTime()
        {
            timerText.text = $"Time : {remainingSeconds: 0.0}";
        }
    }
}