using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Perception.Randomization.Parameters;
using UnityEngine.Perception.Randomization.Randomizers;

using UnityEngine.Perception.Randomization.Samplers;


/// Creates a 2D layer of of evenly spaced GameObjects from a given list of prefabs
[Serializable]
[AddRandomizerMenu("ObjectPlacementRandomizer")]
public class ObjectPlacementRandomizer : Randomizer
{
    public GameObject surfaceObject;
    //List of objects to be placed on the surface
    public GameObject[] objectsToPlace;
    //The container object that will be the parent of all placed objects from this Randomizer
    GameObject m_Container;
    //This cache allows objects to be reused across placements
    UnityEngine.Perception.Randomization.Utilities.GameObjectOneWayCache m_GameObjectOneWayCache;

    protected override void OnAwake()
    {
        m_Container = new GameObject("Objects");
        m_Container.transform.parent = scenario.transform;
        m_GameObjectOneWayCache = new UnityEngine.Perception.Randomization.Utilities.GameObjectOneWayCache(
            m_Container.transform, objectsToPlace, this);
    }

    /// Generates a foreground layer of objects at the start of each Scenario Iteration
    protected override void OnIterationStart()
    {
        // Calculate the original surface bounds
        var surfaceBounds = CalculateSurfaceBounds(surfaceObject.transform);
        Debug.Log(surfaceBounds);

        foreach (var prefab in objectsToPlace)
        {
            // Get the current position and rotation of the prefab
            Vector3 currentPosition = prefab.transform.position;
            Quaternion currentRotation = prefab.transform.rotation;

            // Calculate a random position within the surface bounds with the same y coordinate as the current position
            float randomX = UnityEngine.Random.Range(surfaceBounds.min.x, surfaceBounds.max.x);
            float randomZ = UnityEngine.Random.Range(surfaceBounds.min.z, surfaceBounds.max.z);
            Vector3 randomPosition = new Vector3(randomX, currentPosition.y, randomZ);

            // Instantiate the object at the calculated position with the original rotation
            InstantiateFromCache(prefab, randomPosition, currentRotation);
        }
    }

    void InstantiateFromCache(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        var instance = m_GameObjectOneWayCache.GetOrInstantiate(prefab);

        // Set the position and rotation of the instance
        instance.transform.position = position;
        instance.transform.rotation = rotation;
    }

    Bounds CalculateSurfaceBounds(Transform surfaceTransform)
    {
        var bounds = new Bounds(Vector3.zero, Vector3.zero);
        var renderer = surfaceTransform.GetComponent<Renderer>();

        if (renderer != null)
        {
            bounds = renderer.bounds;
            Vector3 reducedExtents = bounds.extents * 0.25f; // Reducing extents by half
            bounds.extents = reducedExtents;

        }
        else
        {
            Debug.LogError("Renderer not found on the surface object. Surface bounds cannot be calculated.");
        }

        return bounds;
    }
    /// Hides all foreground objects after each Scenario Iteration is complete
    protected override void OnIterationEnd()
    {
        m_GameObjectOneWayCache.ResetAllObjects();
    }
}
