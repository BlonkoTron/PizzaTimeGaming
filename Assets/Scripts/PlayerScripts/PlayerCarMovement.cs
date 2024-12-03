using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCarMovement : MonoBehaviour
{
    #region inspectorstuff
    
    [Header("MovementStats")]
    [SerializeField] float maxSpeed;
    [SerializeField] float acceleration;

    #endregion

    #region privatestuff
    
    private Rigidbody rb;

    private Vector3 movement;

    #endregion

    private void OnMove(InputValue input)
    {

        movement = input.Get<Vector3>();

        Debug.Log(movement + " Vroom");

    }


}
