---
uid: androidxr-openxr-architecture
---
# Architecture

Understand the OpenXR extensions Android XR uses.

Android XR functions as both an OpenXR feature group and an AR Foundation [provider plug-in](xref:arfoundation-manual).

## About OpenXR

OpenXR is an open-source standard that defines an interface between XR apps and platform runtimes. The OpenXR specification contains two categories of features:

* Core features: present on every platform
* Extensions: optional and might not be implemented by some platforms.

Unity's [OpenXR Plug-in](xref:openxr-manual) integrates core features, while this package integrates Google’s Android-specific vendor extensions.

## OpenXR extensions

You can access Google’s Android XR OpenXR extensions in the Khronos Group [OpenXR Specification](https://registry.khronos.org/OpenXR/specs/1.1/html/xrspec.html) and Google's [Android XR developers](https://developer.android.com/develop/xr/openxr/extensions) documentation.

This package enables support for the following OpenXR extensions in your project:

| **Extension** | **Usage** | **Description** |
| :------------ | :-------- | :-------------- |
| [XR_ANDROID_depth_texture](https://developer.android.com/develop/xr/openxr/extensions/XR_ANDROID_depth_texture) (Android developer) | [Occlusion](xref:androidxr-openxr-occlusion) | Exposes raw and smooth depth for occlusion, hit tests, and other specific tasks that make use of accurate scene geometry, such as counterfeit face detection. |
| [XR_ANDROID_device_anchor_persistence](https://registry.khronos.org/OpenXR/specs/1.1/html/xrspec.html#XR_ANDROID_device_anchor_persistence) (Khronos) | [Anchors](xref:androidxr-openxr-anchors) | Allows the application to persist, retrieve, and un-persist anchors on the current device for the current user, across application and device sessions. The anchors are persisted per app, as identified by their Android package name. |
| [XR_ANDROID_eye_tracking](https://developer.android.com/develop/xr/openxr/extensions/XR_ANDROID_eye_tracking) (Android developer) | [Face tracking](xref:androidxr-openxr-faces) | Allows the application to get weights of blend shapes and render facial expressions in XR experiences. |
| [XR_ANDROID_face_tracking](https://developer.android.com/develop/xr/openxr/extensions/XR_ANDROID_face_tracking) (Android developer) | [Face tracking](xref:androidxr-openxr-faces) | Enables applications to get weights of blend shapes and render facial expressions in XR experiences. Provides the information needed to create realistic avatars and expressive representations of users in virtual space. |
|[XR_ANDROID_hand_mesh](https://developer.android.com/develop/xr/openxr/extensions/XR_ANDROID_hand_mesh) (Android developer) | [Hand mesh data](xref:androidxr-openxr-hand-mesh-data) |  Enables hand tracking inputs represented as a dynamic hand mesh. This extension is intended to provide vertex and index buffers for the mesh of a personalized representation of the user's hands. |
|[XR_ANDROID_light_estimation](https://developer.android.com/develop/xr/openxr/extensions/XR_ANDROID_light_estimation) (Android developer) | [Camera](xref:androidxr-openxr-camera) | Estimates the environmental lighting (including spherical harmonics) of a user's current environment. This extension allows the application to request data representing the lighting of the real-world environment around the headset. |
| [XR_ANDROID_passthrough_camera_state](https://registry.khronos.org/OpenXR/specs/1.1/html/xrspec.html#XR_ANDROID_passthrough_camera_state)	(Khronos) | [Camera](xref:androidxr-openxr-camera) | Passthrough cameras may take time to start up and are not immediately available. This extension lets applications know the current state of the passthrough camera. |
|[XR_ANDROID_performance_metrics](https://developer.android.com/develop/xr/openxr/extensions/XR_ANDROID_performance_metrics) (Android developer) | [Performance metrics](xref:androidxr-openxr-performance-metrics) | This extension provides APIs to enumerate and query various performance metrics counters of the current XR device, compositor and XR application. |
| [XR_ANDROID_raycast](https://registry.khronos.org/OpenXR/specs/1.1/html/xrspec.html#XR_ANDROID_raycast) (Khronos) | [Ray casts](xref:androidxr-openxr-raycasts) | This extension allows the application to perform ray casts against trackables in the environment. Raycasts are useful for detecting objects in the environment that are in the trajectory of a ray from a given origin. |
| [XR_ANDROID_trackables](https://registry.khronos.org/OpenXR/specs/1.1/html/xrspec.html#XR_ANDROID_trackables) (Khronos) | [Anchors](xref:androidxr-openxr-anchors),  [Bounding boxes](xref:androidxr-openxr-bounding-boxes), [Plane detection](xref:androidxr-openxr-plane-detection), [Ray casts](xref:androidxr-openxr-raycasts) | This extension allows the application to access trackables from the physical environment, and create anchors attached to a trackable. It also allows applications to create world-locked spatial anchors. |
| [XR_ANDROID_trackables_object](https://registry.khronos.org/OpenXR/specs/1.1/html/xrspec.html#XR_ANDROID_trackables_object) (Khronos) | [Bounding boxes](xref:androidxr-openxr-bounding-boxes) | This extension enables physical object tracking such as keyboards, mice, and other objects in the environment. |
| [XR_FB_display_refresh_rate](https://registry.khronos.org/OpenXR/specs/1.0/html/xrspec.html#XR_FB_display_refresh_rate) (Khronos) | [Display utilities](xref:androidxr-openxr-display-utilities) | On platforms which support dynamically adjusting the display refresh rate, application developers may request a specific display refresh rate in order to improve the overall user experience. |

## Additional resources

* [Android XR features](xref:androidxr-openxr-features)
* [Build with supported OpenXR extensions](https://developer.android.com/develop/xr/openxr/extensions) (Android XR developer documentation)