---
uid: androidxr-openxr-faces
---
# Face tracking

This page supplements the AR Foundation [Face tracking](xref:arfoundation-face-tracking) manual. The following sections only contain information about APIs where Google's Android XR runtime exhibits platform-specific behavior.

[!include[](../snippets/arf-docs-tip.md)]

> [!IMPORTANT]
> You must ensure to configure the appropriate [Permissions](#permissions) to use face tracking features on Android XR.

## Optional feature support

Android XR implements the following optional features of AR Foundation's [XRFaceSubsystem](xref:UnityEngine.XR.ARSubsystems.XRFaceSubsystem):

| Feature | Descriptor Property | Supported |
| :------ | :------------------ | :----: |
| **Face pose** | [supportsFacePose](xref:UnityEngine.XR.ARSubsystems.XRFaceSubsystemDescriptor.supportsFacePose) |     |
| **Face mesh vertices and indices** | [supportsFaceMeshVerticesAndIndices](xref:UnityEngine.XR.ARSubsystems.XRFaceSubsystemDescriptor.supportsFaceMeshVerticesAndIndices) |     |
| **Face mesh UVs** | [supportsFaceMeshUVs](xref:UnityEngine.XR.ARSubsystems.XRFaceSubsystemDescriptor.supportsFaceMeshUVs) |     |
| **Face mesh normals** | [supportsFaceMeshNormals](xref:UnityEngine.XR.ARSubsystems.XRFaceSubsystemDescriptor.supportsFaceMeshNormals) |     |
| **Eye tracking** |  [supportsEyeTracking](xref:UnityEngine.XR.ARSubsystems.XRFaceSubsystemDescriptor.supportsEyeTracking) | Yes |
| **Blend Shapes** | [supportsBlendShapes](xref:UnityEngine.XR.ARSubsystems.XRFaceSubsystemDescriptor.supportsBlendShapes) | Yes |

> [!NOTE]
> Refer to AR Foundation [Face tracking platform support](xref:arfoundation-face-tracking-platform-support) for more information on the optional features of the Face subsystem.

## Face data

This platform exposes face data for the active user (the person wearing the headset). Currently, gaze and blend shape data is surfaced.

Gaze data is available within the [ARFace](xref:UnityEngine.XR.ARFoundation.ARFace) class, while blend shape data needs to be queried for separately using the [TryGetBlendShapes](xref:UnityEngine.XR.ARSubsystems.XRFaceSubsystem.TryGetBlendShapes) API.

New blend shape data might be available when the `ARFace` [updated](xref:UnityEngine.XR.ARFoundation.ARFace.updated) event is triggered.

> [!TIP]
> To ensure you keep your mesh renders up to date, update your mesh renderer each time the `ARFace` [updated](xref:UnityEngine.XR.ARFoundation.ARFace.updated) event is triggered.

You can use the [TryGetInwardRegionConfidences](xref:UnityEngine.XR.OpenXR.Features.Android.AndroidOpenXRFaceSubsystem.TryGetInwardRegionConfidences*) API to determine the accuracy of blend shape data per face region. A confidence value of at least `0.3` indicates acceptable blend shape data. To learn how Android XR groups blend shape locations into confidence regions, consult the [Android XR Face Tracking documentation](https://developer.android.com/develop/xr/openxr/extensions/XR_ANDROID_face_tracking).

To know which face is the inward avatar-eyes gaze object, cast an `XRFaceSubsystem` to an `AndroidOpenXRFaceSubsystem` and read its `inwardID` property.

## Permissions

AR Foundation's face tracking feature requires two Android system permissions on the Android XR runtime. Your user must grant your app the `android.permission.EYE_TRACKING_COARSE` and `android.permission.FACE_TRACKING` permissions before it can track face data.

To avoid permission-related errors at runtime, set up your scene with the AR Face Manager component disabled, then enable it only after the required permission is granted.

The following example code demonstrates how to handle required permissions and enable the AR Face Manager component:

[!code-cs[request_face_permission](../../Tests/Runtime/CodeSamples/PermissionSamples.cs#request_face_permission)]

For a code sample that involves multiple permissions in a single request, refer to the [Permissions](xref:androidxr-openxr-permissions) page.
