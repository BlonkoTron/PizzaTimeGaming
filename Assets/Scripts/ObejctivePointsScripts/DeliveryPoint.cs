using UnityEngine;

public class DeliveryPoint : MonoBehaviour
{

    private bool hasEnded;
    private float pickupSpeed = 0.5f;

    private void OnEnable()
    {
        hasEnded = false;
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<Rigidbody>().linearVelocity.magnitude < pickupSpeed)
            {
                if (!hasEnded)
                {
                    EndDelivery();
                }

            }
        }
    }

    private void EndDelivery()
    {
        hasEnded = true;

        GameManager.instance.EndDelivery();

        gameObject.SetActive(false);

    }


}
