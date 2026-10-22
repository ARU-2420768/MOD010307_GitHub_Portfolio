using UnityEngine;
using MicrogameCourse.Framework;
using MicrogameCourse.Microgames;
using Unity.VisualScripting;


namespace MicrogameCourse.Framework
{
    public class PrefabManager : MicrogameBehaviour
    {
        [SerializeField] float gateSpeedReset = 1f;
        [SerializeField] bool showGates = true;
        private SlalomGame game;
        private MicrogameSession session;

        private void Awake()
        {
            session = FindFirstObjectByType<MicrogameSession>();
            game = FindFirstObjectByType<SlalomGame>();
        }

        private void Update()
        {
            if (!session.isTimerStarted) return;
            transform.position += Mathf.Clamp(game.gateMoveSpeed,game.gateMinMoveSpeed, game.gateMaxMoveSpeed) * Time.deltaTime * Vector3.up;
        }


    }
}