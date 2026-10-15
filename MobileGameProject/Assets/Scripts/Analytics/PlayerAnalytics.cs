using System;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UnityConsent;

public class PlayerAnalytics : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    async void Awake()
    {
        Debug.Log("Player Analytics Script");

        try
        {
            await UnityServices.InitializeAsync();
            Debug.Log("Analytics Initialised");
            SendPlayerStartEvent(1);
            Debug.Log("Player Analytics Start Event Sent");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Analytics Init Failed: {ex.Message}");
        }

        //await UnityServices.InitializeAsync();
        //AnalyticsService.Instance.StartDataCollection();
        //EndUserConsent.SetConsentState();
        //
    }

    public void SendPlayerStartEvent(int level)
    {
        Debug.Log("Player Analytics Start Event Received");
        CustomEvent playerStart = new("pong_start")
        {
            { "level", level },
            { "time", DateTime.UtcNow.ToString("o") }
        };
        AnalyticsService.Instance.RecordEvent(playerStart);

    }


    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

    }
}
