using System;
using UnityEngine;


namespace MicrogameCourse.Learning
{
    public class LifeCycleProbe : MonoBehaviour
    {

        private bool hasLoggedFirstUpdate;

        private void Awake()
        {
            Log("Awake");
        }

        private void OnEnable()
        {
            Log("OnEnable");            
        }

        private void Log(string eventName)
        {
            Debug.Log($"[frame {Time.frameCount}] {gameObject.name}: {eventName}");
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Log("Start"); 
        }

        // Update is called once per frame
        private void Update()
        {
            if (!hasLoggedFirstUpdate)
            {
                Log("first Update");
                hasLoggedFirstUpdate = true;
            }
        }

        private void OnDisable()
        {
            Log("OnDisable");            
        }

        private void OnDestroy()
        {
            Log("OnDestroy");    
        }
    }


}

