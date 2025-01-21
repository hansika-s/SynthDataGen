using System;
using UnityEngine;
using UnityEngine.Perception.Randomization.Parameters;
using UnityEngine.Perception.Randomization.Randomizers;
using UnityEngine.Perception.Randomization.Samplers;

public class HandMaterialRandomizerTag : RandomizerTag { }

[Serializable]
[AddRandomizerMenu("Hand Material Randomzier")]
public class HandMaterialRandomizer : Randomizer
{
    public Material[] handMaterials;
    public Material[] sleeveMaterials;

    protected override void OnIterationStart()
    {
        var hands = tagManager.Query<HandMaterialRandomizerTag>();

        var handMaterialIndex = UnityEngine.Random.Range(0, handMaterials.Length);
        var handMaterial = handMaterials[handMaterialIndex];

        Material sleeveMaterial;
        if (UnityEngine.Random.value <= 0.30f)
        {
            sleeveMaterial = handMaterial;
        }
        else
        {
            var sleeveMaterialIndex = UnityEngine.Random.Range(0, sleeveMaterials.Length);
            sleeveMaterial = sleeveMaterials[sleeveMaterialIndex];
        }

        foreach (var hand in hands)
        {
            Renderer[] renderers = hand.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            foreach (Renderer renderer in renderers)
            {
                if (renderer.gameObject.name == "Model")
                {
                    renderer.material = handMaterial;
                }
            }

            var sleeveRenderer = hand.transform.Find("Arm")?.GetComponent<Renderer>();
            if (sleeveRenderer != null)
            {
                sleeveRenderer.material = sleeveMaterial;
            }


        }

    }
}
