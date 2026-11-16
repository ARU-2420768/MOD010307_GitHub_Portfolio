using System.Collections;
using MicrogameCourse.Microgames;
using UnityEngine;

public class BoundaryManager : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider collision)
    {

        //Debug.Log($"Collider name: {collision.tag}");

        if (collision.CompareTag("Stone"))
        {
            Rigidbody rb = collision.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            StartCoroutine(FlashThenDisable(collision.gameObject));
            //collision.gameObject.SetActive(false);
        }
    }

    private IEnumerator FlashThenDisable(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();

        if (renderer != null)
        {
            for (int i = 0; i < 2; i++)
            {
                renderer.enabled = false;
                yield return new WaitForSeconds(0.15f);

                renderer.enabled = true;
                yield return new WaitForSeconds(0.15f);
            }
        }

        obj.SetActive(false);
    }

}
