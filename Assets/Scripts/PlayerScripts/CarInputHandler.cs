using UnityEngine;
using UnityEngine.InputSystem;

public class CarInputHandler : MonoBehaviour
{
    private float driveInput;
    private Vector3 steerInput;

    [SerializeField] private Wheel[] wheels;

    public void OnDrive(InputAction.CallbackContext context)
    {
        driveInput = context.ReadValue<float>();
        Debug.Log(driveInput);
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
        }
    }




}
