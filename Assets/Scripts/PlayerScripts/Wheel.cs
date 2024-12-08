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
            //Tire up direction
            Vector3 springDir = transform.up;

            // Velocity Of the Tire
            Vector3 tireVel = carRB.GetPointVelocity(transform.position);

            //calculating the offset we need to put the car at.
            float offset = restLength - tire.distance;

            //Calculating the velocity in the spring direction aka transform.up
        
            float velocity = Vector3.Dot(springDir, tireVel);

            //Calcululating the actual suspension
            // What the offset times with how strong the tire is plus taking the actual speed into account and applying dampening to stop the car from bopping up and down forever
            float susForce = (offset * springPower) - (velocity * damping);

            //applying the force at the wheels position
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

        //In How fast is it moving in the steering direction.
        float steeringVel = Vector3.Dot(steeringDirection, tireVel);

        float carSpeed = Vector3.Dot(carTrans.forward, carRB.linearVelocity);

        //Here weuse the animation Curve to see how much grip the tire should have based on the speed were moving
        float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(carSpeed) / maxSpeed);
        float curGrip = tireGrip.Evaluate(normalizedSpeed);

        //Taking the opposite force and timing it with the grip to allow us to change how much car will slide
        float VelChange = -steeringVel * curGrip;
        
        //adding the force to prevent sliding too much :)
        carRB.AddForceAtPosition(steeringDirection * tireMass * VelChange, transform.position);

    }

    private void Turn()
    {
        if (steerInput != Vector3.zero)
        {
                //Rotating wheel wow crazy
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
