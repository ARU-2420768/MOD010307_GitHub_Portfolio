using UnityEngine;
using MicrogameCourse.Framework;

namespace MicrogameCourse.Framework
{
    public class MoveGates : MicrogameBehaviour
    {

        private float moveSpeed = 100f;
        private MicrogameSession session;

        private void Awake()
        {
            session = FindFirstObjectByType<MicrogameSession>();
        }

        private void Update()
        {
            if (!session.isTimerStarted) return;
            transform.position += moveSpeed * Time.deltaTime * Vector3.down;
        }


    }
}