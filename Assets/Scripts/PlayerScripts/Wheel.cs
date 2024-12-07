using UnityEngine;
using UnityEngine.InputSystem;

public class Wheel : MonoBehaviour
{
    [Header("WheelSettings")]
    [SerializeField] bool drive = true;
    [SerializeField] bool turning = true;


    [Header("References")]
    [SerializeField] private Rigidbody carRB;
    [SerializeField] private Transform carTrans;
    [SerializeField] private LayerMask drivable;

    [Header("SusStats")]
    [SerializeField] private float springPower;
    [SerializeField] private float restLength;
    
    [SerializeField] private float wheelRadius;
    [SerializeField] private float damping;

    [Header("SpeedStats")]
    [SerializeField] float maxSpeed;
    [SerializeField] AnimationCurve speedCurve;
    [SerializeField] float acceleration;
    [SerializeField] float decceleration;
    [SerializeField] float brakePower;

    [Header("TurnStats")]
    [SerializeField] float turnSpeed;
    [SerializeField] AnimationCurve tireGrip;
    [SerializeField] float tireMass;

    [HideInInspector] public float driveInput = 0;
    [HideInInspector] public float brakeInput = 0;
    [HideInInspector] public Vector3 steerInput;

    private bool isGrounded = false;

    private Vector3 accelDirection;

    private void FixedUpdate()
    {
        isGrounded = Suspension();

        if (isGrounded)
        {
            if (drive)
            {
                Accelerate();
            }

            if (turning)
            {
                Turn();
            }
            
            SlowDown();
            TurnGrip();
        }

    }

    private bool Suspension()
    {
        RaycastHit tire;

        if (Physics.Raycast(transform.position, -transform.up, out tire, wheelRadius, drivable))
        {
            Vector3 springDir = transform.up;

            Vector3 tireVel = carRB.GetPointVelocity(transform.position);

            float offset = restLength - tire.distance;

            float velocity = Vector3.Dot(springDir, tireVel);

            float susForce = (offset * springPower) - (velocity * damping);

            carRB.AddForceAtPosition(springDir * susForce, transform.position);

            return true;

        }
        else 
        {
            return false;
        }
    }

    private void Accelerate()
    {
  
        accelDirection = transform.forward;
        
        if (driveInput > 0)
        {
            float carSpeed = Vector3.Dot(carTrans.forward, carRB.linearVelocity);

            float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(carSpeed) / maxSpeed);

            float availableTorque = speedCurve.Evaluate(normalizedSpeed) * driveInput + acceleration;

            carRB.AddForceAtPosition(transform.forward * availableTorque, transform.position);
        }


    }

    private void SlowDown()
    {
             
        Vector3 tireVel = carRB.GetPointVelocity(transform.position);

        float deccelVel = Vector3.Dot(transform.forward, tireVel);

        float deccelRate;

        if (brakeInput > 0)
        {
            deccelRate = brakePower;
        }
        else
        {
            deccelRate = decceleration;
        }

        float velChange = -deccelVel * deccelRate;
               
        carRB.AddForceAtPosition(transform.forward * velChange, transform.position);
        
    }

    private void TurnGrip()
    {
        Vector3 steeringDirection = transform.right;

        Vector3 tireVel = carRB.GetPointVelocity(transform.position);

        float steeringVel = Vector3.Dot(steeringDirection, tireVel);

        float carSpeed = Vector3.Dot(carTrans.forward, carRB.linearVelocity);

        float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(carSpeed) / maxSpeed);

        float curGrip = tireGrip.Evaluate(normalizedSpeed);

        float VelChange = -steeringVel * curGrip;

        carRB.AddForceAtPosition(steeringDirection * tireMass * VelChange, transform.position);

    }

    private void Turn()
    {
        if (steerInput != Vector3.zero)
        {

                Vector3 turnDirection = carRB.transform.right * steerInput.x;

                Quaternion toRotation = Quaternion.LookRotation(turnDirection + carRB.transform.forward, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, turnSpeed * Time.fixedDeltaTime);


        }
        else
        {
            Quaternion toRotation = Quaternion.LookRotation(carRB.transform.forward, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, turnSpeed * Time.fixedDeltaTime);
        }



    }

}
