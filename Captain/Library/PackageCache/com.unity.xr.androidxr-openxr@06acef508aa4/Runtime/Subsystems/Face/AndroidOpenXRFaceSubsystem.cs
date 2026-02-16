using System;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Android;
using UnityEngine.Scripting;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.OpenXR.Features.Android
{
    /// <summary>
    /// The Android-OpenXR implementation of the <see cref="XRFaceSubsystem"/>.
    /// </summary>
    [Preserve]
    public sealed class AndroidOpenXRFaceSubsystem : XRFaceSubsystem
    {
        /// <summary>
        /// Do not call this directly. Call <c>Create</c> on a relevant <see cref="XRFaceSubsystemDescriptor"/> instead.
        /// </summary>
        public AndroidOpenXRFaceSubsystem() => instance = this;

        /// <summary>
        /// <see cref="TrackableId"/> for the face of the person wearing the headset.
        /// </summary>
        public TrackableId inwardID
        {
            get
            {
                var derivedProvider = provider as AndroidOpenXRFaceProvider;
                if (derivedProvider == null)
                    return TrackableId.invalidId;

                return derivedProvider.inwardID;
            }
        }

        internal bool TryGetEyeTrackingStates(out AndroidOpenXREyeTrackingStates states, TrackableId id)
        {
            var derivedProvider = provider as AndroidOpenXRFaceProvider;
            if (derivedProvider == null || id != derivedProvider.inwardID)
            {
                states = AndroidOpenXREyeTrackingStates.None;
                return false;
            }

            states = derivedProvider.eyeTrackingStates;
            return true;
        }

#if UNITY_ANDROID
        /// <summary>
        /// Attempts to get the blend shapes for the face of the person wearing the headset. This call may fail if no blend shape data can be found for the inward face.
        /// </summary>
        /// <param name="allocator">The allocator to use for the returned blend shapes native array.</param>
        /// <returns>A result of a new native array, allocated with the requested allocation strategy, describing
        /// the blend shapes for the inward face. You own the returned native array and are responsible for calling <c>Dispose</c>
        /// on it if you have opted for the `Allocator.Persistent` strategy.</returns>
        public Result<NativeArray<XRFaceBlendShape>> TryGetInwardBlendShapes(Allocator allocator)
        {
            return TryGetBlendShapes(inwardID, allocator);
        }
#endif // UNITY_ANDROID

        /// <summary>
        /// Attempts to get the region confidences associated with the facial blend shapes of the person wearing the headset. This call might fail if no confidence region data can be found for the inward face.
        /// </summary>
        /// <param name="allocator">The allocator to use for the returned region confidence native array.</param>
        /// <returns>A result of a new native array, allocated with the requested allocation strategy, containing confidence values
        /// corresponding to a <see cref="AndroidXRFaceConfidenceRegion"/> based on the index of the array. You own the returned native array and are responsible for calling <c>Dispose</c>
        /// on it if you have opted for the `Allocator.Persistent` strategy.</returns>
        public Result<NativeArray<float>> TryGetInwardRegionConfidences(Allocator allocator)
        {
            return ((AndroidOpenXRFaceProvider)provider).TryGetInwardRegionConfidences(allocator);
        }

        internal const string k_SubsystemId = "Android-Face";

        internal static AndroidOpenXRFaceSubsystem instance { get; private set; }

        class AndroidOpenXRFaceProvider : Provider
        {
            protected override bool TryInitialize()
            {
                if (!(OpenXRRuntime.IsExtensionEnabled(Constants.OpenXRExtensions.k_XR_ANDROID_eye_tracking) ||
                    OpenXRRuntime.IsExtensionEnabled(Constants.OpenXRExtensions.k_XR_ANDROID_face_tracking)))
                {
                    return false;
                }

                NativeApi.Create();
                return true;
            }

            public override void Start()
            {
#if UNITY_ANDROID
                if (!Permission.HasUserAuthorizedPermission(Constants.Permissions.k_EyeTrackingFinePermission)
                    && !Permission.HasUserAuthorizedPermission(Constants.Permissions.k_EyeTrackingCoarsePermission))
                {
                    Debug.LogWarning($"Eye tracking requires system eye tracking permission {Constants.Permissions.k_EyeTrackingCoarsePermission} (or {Constants.Permissions.k_EyeTrackingFinePermission}), but no permission was granted.");
                    return;
                }

                if (!Permission.HasUserAuthorizedPermission(Constants.Permissions.k_FaceTrackingPermission))
                {
                    Debug.LogWarning($"Face detection requires face tracking system permission {Constants.Permissions.k_FaceTrackingPermission}, but no permission was granted.");
                    return;
                }
#endif // UNITY_ANDROID

                m_InwardID = NativeApi.StartAndGetInwardID();
            }

            public override void Stop() { }

            public override void Destroy() => NativeApi.Destroy();

            public override unsafe TrackableChanges<XRFace> GetChanges(XRFace defaultFace, Allocator allocator)
            {
                NativeApi.AcquireChanges(
                    out var addedPtr, out var addedCount,
                    out var updatedPtr, out var updatedCount,
                    out var removedPtr, out var removedCount,
                    out var elementSize,
                    ref m_EyeTrackingStates);

                try
                {
                    return new TrackableChanges<XRFace>(
                        addedPtr, addedCount,
                        updatedPtr, updatedCount,
                        removedPtr, removedCount,
                        defaultFace, elementSize,
                        allocator);
                }
                finally
                {
                    NativeApi.ReleaseChanges();
                }
            }

#if UNITY_ANDROID
            public override Result<NativeArray<XRFaceBlendShape>> TryGetBlendShapes(TrackableId faceId, Allocator allocator)
            {
                XRResultStatus resultStatus;

                if (faceId != m_InwardID)
                {
                    Debug.LogError($"Provided TrackableId({faceId}) does not match inward face");
                    resultStatus = new XRResultStatus(XRResultStatus.StatusCode.ValidationFailure);
                    return new Result<NativeArray<XRFaceBlendShape>>(resultStatus, default);
                }

                resultStatus = NativeApi.TryGetInwardFaceBlendShapes(out var ptrNativeBlendShapesArray, out var sizeOfStruct, out var blendShapeCount);

                if (resultStatus.IsError())
                {
                    return new Result<NativeArray<XRFaceBlendShape>>(resultStatus, default);
                }

                unsafe
                {
                    var blendShapes = NativeCopyUtility.PtrToNativeArrayWithDefault<XRFaceBlendShape>(
                        default, (void*)ptrNativeBlendShapesArray, sizeOfStruct, blendShapeCount, allocator);

                    return new Result<NativeArray<XRFaceBlendShape>>(resultStatus, blendShapes);
                }
            }
#endif // UNITY_ANDROID

            public Result<NativeArray<float>> TryGetInwardRegionConfidences(Allocator allocator)
            {
                XRResultStatus resultStatus;

                if (!OpenXRRuntime.IsExtensionEnabled(Constants.OpenXRExtensions.k_XR_ANDROID_face_tracking))
                {
                    Debug.LogError($"{Constants.OpenXRExtensions.k_XR_ANDROID_face_tracking} must be enabled to query for region confidence data.");
                    resultStatus = new XRResultStatus(XRResultStatus.StatusCode.ProviderUninitialized);
                    return new Result<NativeArray<float>>(resultStatus, default);
                }

                resultStatus = NativeApi.TryGetInwardRegionConfidences(out var ptrRegionConfidencesArray, out var regionConfidencesCount);

                if (resultStatus.IsError())
                {
                    return new Result<NativeArray<float>>(resultStatus, default);
                }

                unsafe
                {
                    var nativeRegionConfidencesArray = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<float>(
                        (void*)ptrRegionConfidencesArray, regionConfidencesCount, Allocator.None);

                    var regionConfidences = new NativeArray<float>(regionConfidencesCount, allocator);
                    regionConfidences.CopyFrom(nativeRegionConfidencesArray);
                    return new Result<NativeArray<float>>(resultStatus, regionConfidences);
                }
            }

            internal TrackableId inwardID => m_InwardID;

            TrackableId m_InwardID;
            AndroidOpenXREyeTrackingStates m_EyeTrackingStates;
            internal AndroidOpenXREyeTrackingStates eyeTrackingStates => m_EyeTrackingStates;
            internal static AndroidOpenXRFaceSubsystem instance { get; private set; }

            [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
            static void RegisterDescriptor()
            {
                XRFaceSubsystemDescriptor.Register(new XRFaceSubsystemDescriptor.Cinfo
                {
                    id = k_SubsystemId,
                    providerType = typeof(AndroidOpenXRFaceProvider),
                    subsystemTypeOverride = typeof(AndroidOpenXRFaceSubsystem),
                    supportsEyeTracking = true,
                    supportsBlendShapes = true
                });
            }

            static class NativeApi
            {
                [DllImport(Constants.k_ARFoundationLibrary, EntryPoint = "UnityOpenXRAndroid_Face_Create")]
                public static extern void Create();

                [DllImport(Constants.k_ARFoundationLibrary, EntryPoint = "UnityOpenXRAndroid_Face_Destroy")]
                public static extern void Destroy();

                [DllImport(Constants.k_ARFoundationLibrary, EntryPoint = "UnityOpenXRAndroid_Face_StartAndGetInwardID")]
                public static extern TrackableId StartAndGetInwardID();

                [DllImport(Constants.k_ARFoundationLibrary, EntryPoint = "UnityOpenXRAndroid_Face_AcquireChanges")]
                public static extern unsafe void AcquireChanges(
                    out void* addedPtr, out int addedCount,
                    out void* updatedPtr, out int updatedCount,
                    out void* removedPtr, out int removedCount,
                    out int elementSize,
                    ref AndroidOpenXREyeTrackingStates eyeTrackingStates);

                [DllImport(Constants.k_ARFoundationLibrary, EntryPoint = "UnityOpenXRAndroid_Face_ReleaseChanges")]
                public static extern void ReleaseChanges();

                [DllImport(Constants.k_ARFoundationLibrary, EntryPoint = "UnityOpenXRAndroid_Face_TryGetInwardFaceBlendShapes")]
                public static extern XRResultStatus TryGetInwardFaceBlendShapes(
                    out IntPtr ptrBlendShapesArray,
                    out int sizeOfStruct,
                    out int blendShapeCount);

                [DllImport(Constants.k_ARFoundationLibrary, EntryPoint = "UnityOpenXRAndroid_Face_TryGetInwardRegionConfidences")]
                public static extern XRResultStatus TryGetInwardRegionConfidences(
                    out IntPtr ptrRegionConfidencesArray,
                    out int regionConfidencesCount);
            }
        }
    }
}
