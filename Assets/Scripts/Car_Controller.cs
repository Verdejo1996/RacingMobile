using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Controller : MonoBehaviour
{
    public Input_Controller inputController;
    public float maxSpeed = 20f;
    public float accelerationRate = 15f;
    public float friction = 5f;
    public float brakeFriction = 25f;

    private Rigidbody rb;
    private float currentSpeed = 0f;

    public Gyro_Controller gyroSteering;
    public float turnSpeed = 100f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (inputController == null)
        {
            inputController = FindObjectOfType<Input_Controller>();
        }

        if (gyroSteering == null)
        {
            gyroSteering = FindObjectOfType<Gyro_Controller>();
        }
    }

    void FixedUpdate()
    {
        /*        float accelerationInput = inputController.currentAcceleration;
                float brakingInput = inputController.currentBraking;

                // Aceleración
                if (accelerationInput > 0)
                {
                    Vector3 force = (accelerationInput / 100f) * accelerationForce * transform.forward;
                    rb.AddForce(force, ForceMode.Acceleration);
                }

                // Frenado
                if (brakingInput > 0)
                {
                    Vector3 oppositeForce = (brakingInput / 100f) * brakingForce * -rb.velocity.normalized;
                    rb.AddForce(oppositeForce, ForceMode.Acceleration);
                }*/

        float accelInput = inputController.currentAcceleration / 100f;
        float brakeInput = inputController.currentBraking / 100f;

        // Acelerar
        if (accelInput > 0f)
        {
            currentSpeed += accelInput * accelerationRate * Time.fixedDeltaTime;
        }

        // Frenar manualmente
        if (brakeInput > 0f)
        {
            currentSpeed -= brakeInput * brakeFriction * Time.fixedDeltaTime;
        }
        else
        {
            // Frenado suave por fricción
            currentSpeed -= friction * Time.fixedDeltaTime;
        }

        // Clamp para que no se pase ni vaya en reversa
        currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);

        // Aplicar movimiento con la velocidad actual
        Vector3 movement = currentSpeed * Time.fixedDeltaTime * transform.forward;
        rb.MovePosition(rb.position + movement);

        // Aplicar rotación según el tilt
        float steerInput = gyroSteering.GetSteeringInput();
        transform.Rotate(0f, steerInput * turnSpeed * Time.fixedDeltaTime, 0f);

        AlignToGround();
    }

    void AlignToGround()
    {
        RaycastHit hit;

        // Lanza un rayo hacia abajo desde un poco más arriba del vehículo
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, -transform.up, out hit, 5f))
        {
            // La normal del terreno
            Vector3 groundNormal = hit.normal;

            // Calcular la rotación necesaria para alinear "arriba" del auto con la normal del terreno
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, groundNormal) * transform.rotation;

            // Interpolar suavemente hacia esa rotación
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }
}