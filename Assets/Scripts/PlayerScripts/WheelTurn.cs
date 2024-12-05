using UnityEngine;
using UnityEngine.InputSystem;

public class WheelTurn : MonoBehaviour
{


    [SerializeField] private float turnSpeed;
    [SerializeField] private Vector3 maxTurn;

    private Vector3 steer;



    private void FixedUpdate()
    {
        
        if (steer != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(steer, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, turnSpeed * Time.fixedDeltaTime);
        }
        else
        {
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, turnSpeed * Time.fixedDeltaTime);
        }
        
        
    }


    public void OnMove(InputAction.CallbackContext context)
{
    steer = context.ReadValue<Vector3>();
}

}
