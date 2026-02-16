using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.OpenXR.Features.Android
{
    /// <summary>
    /// Extensions to [XRFaceBlendShape](xref:XRFaceBlendShape)
    /// </summary>
    public static class XRFaceBlendShapeExtensions
    {
#if UNITY_ANDROID
        /// <summary>
        /// Helper function for casting face feature parameter to platform specific feature type.
        /// </summary>
        /// <returns>A hash code generated from this object's fields.</returns>
        public static AndroidXRBlendShapeLocation AsAndroidXRBlendShapeLocation(this XRFaceBlendShape coefficient)
        {
            return (AndroidXRBlendShapeLocation)coefficient.blendShapeId;
        }
#endif // UNITY_ANDROID
    }
}
