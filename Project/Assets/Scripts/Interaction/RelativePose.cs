using UnityEngine;

public struct RelativePose
{
    public Vector3 localOffset;          
    public Quaternion localRotationOffset;

    public RelativePose(Transform root, Transform target) {
        localOffset = root.InverseTransformPoint(target.position);
        localRotationOffset = Quaternion.Inverse(root.rotation) * target.rotation;
    }
    public RelativePose(Transform root, Transform target, Vector3 offset) {
        localOffset = root.InverseTransformPoint(target.position) + offset;
        localRotationOffset = Quaternion.Inverse(root.rotation) * target.rotation;
    }

    public void ApplyTo(Transform root, Transform target)
    {
        target.SetPositionAndRotation(
            root.TransformPoint(localOffset),
            root.rotation * localRotationOffset
        );
    }
}