using System;
using UnityEngine;
using UnityEngine.Perception.Randomization.Parameters;
using UnityEngine.Perception.Randomization.Randomizers;
using UnityEngine.Perception.Randomization.Samplers;

public class ObjectRotationRandomizerTag : RandomizerTag { }

[Serializable]
[AddRandomizerMenu("Object Rotation Randomizer")]
public class ObjectRotationRandomizer : Randomizer
{
    public FloatParameter rotationRange = new FloatParameter { value = new UniformSampler(0f, 90f) };

    protected override void OnIterationStart()
    {
        var objects = tagManager.Query<ObjectRotationRandomizerTag>();

        foreach (var obj in objects)
        {
            var transform = obj.transform;
            if (obj.name == "semitrailer_chassis(Clone)" || obj.name == "hot_glue_gun(Clone)")
            {
                var xRotation = transform.rotation.eulerAngles.x;
                var yRotation = rotationRange.Sample();
                var zRotation = transform.rotation.eulerAngles.z;
                transform.rotation = Quaternion.Euler(xRotation, yRotation, zRotation);
            }
            else
            {
                var xRotation = rotationRange.Sample();
                var yRotation = rotationRange.Sample();
                var zRotation = rotationRange.Sample();
                transform.rotation = Quaternion.Euler(xRotation, yRotation, zRotation);
            }
        }
    }
}
