using UnityEngine;
using UnityEngine.UIElements;

public class PickUpPoint : MonoBehaviour
{

    private float pickupSpeed = 0.5f;
    private float distanceToPoint;

    private GameObject DeliveryPoint;
    private bool hasStarted;

    private void OnEnable()
    {   

        DeliveryPoint = GameManager.instance.GetDeliveryPoint();

        hasStarted = false;

        if (DeliveryPoint != null)
        {
            
            distanceToPoint = CalculateDistance(transform, DeliveryPoint.transform);
            Debug.Log(distanceToPoint + "m to: " + DeliveryPoint);
        }
    }

    private void StartDelivery()
    {
        hasStarted = true;

        DeliveryPoint.SetActive(true);

        GameManager.instance.StartDelivery(distanceToPoint, DeliveryPoint);

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<Rigidbody>().linearVelocity.magnitude < pickupSpeed)
            {
                if (!hasStarted)
                {
                    StartDelivery();
                }
                  
            }
        }
    }


    float CalculateDistance(Transform transform1, Transform transform2)
    {
        float distance = Vector3.Distance(transform1.position, transform2.position);

        return distance;
    }


}
