using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine.Android;
using UnityEngine.Scripting;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.OpenXR.Features.Android
{
    /// <summary>
    /// The Android-OpenXR implementation of the <see cref="XRBoundingBoxSubsystem"/>.
    /// </summary>
    [Preserve]
    public sealed class AndroidOpenXRBoundingBoxSubsystem : XRBoundingBoxSubsystem
    {
        internal const string k_SubsystemId = "Android-Bounding-Box";

        /// <summary>
        /// Attempts to set which bounding boxes will be detected.
        /// </summary>
        /// <param name="detectionMode">Specifies which classifications of bounding boxes should be detected.</param>
        /// <returns>The status of setting the detection mode.</returns>
        public XRResultStatus TrySetBoundingBoxDetectionMode(BoundingBoxClassifications detectionMode)
        {
            return ((AndroidOpenXRBoundingBoxPovider)provider).TrySetBoundingBoxDetectionMode(detectionMode);
        }

        /// <summary>
        /// Gets which bounding boxes are currently being detected.
        /// </summary>
        /// <returns>The classifications of bounding boxes that are being detected.</returns>
        public BoundingBoxClassifications GetBoundingBoxDetectionMode()
        {
            return ((AndroidOpenXRBoundingBoxPovider)provider).GetBoundingBoxDetectionMode();
        }

        /// <summary>
        /// Gets which bounding boxes classifications for which detection is supported.
        /// </summary>
        /// <returns>The classifications of bounding boxes for which detection is supported.</returns>
        public BoundingBoxClassifications GetSupportedBoundingBoxDetectionModes()
        {
            return ((AndroidOpenXRBoundingBoxPovider)provider).GetSupportedBoundingBoxDetectionModes();
        }

        class AndroidOpenXRBoundingBoxPovider : Provider
        {

            protected override bool TryInitialize()
            {
                if (OpenXRRuntime.IsExtensionEnabled(Constants.OpenXRExtensions.k_XR_ANDROID_trackables) &&
                    OpenXRRuntime.IsExtensionEnabled(Constants.OpenXRExtensions.k_XR_ANDROID_trackables_object))
                {
                    NativeApi.Create();
                    return true;
                }
                return false;
            }

            public override void Start()
            {
#if UNITY_ANDROID
                if (!Permission.HasUserAuthorizedPermission(Constants.Permissions.k_SceneUnderstandingCoarsePermission) &&
                    !Permission.HasUserAuthorizedPermission(Constants.Permissions.k_SceneUnderstandingFinePermission))
                {
                    Debug.LogWarning($"Bounding Box detection requires system permission {Constants.Permissions.k_SceneUnderstandingCoarsePermission}, but permission was not granted.");
                }
#endif // UNITY_ANDROID
            }

            public override void Stop() {}

            public override void Destroy() => NativeApi.Destroy();

            public override unsafe TrackableChanges<XRBoundingBox> GetChanges(XRBoundingBox defaultBoundingBox, Allocator allocator)
            {
                NativeApi.GetChanges(
                    out var addedPtr, out var addedCount,
                    out var updatedPtr, out var updatedCount,
                    out var removedPtr, out var removedCount,
                    out var elementSize);
                try
                {
                    return new TrackableChanges<XRBoundingBox>(
                        addedPtr, addedCount,
                        updatedPtr, updatedCount,
                        removedPtr, removedCount,
                        defaultBoundingBox, elementSize,
                        allocator);
                }
                finally
                {
                    NativeApi.ClearChanges();
                }
            }

            public XRResultStatus TrySetBoundingBoxDetectionMode(BoundingBoxClassifications detectionMode)
            {
                XRResultStatus result = new XRResultStatus();
                NativeApi.TrySetBoundingBoxDetectionMode(detectionMode, ref result);
                return result;
            }

            public BoundingBoxClassifications GetBoundingBoxDetectionMode()
            {
                return NativeApi.GetBoundingBoxDetectionMode();
            }

            public BoundingBoxClassifications GetSupportedBoundingBoxDetectionModes()
            {
                return NativeApi.GetSupportedBoundingBoxDetectionModes();
            }

            [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
            static void RegisterDescriptor()
            {
                XRBoundingBoxSubsystemDescriptor.Register(new XRBoundingBoxSubsystemDescriptor.Cinfo
                {
                    id = k_SubsystemId,
                    providerType = typeof(AndroidOpenXRBoundingBoxPovider),
                    subsystemTypeOverride = typeof(AndroidOpenXRBoundingBoxSubsystem),
                    supportsClassification = true,
                });
            }

            static class NativeApi
            {
                [DllImport(Constants.k_ARFoundationLibrary, EntryPoint = "UnityOpenXRAndroid_BoundingBox_Create")]
                public static extern void Create();

                [DllImport(Constants.k_ARFoundationLibrary, EntryPoint = "UnityOpenXRAndroid_BoundingBox_Destroy")]
                public static extern void Destroy();

                [DllImport(Constants.k_ARFoundationLibrary, EntryPoint = "UnityOpenXRAndroid_BoundingBox_GetChanges")]
                public static extern unsafe void GetChanges(
                    out void* addedPtr, out int addedCount,
                    out void* updatedPtr, out int updatedCount,
                    out void* removedPtr, out int removedCount,
                    out int elementSize);

                [DllImport(Constants.k_ARFoundationLibrary, EntryPoint = "UnityOpenXRAndroid_BoundingBox_ClearChanges")]
                public static extern void ClearChanges();

                [DllImport(Constants.k_ARFoundationLibrary, EntryPoint = "UnityOpenXRAndroid_BoundingBox_GetBoundingBoxDetectionMode")]
                public static extern BoundingBoxClassifications GetBoundingBoxDetectionMode();

                [DllImport(Constants.k_ARFoundationLibrary, EntryPoint = "UnityOpenXRAndroid_BoundingBox_GetSupportedBoundingBoxDetectionModes")]
                public static extern BoundingBoxClassifications GetSupportedBoundingBoxDetectionModes();

                [DllImport(Constants.k_ARFoundationLibrary, EntryPoint = "UnityOpenXRAndroid_BoundingBox_TrySetBoundingBoxDetectionMode")]
                public static extern void TrySetBoundingBoxDetectionMode(BoundingBoxClassifications detectionMode, ref XRResultStatus resultStatus);
            }
        }
    }
}
