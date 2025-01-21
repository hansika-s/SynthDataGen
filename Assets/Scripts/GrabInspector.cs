using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand;
using UnityEngine.Perception.GroundTruth.MetadataReporter.Tags;


public class GrabInspector : MonoBehaviour
{
    private LabelingInteractionTag interactionTag;
    private bool isGrabbed = false;
    private Coroutine grabLoggingCoroutine;

    public Camera cameraComponent;


    public void LogGrabDetails(Hand hand, Grabbable grabbable)
    {
        isGrabbed = true;
        grabLoggingCoroutine = StartCoroutine(LogGrabbedState(hand, grabbable));
    }

    private IEnumerator LogGrabbedState(Hand hand, Grabbable grabbable)
    {
        while (isGrabbed)
        {
            // interactionTag = hand.GetComponent<LabelingInteractionTag>();
            interactionTag = hand.GetComponentInChildren<LabelingInteractionTag>();
            interactionTag.contactState = 1;
            string handSide = hand.name.Contains("Left") ? "Left" : "Right";
            string grabbedObjectName = grabbable.name;
            int objectId = grabbable.gameObject.GetComponent<ObjectID>().id;
            interactionTag.idObj = objectId;


            Vector3 handPosition = cameraComponent.WorldToScreenPoint(hand.transform.position);
            Vector3 objectPosition = cameraComponent.WorldToScreenPoint(grabbable.transform.position);
            Vector2 handCenter2D = new Vector2(handPosition.x, handPosition.y);
            Vector2 objectCenter2D = new Vector2(objectPosition.x, objectPosition.y);

            Vector2 handToObject2D = objectCenter2D - handCenter2D;

            interactionTag.dx = handToObject2D.x;
            interactionTag.dy = handToObject2D.y;
            interactionTag.magnitude = handToObject2D.magnitude;

            Debug.Log($"Object '{grabbedObjectName}' is grabbed by {handSide} hand at frame {Time.frameCount}");


            yield return null;
        }

    }
    public void Reset()
    {
        StopCoroutine(grabLoggingCoroutine);
        isGrabbed = false;
        interactionTag.contactState = 0;
        interactionTag.idObj = -1;
        interactionTag.dx = -1.0f;
        interactionTag.dy = -1.0f;
        interactionTag.magnitude = -1.0f;
    }

}
