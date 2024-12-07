using UnityEngine;

public class ArrowController : MonoBehaviour
{

    [SerializeField] private float rotationSpeed = 5f;
    private Transform target;


    public void SetDestination(Transform destination)
    {
        target = destination;
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            direction.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }



}
