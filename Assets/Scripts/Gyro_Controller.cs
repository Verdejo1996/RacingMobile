using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gyro_Controller : MonoBehaviour
{
    public float turnSpeed = 100f; // sensibilidad del giro
    private bool gyroEnabled;
    private Gyroscope gyro;

    void Start()
    {
        gyroEnabled = EnableGyro();
    }

    bool EnableGyro()
    {
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            return true;
        }
        return false;
    }

    public float GetSteeringInput()
    {
        if (!gyroEnabled) return 0f;

        // Usamos el acelerómetro en lugar del giroscopio para rotación horizontal
        float tilt = Input.acceleration.x;

        // Clamp para evitar valores extremos
        return Mathf.Clamp(tilt, -1f, 1f);
    }
}
