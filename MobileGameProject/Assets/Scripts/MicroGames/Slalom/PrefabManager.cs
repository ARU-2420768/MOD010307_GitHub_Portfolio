using UnityEngine;
using MicrogameCourse.Framework;
using MicrogameCourse.Microgames;
using Unity.VisualScripting;


namespace MicrogameCourse.Framework
{
    public class PrefabManager : MicrogameBehaviour
    {
        private SlalomGame game;
        private MicrogameSession session;

        private void Awake()
        {
            session = FindFirstObjectByType<MicrogameSession>();
            game = FindFirstObjectByType<SlalomGame>();
            //GateSuccess[] gates = GetComponentsInChildren<GateSuccess>();

            //foreach (GateSuccess gate in gates)
            //{
                //gate.onGateTriggered.AddListener(game.GateTriggered);
            //}
        }

        private void Update()
        {
            if (!session.isTimerStarted) return;
            //transform.position += game.gateMoveSpeed * Time.deltaTime * Vector3.up;
            transform.position += Mathf.Clamp(game.gateMoveSpeed,game.gateMinMoveSpeed, game.gateMaxMoveSpeed) * Time.deltaTime * Vector3.up;
        }


    }
}