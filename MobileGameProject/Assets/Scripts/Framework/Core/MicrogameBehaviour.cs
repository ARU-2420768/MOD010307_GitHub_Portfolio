using UnityEngine;


namespace MicrogameCourse.Framework
{
    public abstract class MicrogameBehaviour : MonoBehaviour
    {
        // microgame session
        
        protected bool IsRunning {get; set;}
        protected MicrogameSession Session {get; private set;}

        public virtual void OnPointerPressed()
        {
        }

        public virtual void OnDrag(Vector2 delta)
        {
        }

        public virtual void OnDirectionalInput(Vector2 direction, GameObject button)
        {
        }

        public virtual void OnButtonTap(GameObject button)
        {
        }

        public virtual void Begin(MicrogameSession session)
        {
            Session = session;
            IsRunning = true;
        }

        public virtual void End()
        {
            IsRunning = false;
        }

        protected void Win()
        {
            if(IsRunning)
            {
                Session.Finish(true);
            }
        }

        protected void Lose()
        {
            if(IsRunning)
            {
                Session.Finish(false);
            }    
        }

    }

}
