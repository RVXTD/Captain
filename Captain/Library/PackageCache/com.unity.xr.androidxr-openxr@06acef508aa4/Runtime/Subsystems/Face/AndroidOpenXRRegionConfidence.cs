namespace UnityEngine.XR.OpenXR.Features.Android
{
    /// <summary>
    /// Matches Android XR native type, XRFaceConfidenceRegions.
    ///
    /// Enum values that identify different regions of the face for which confidence values can be obtained.
    /// </summary>
    /// <remarks> Confidence values can be obtained for each region representing the accuracy of the region's blend shape data.
    /// Use a region's enum value to obtain the corresponding confidence value from the array provided by <see cref="AndroidOpenXRFaceSubsystem.TryGetInwardRegionConfidences*"/>.
    /// </remarks>
    public enum AndroidXRFaceConfidenceRegion
    {
        /// <summary>
        /// Lower confidence region.
        /// </summary>
        Lower = 0,
        /// <summary>
        /// Left upper confidence region.
        /// </summary>
        LeftUpper = 1,
        /// <summary>
        /// Right upper confidence region.
        /// </summary>
        RightUpper = 2
    }
}
