using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class CarRotation : MonoBehaviour
{

    [SerializeField] private float rotationSpeed;

    private void Update()
    {
        Vector3 currentRotation = transform.eulerAngles;
        currentRotation.z = Mathf.Lerp(currentRotation.z, 0, Time.deltaTime * rotationSpeed);
        transform.eulerAngles = currentRotation;
    }

}
