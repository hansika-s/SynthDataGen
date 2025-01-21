using System.Collections.Generic;
using UnityEngine.Perception.GroundTruth.DataModel;
using UnityEngine.Perception.GroundTruth.LabelManagement;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Perception.GroundTruth.MetadataReporter.Tags
{

    [MovedFrom("UnityEngine.Perception.GroundTruth.ReportMetadata")]
    public class LabelingInteractionTag : LabeledMetadataTag
    {
        public int handSide = -1; // 0: left hand, 1: right hand, -1: for objects
        public int contactState = -1; // 0: no contact, 1: contact
        // public float[] bboxObj = new float[] { -1.0f, -1.0f, -1.0f, -1.0f }; // ([x0, y0, width, height] bounding box of the active object or [-1, -1, -1, -1])
        public int idObj = -1; // object ID
        public float dx = -1.0f; // x component of offset vector
        public float dy = -1.0f; // y component of offset vector
        public float magnitude = -1.0f; // magnitude of offset vector

        protected override string key => "hand_object_interaction";

        protected override void GetReportedValues(IMessageBuilder builder)
        {
            builder.AddInt("hand_side", handSide);
            builder.AddInt("contact_state", contactState);
            // builder.AddFloatArray("bbox_obj", bboxObj);
            builder.AddInt("id_obj", idObj);
            builder.AddFloat("dx", dx);
            builder.AddFloat("dy", dy);
            builder.AddFloat("magnitude", magnitude);
        }
    }
}