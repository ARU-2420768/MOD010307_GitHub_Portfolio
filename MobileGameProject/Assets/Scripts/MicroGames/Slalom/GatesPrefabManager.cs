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

        [SerializeField] private int location = 7;
        [SerializeField] private Transform playArea;
        //[SerializeField] private Vector3 lastGateSpawnLocation = new Vector3(0f,0.45f,0f);
        [SerializeField] private Vector3 newGateSpawnLocation;
        [SerializeField] private float laneWidth = 0.06f;

        private SlalomGame game;

        private void Awake()
        {
            game = FindFirstObjectByType<SlalomGame>();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            // Create Start Game
            CreateSpecificGateFromPrefab(gatesPrefabsSSS);

            for(int i = 0; i < 11; i++){
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
                int direction = Random.value < 0.5f ? -1 : 1;
                
                float yRotation = direction == 1 ? 0f : 180f;

                GameObject gate = Instantiate(selectedGate.prefab, playArea);
                gate.transform.localPosition = game.lastGateSpawnLocation;
                gate.transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);

                Debug.Log($"Spawned: {selectedGate.prefab.name}");

                game.lastGateSpawnLocation += new Vector3(0.06f * (selectedGate.nextOffset) * direction, -0.1f, 0f);

            }
        }

    }

}
