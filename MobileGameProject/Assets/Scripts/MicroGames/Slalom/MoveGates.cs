using UnityEngine;
using MicrogameCourse.Framework;

namespace MicrogameCourse.Framework
{
    public class MoveGates : MicrogameBehaviour
    {

        private float moveSpeed = 100f;
        private MicrogameSession session;
        private RectTransform rectTransform;
        private RectTransform parentRect;
        private float bottomLimit;


        private void Awake()
        {
            session = FindFirstObjectByType<MicrogameSession>();
            rectTransform = GetComponent<RectTransform>();
            parentRect = transform.parent.GetComponent<RectTransform>();
            bottomLimit = -parentRect.rect.height / 2f;
        }

        private void Update()
        {
            if (!session.isTimerStarted) return;
            transform.position += moveSpeed * Time.deltaTime * Vector3.down;
            
            if(rectTransform.anchoredPosition.y <= bottomLimit + 25)
            {
                gameObject.SetActive(false);
                Destroy(gameObject);
            }
            
        }

        private void OnDestroy()
        {
            
        }

    }
}