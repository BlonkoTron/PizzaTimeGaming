using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarInputHandler : MonoBehaviour
{
    private float driveInput;
    private float brakeInput;
    private Vector3 steerInput;


    [SerializeField] private Wheel[] wheels;

    public void OnDrive(InputAction.CallbackContext context)
    {
        driveInput = context.ReadValue<float>();
        
    }

    public void OnBrake(InputAction.CallbackContext context)
    {
        brakeInput = context.ReadValue<float>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        steerInput = context.ReadValue<Vector3>();
    }

    private void Update()
    {

        foreach (Wheel wheel in wheels)
        {
            wheel.driveInput = driveInput;
            wheel.steerInput = steerInput;
            wheel.brakeInput = brakeInput;
        }


    }

}
