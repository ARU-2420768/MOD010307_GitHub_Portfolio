using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MicrogameCourse.Framework;
using MicrogameCourse.ProgressionFramework;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace MicrogameCourse.Microgames
{
    public class GatesPrefabManager : MonoBehaviour
    {
        [SerializeField] private GateDefinition[] gatesPrefabs;
        [SerializeField] private GateDefinition gatesPrefabsSSS;
        [SerializeField] private GateDefinition gatesPrefabsSS;
        [SerializeField] private GateDefinition gatesPrefabsSC;
        [SerializeField] private GateDefinition gatesPrefabsTC;
        [SerializeField] private GateDefinition gatesPrefabsTCR;
        [SerializeField] private GateDefinition gatesPrefabsTS;
        [SerializeField] private GateDefinition gatesPrefabsSFS;

        [SerializeField] private int location = 7;
        [SerializeField] private Transform playArea;
        //[SerializeField] private Vector3 lastGateSpawnLocation = new Vector3(0f,0.45f,0f);
        [SerializeField] private Vector3 newGateSpawnLocation;
        [SerializeField] private float laneWidth = 0.06f;

        private SlalomGame game;
        private int biasDirection = 1;

        private void Awake()
        {
            game = FindFirstObjectByType<SlalomGame>();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            // Create Start Game
            CreateSpecificGateFromPrefab(gatesPrefabsSSS);
            CreateSpecificGateFromPrefab(gatesPrefabsSSS);

            for(int i = 0; i < 30; i++){
                CreateNewGateFromPrefab();
            }



        }

        private void CreateSpecificGateFromPrefab(GateDefinition gatePrefabDefinition)
        {
            GameObject gate = Instantiate(gatePrefabDefinition.prefab, playArea);

            int direction = Random.value < 0.5f ? -1 : 1;
            float yRotation = direction == 1 ? 0f : 180f;

            location += direction;

            gate.transform.localPosition = game.lastGateSpawnLocation;
            gate.transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);

            float horizontalShift = (gatePrefabDefinition.nextOffset * laneWidth * direction);
            game.lastGateSpawnLocation += new Vector3(horizontalShift,-0.1f,0f);
        }

        // Update is called once per frame
        void Update()
        {

            
        }

        private void CreateNewGateFromPrefab()
        {
            string enumName = $"Position{location}";
            List<GateDefinition> matchedConnections = new();

            // Convert location to binary
            GateConnection requiredConnection = (GateConnection)(1 << (location - 1));

            foreach(var gatePrefab in gatesPrefabs)
            {
                if (gatePrefab.allowedLocations.HasFlag(requiredConnection))
                {
                    matchedConnections.Add(gatePrefab);
                }
            }



            if (matchedConnections.Count > 0)
            {
                GateDefinition selectedGate = matchedConnections[Random.Range(0, matchedConnections.Count)];

                //int biasDirection = Random.value < 0.5f ? -1 : 1;
                float biasStrength = Mathf.Lerp(0.6f, 0.95f, Mathf.Abs(location - 7f) / 6f);

                if (location >= 12)
                {
                    biasDirection = -1;
                }
                else if (location <= 2)
                {
                    biasDirection = 1;
                }

                int direction = Random.value < biasStrength ? biasDirection : -biasDirection;
                //int direction = Random.value < 0.5f ? -1 : 1;
                


                float yRotation = direction == 1 ? 0f : 180f;

                GameObject gate = Instantiate(selectedGate.prefab, playArea);
                gate.transform.localPosition = game.lastGateSpawnLocation;
                gate.transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);
                foreach(var g in gate.GetComponentsInChildren<GateSuccess>())
                    g.onGateTriggered.AddListener(game.GateTriggered);

                foreach(var slowdownTrigger in gate.GetComponentsInChildren<SlowdownTriggerManager>())
                    slowdownTrigger.onSlowdownTriggerEntered.AddListener(game.SlowdownTriggered);

                //Debug.Log($"Spawned: {selectedGate.prefab.name}");
                location += selectedGate.nextOffset * direction;
                game.lastGateSpawnLocation += new Vector3(0.06f * (selectedGate.nextOffset) * direction, -0.1f, 0f);

            }
        }

    }

}
