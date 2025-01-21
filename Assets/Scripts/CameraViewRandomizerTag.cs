using System;
using UnityEngine;
using UnityEngine.Perception.Randomization.Parameters;
using UnityEngine.Perception.Randomization.Randomizers;
using UnityEngine.Perception.Randomization.Samplers;

[Serializable]
[AddRandomizerMenu("CameraViewRandomizer")]
public class CameraViewRandomizer : Randomizer
{
    public GameObject user;

    // Pitch (X-Axis):This range allows for subtle nodding movements, with the head tilting slightly up or down.
    // Yaw (Y-Axis): This range enables gentle side-to-side head movements, simulating turning to look around.
    // Roll (Z-Axis): This range provides slight tilting movements from side to side, adding a touch of naturalness to the head's orientation.

    private FloatParameter rotationRangeX = new FloatParameter { value = new UniformSampler(-40f, 0f) };
    private FloatParameter rotationRangeY = new FloatParameter { value = new UniformSampler(-2f, 2f) };
    private FloatParameter rotationRangeZ = new FloatParameter { value = new UniformSampler(-2f, 2f) };

    private FloatParameter movementRangeX = new FloatParameter { value = new UniformSampler(-0.05f, 0.15f) };
    private FloatParameter movementRangeZ = new FloatParameter { value = new UniformSampler(-0.5f, -0.45f) };

    private Quaternion initialRotation;
    private Vector3 initialPosition;

    protected override void OnAwake()
    {
        // Store the initial rotation and position of the user object
        if (user != null)
        {
            initialPosition = user.transform.localPosition;
            initialRotation = user.transform.localRotation;
        }
        else
        {
            Debug.LogError("User GameObject is not assigned.");
        }
    }

    protected override void OnIterationStart()
    {
        // Ensure user is not null
        if (user == null)
        {
            Debug.LogError("User GameObject is not assigned.");
            return;
        }

        // Reset to initial rotation and position before applying new random values
        user.transform.localPosition = initialPosition;
        user.transform.localRotation = initialRotation;

        // User movement
        var horizontalMovementDelta = movementRangeX.Sample();
        var forwardMovementDelta = movementRangeZ.Sample();
        user.transform.localPosition = new Vector3(horizontalMovementDelta, user.transform.localPosition.y, forwardMovementDelta);

        // User Head Rotation
        var rotationDeltaX = rotationRangeX.Sample();
        var rotationDeltaY = rotationRangeY.Sample();
        var rotationDeltaZ = rotationRangeZ.Sample();

        Quaternion rotationDelta = Quaternion.Euler(rotationDeltaX, rotationDeltaY, rotationDeltaZ);
        Quaternion targetRotation = user.transform.localRotation * rotationDelta;
        user.transform.localRotation = targetRotation;



    }
}

