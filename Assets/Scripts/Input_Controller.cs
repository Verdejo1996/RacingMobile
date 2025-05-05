using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input_Controller : MonoBehaviour
{
    public float maxAcceleration = 100f;
    public float accelerationSensitivity = 0.5f;
    public float brakingSensitivity = 0.5f;
    public float maxBraking = 100f;

    public float currentAcceleration = 0f;
    public float currentBraking = 0f;

    private Vector2 rightSwipeStart;
    private Vector2 leftSwipeStart;
    private bool isRightSwiping = false;
    private bool isLeftSwiping = false;

    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.position.x > Screen.width / 2)
            {
                HandleRightSwipe(touch); // acelerar
            }
            else
            {
                HandleLeftSwipe(touch); // frenar
            }
        }
    }

    void HandleRightSwipe(Touch touch)
    {
        switch (touch.phase)
        {
            case TouchPhase.Began:
                rightSwipeStart = touch.position;
                isRightSwiping = true;
                break;

            case TouchPhase.Moved:
                if (isRightSwiping)
                {
                    float swipeDistance = touch.position.y - rightSwipeStart.y;
                    currentAcceleration = Mathf.Clamp(swipeDistance * accelerationSensitivity, 0, maxAcceleration);
                }
                break;

            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                isRightSwiping = false;
                currentAcceleration = 0;
                break;
        }
    }

    void HandleLeftSwipe(Touch touch)
    {
        switch (touch.phase)
        {
            case TouchPhase.Began:
                leftSwipeStart = touch.position;
                isLeftSwiping = true;
                break;

            case TouchPhase.Moved:
                if (isLeftSwiping)
                {
                    float swipeDistance = leftSwipeStart.y - touch.position.y; // hacia abajo
                    currentBraking = Mathf.Clamp(swipeDistance * brakingSensitivity, 0, maxBraking);
                }
                break;

            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                isLeftSwiping = false;
                currentBraking = 0;
                break;
        }
    }
}
