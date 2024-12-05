using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCarMovement : MonoBehaviour
{
    #region inspectorstuff
    
    [Header("MovementStats")]
    [SerializeField] float maxSpeed;
    [SerializeField] AnimationCurve speedCurve;
    [SerializeField] float acceleration;
    [SerializeField] float deceleration;
    [SerializeField] float decelerationThreshold;
    [SerializeField] float turnSpeed;
    [SerializeField] AnimationCurve turningCurve;
    [SerializeField] float sideDrag;

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
    [SerializeField] private Transform WheelTransform;

    #endregion

    #region privatestuff
    
    private Rigidbody rb;

    private Vector3 steer;

    private float currentSpeed;

    private int[] wheelCheck = new int[4];
    
    private bool speederDown;
    private bool brakeDown;
    private bool isGrounded;
    
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

    }

    private void FixedUpdate()
    {
        CalculateSpeed();
        Suspension();
        Grounded();

        if (isGrounded)
        {
            AddDrag();
            Turn();

            if (speederDown)
            {
                Accelerate();
            }
            else if (currentSpeed > decelerationThreshold)
            {
                //Decelerate();
            }
           
        }

    }

    private void CalculateSpeed()
    {
        Vector3 accelDir = transform.forward;

        float curSpeed = Vector3.Dot(accelDir, rb.linearVelocity);

        currentSpeed = Mathf.Clamp01(Mathf.Abs(curSpeed) / maxSpeed);

    }


    private void Accelerate()
    {
        Vector3 accelDir = transform.forward;

        float avaliableTorque = speedCurve.Evaluate(currentSpeed) * acceleration;

        rb.AddForceAtPosition(accelDir * avaliableTorque, accelerationPoint.position);

    }
    private void Decelerate()
    {
        float avaliableTorque = speedCurve.Evaluate(currentSpeed) * deceleration;

        rb.AddForceAtPosition(-transform.forward * avaliableTorque, accelerationPoint.position);
    }


    private void Turn()
    {
        rb.AddRelativeTorque(turnSpeed * steer.x * turningCurve.Evaluate(currentSpeed) * Mathf.Sign(currentSpeed) * transform.up, ForceMode.Acceleration);
    }

    private void AddDrag()
    {
        float currentSideSpeed = rb.linearVelocity.x;

        float dragPower = -currentSideSpeed * sideDrag;

        Vector3 dragForce = transform.right * dragPower;

        rb.AddForceAtPosition(dragForce, rb.worldCenterOfMass, ForceMode.Acceleration);
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

    public void OnBrake(InputAction.CallbackContext context)
    {
        brakeDown = context.ReadValueAsButton();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        steer = context.ReadValue<Vector3>();
    }

   

}
