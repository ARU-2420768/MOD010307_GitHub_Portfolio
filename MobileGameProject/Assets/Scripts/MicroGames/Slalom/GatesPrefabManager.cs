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
        [SerializeField] private int location = 7;
        [SerializeField] private Transform playArea;
        [SerializeField] private Vector3 lastGateSpawnLocation = new Vector3(0f,0.45f,0f);
        [SerializeField] private Vector3 newGateSpawnLocation;



        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            CreateNewGateFromPrefab();
            //CreateNewGateFromPrefab();
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
                    Debug.Log($"Matched Gate: {gatePrefab.prefab.name} ({gatePrefab.allowedLocations})");

                    GameObject gate = Instantiate(gatePrefab.prefab, playArea);

                    int direction = Random.value < 0.5f ? -1 : 1;
                    float yRotation = direction == 1 ? 0f : 180f;
                    gate.transform.localPosition = lastGateSpawnLocation;
                    gate.transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);

                    lastGateSpawnLocation += new Vector3(0.06f * (gatePrefab.nextOffset + gatePrefab.preOffset) * direction, -0.1f, 0f);


                }
            }



            if (matchedConnections.Count > 0 && false)
            {
                GateDefinition selectedGate = matchedConnections[Random.Range(0, matchedConnections.Count)];
                int direction = Random.value < 0.5f ? -1 : 1;
                
                float yRotation = direction == 1 ? 0f : 180f;

                GameObject gate = Instantiate(selectedGate.prefab, playArea);
                gate.transform.localPosition = lastGateSpawnLocation;
                gate.transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);

                Debug.Log($"Spawned: {selectedGate.prefab.name}");

                lastGateSpawnLocation += new Vector3(0.06f * (selectedGate.nextOffset + selectedGate.preOffset) * direction, -0.1f, 0f);

            }

            
            Debug.Log($"Matched Connections: {string.Join(", ", matchedConnections)}");
        }

    }

}
