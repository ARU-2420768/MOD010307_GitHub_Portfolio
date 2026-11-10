using UnityEngine;
using UnityEngine.Events;

public class SlowdownTriggerManager : MonoBehaviour
{

    public UnityEvent<string> onSlowdownTriggerEntered;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag != "Player") return;
           
        onSlowdownTriggerEntered?.Invoke(this.tag);
    }

}
