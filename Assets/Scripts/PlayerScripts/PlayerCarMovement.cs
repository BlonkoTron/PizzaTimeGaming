using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCarMovement : MonoBehaviour
{
    #region inspectorstuff
    
    [Header("MovementStats")]
    [SerializeField] float maxSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float deceleration;
    [SerializeField] float turnSpeed;

    [Header("SusStats")]
    [SerializeField] private float springLengthMin;
    [SerializeField] private float restLength;
    [SerializeField] private float springLengthMax;
    [SerializeField] private float wheelRadius;
    [SerializeField] private float damping;

    [Header("References")]
    [SerializeField] private Transform[] susPoints; //AMOGUS
    [SerializeField] private LayerMask drivable;
    [SerializeField] private Transform accelerationPoint;

    #endregion

    #region privatestuff
    
    private Rigidbody rb;

    private Vector3 steer;
    private Vector3 velocity;

    private int[] wheelCheck = new int[4];
    
    private bool speederDown;
    private bool isGrounded;
    
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

    }

    private void FixedUpdate()
    {

        Suspension();
        Grounded();

        if (isGrounded)
        {   


        }

    }


    private void Suspension()
    {
        for (int i = 0; i < susPoints.Length; i++)
        {
            RaycastHit hit;

            float maxL = restLength + springLengthMax;

            if (Physics.Raycast(susPoints[i].position, -susPoints[i].up, out hit, maxL + wheelRadius, drivable))
            {
                float currentSpringL = hit.distance - wheelRadius;
                float springCompress = (restLength - currentSpringL) / springLengthMax;
                float susVelocity = Vector3.Dot(rb.GetPointVelocity(susPoints[i].position), susPoints[i].up);
                float dampForce = damping * susVelocity;

                wheelCheck[i] = 1;

                float susForce = springLengthMin * springCompress;

                float totalForce = susForce - dampForce;
                rb.AddForceAtPosition(totalForce * susPoints[i].up, susPoints[i].position);

                Debug.DrawLine(susPoints[i].position, hit.point, Color.blue);

            }
            else
            {
                wheelCheck[i] = 0;
                Debug.DrawLine(susPoints[i].position, susPoints[i].position + (wheelRadius + maxL) * -susPoints[i].up, Color.blue);
            }
        }
    }

    private void Grounded()
    {
        int wheelsGrounded = 0;

        for (int i = 0; i < wheelCheck.Length; i++) 
        {
            wheelsGrounded += wheelCheck[i];
        }

        if (wheelsGrounded > 1) 
        {
            
            isGrounded = true;

        }
        else
        {
            isGrounded = false;
        }
    }

    public void OnDrive(InputAction.CallbackContext context)
    {
        speederDown = context.ReadValueAsButton();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        steer = context.ReadValue<Vector3>();
    }

}
