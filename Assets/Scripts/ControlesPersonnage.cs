using UnityEngine;
using UnityEngine.InputSystem;

public class ControlesPersonnage : MonoBehaviour
{
    public InputAction mvt2d;
    [Range(0f, 10f)]
    public float vitesse = 5;

    Rigidbody rigidBody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }


    private void OnEnable()
    {
        mvt2d.Enable();
    }

    private void OnDisable()
    {
        mvt2d.Disable();
    }

    private void FixedUpdate()
    {
        //Debug.Log(mvt2d.ReadValue<Vector2>());
        Vector3 mvtFinal = new Vector3(mvt2d.ReadValue<Vector2>().x, 0, mvt2d.ReadValue<Vector2>().y).normalized;
        rigidBody.linearVelocity = mvtFinal * vitesse;
    }

}
