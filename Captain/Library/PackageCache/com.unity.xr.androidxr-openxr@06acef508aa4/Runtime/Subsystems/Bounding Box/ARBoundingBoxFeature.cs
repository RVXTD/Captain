using System.Collections.Generic;
using UnityEngine.XR.ARSubsystems;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.XR.OpenXR.Features;
#endif

namespace UnityEngine.XR.OpenXR.Features.Android
{
    /// <summary>
    /// Enables AR Foundation bounding box support via OpenXR for Android XR devices.
    /// </summary>
#if UNITY_EDITOR
    [OpenXRFeature(UiName = "Android XR: AR Bounding Box",
        BuildTargetGroups = new[] { BuildTargetGroup.Android },
        Company = Constants.k_CompanyName,
        Desc = "AR Foundation bounding box detection support on Android XR devices",
        DocumentationLink = Constants.DocsUrls.k_BoundingBoxUrl,
        OpenxrExtensionStrings = k_OpenXRRequestedExtensions,
        Category = FeatureCategory.Feature,
        FeatureId = featureId,
        Version = "0.1.0")]
#endif

    public class ARBoundingBoxFeature : AndroidXROpenXRFeature
    {
        /// <summary>
        /// The feature id string. This is used to give the feature a well known id for reference.
        /// </summary>
        public const string featureId = "com.unity.openxr.feature.arfoundation-androidxr-bounding-box";

        /// <summary>
        /// The set of OpenXR spec extension strings to enable, separated by spaces.
        /// </summary>
        const string k_OpenXRRequestedExtensions =
            Constants.OpenXRExtensions.k_XR_ANDROID_trackables + " " +
            Constants.OpenXRExtensions.k_XR_ANDROID_trackables_object;

        static readonly List<XRBoundingBoxSubsystemDescriptor> s_BoundingBoxDescriptors = new();

        /// <summary>
        /// Instantiates Android OpenXR Bounding Box subsystem instance, but does not start it.
        /// (Start/Stop is typically handled by AR Foundation managers.)
        /// </summary>
        protected override void OnSubsystemCreate()
        {
            CreateSubsystem<XRBoundingBoxSubsystemDescriptor, XRBoundingBoxSubsystem>(
                s_BoundingBoxDescriptors,
                AndroidOpenXRBoundingBoxSubsystem.k_SubsystemId);
        }

        /// <summary>
        /// Destroys the bounding box subsystem.
        /// </summary>
        protected override void OnSubsystemDestroy()
        {
            DestroySubsystem<XRBoundingBoxSubsystem>();
        }

#if UNITY_EDITOR
        /// <summary>
        /// Validation Rules for ARBoundingBoxFeature.
        /// </summary>
        protected override void GetValidationChecks(List<ValidationRule> rules, BuildTargetGroup targetGroup)
        {
            rules.AddRange(SharedValidationRules.EnableARSessionValidationRules(this));
        }
#endif
    }
}
