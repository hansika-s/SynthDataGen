using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Perception.GroundTruth;

public class CaptureManager : MonoBehaviour
{
    public PerceptionCamera perceptionCamera;
    private bool isCapturing = false;  // Flag to track capture state

    public void StartCapture()
    {
        if (!isCapturing)
        {
            isCapturing = true;
            StartCoroutine(CaptureFrame());
        }
    }

    // Coroutine to capture frame
    public IEnumerator CaptureFrame()
    {
        while (isCapturing)
        {
            perceptionCamera.RequestCapture();
            yield return new WaitForEndOfFrame();
            Debug.Log("Frame captured.");
        }
    }

    public void StopCapture()
    {
        isCapturing = false;
    }

    public void CaptureSingleFrame()
    {
        if (!isCapturing)
        {
            isCapturing = true;
            StartCoroutine(CaptureSingleFrameCoroutine());
        }
    }

    // Coroutine to capture a single frame
    private IEnumerator CaptureSingleFrameCoroutine()
    {
        perceptionCamera.RequestCapture();
        yield return new WaitForEndOfFrame();

        Debug.Log("Single frame captured.");
        isCapturing = false;
    }
}
