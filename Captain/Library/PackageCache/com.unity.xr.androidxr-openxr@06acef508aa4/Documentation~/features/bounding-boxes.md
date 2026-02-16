---
uid: androidxr-openxr-bounding-boxes
---
# Bounding boxes

This page is a supplement to the AR Foundation [Bounding box detection](xref:arfoundation-bounding-box-detection) manual. The following sections only contain information about APIs where Google's Android XR runtime exhibits unique behavior.

[!include[](../snippets/arf-docs-tip.md)]

> [!IMPORTANT]
> You must ensure to configure the appropriate [Permissions](#permissions) to use bounding box detection features on Android XR.

Bounding boxes on Android XR are a predetermined set of objects that are recognized, detected, and tracked by Google's Android XR runtime. They are also referred to as Object Trackables.

## Permissions

AR Foundation's bounding box detection feature requires an Android system permission on the Android XR runtime. Your user must grant your app the `android.permission.SCENE_UNDERSTANDING_COARSE` or `android.permission.SCENE_UNDERSTANDING_FINE` permission before it can track bounding box data.

To avoid permission-related errors at runtime, set up your scene with the AR Bounding Box Manager component disabled, then enable it only after the required permission is granted.

## Bounding box classifications

This package maps Android XR's native object trackable label component to AR Foundation's [BoundingBoxClassifications](xref:UnityEngine.XR.ARFoundation.ARBoundingBox.classifications).

Refer to the table below to understand the mapping between AR Foundation's classifications and Android XR's bounding box labels:

| AR Foundation Label | Android XR label |
| :------------------ | :--------------- |
| Couch               |                  |
| Table               |                  |
| Bed                 |                  |
| Lamp                |                  |
| Plant               |                  |
| Screen              |                  |
| Storage             |                  |
| Bathtub             |                  |
| Chair               |                  |
| Dishwasher          |                  |
| Fireplace           |                  |
| Oven                |                  |
| Refrigerator        |                  |
| Sink                |                  |
| Stairs              |                  |
| Stove               |                  |
| Toilet              |                  |
| WasherDryer         |                  |
| Keyboard            | KEYBOARD         |
| Mouse               | MOUSE            |
| Laptop              | LAPTOP           |
| Other               | UNKNOWN          |

> [!NOTE]
> A blank label indicates that Android XR doesn't support the corresponding bounding box classification.

### Filter by bounding box classification

Android XR enables you to filter objects so that the device only tracks bounding boxes with specified classifications.

To set which bounding boxes should be tracked, use [TrySetBoundingBoxDetectionMode](xref:UnityEngine.XR.OpenXR.Features.Android.AndroidOpenXRBoundingBoxSubsystem.TrySetBoundingBoxDetectionMode(UnityEngine.XR.ARSubsystems.BoundingBoxClassifications)). To check which bounding boxes the device is currently tracking, you can use [GetBoundingBoxDetectionMode](xref:UnityEngine.XR.OpenXR.Features.Android.AndroidOpenXRBoundingBoxSubsystem.GetBoundingBoxDetectionMode). To check which bounding box classifications the platform supports, query with [GetSupportedBoundingBoxDetectionModes](xref:UnityEngine.XR.OpenXR.Features.Android.AndroidOpenXRBoundingBoxSubsystem.GetSupportedBoundingBoxDetectionModes).

## Native pointer

[XRBoundingBox.nativePtr](xref:UnityEngine.XR.ARSubsystems.XRBoundingBox.nativePtr) values returned by this package contain a pointer to the following struct:

```c
typedef struct UnityXRNativeBoundingBox
{
    int version;
    void* boundingBoxPtr;
} UnityXRNativeBoundingBox;
```

Cast the `void* boundingBoxPtr` to an `XrTrackableANDROID` handle in C++ using the following example code:

```cpp
// Marshal the native bounding box data from the XRBoundingBox.nativePtr in C#
UnityXRNativeBoundingBox nativeBoundingBox;
XrTrackableANDROID androidObjectHandle = static_cast<XrTrackableANDROID>(nativeBoundingBox.boundingBoxPtr);
```

To learn more about native pointers and their usage, refer to [Extending AR Foundation](xref:arfoundation-extensions).
