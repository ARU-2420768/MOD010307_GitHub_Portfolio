using UnityEngine;
using MicrogameCourse.Framework;
using MicrogameCourse.Microgames;

namespace MicrogameCourse.Framework
{
    public class MoveGates : MicrogameBehaviour
    {

        private float moveSpeed = 100f;
        private MicrogameSession session;
        private SlalomGame game;
        private RectTransform rectTransform;
        private RectTransform parentRect;
        private float bottomLimit, topLimit;


        private void Awake()
        {
            session = FindFirstObjectByType<MicrogameSession>();
            game = FindFirstObjectByType<SlalomGame>();
            rectTransform = GetComponent<RectTransform>();
            parentRect = transform.parent.GetComponent<RectTransform>();
            bottomLimit = -parentRect.rect.height / 2f;
            topLimit = parentRect.rect.height / 2f;
        }

        private void Update()
        {
            if (!session.isTimerStarted) return;
            transform.position += moveSpeed * Time.deltaTime * Vector3.down;

            if(rectTransform.anchoredPosition.y <= bottomLimit + 10)
            {
                gameObject.SetActive(false);
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            game.CreateNewGate();
        }

    }
}