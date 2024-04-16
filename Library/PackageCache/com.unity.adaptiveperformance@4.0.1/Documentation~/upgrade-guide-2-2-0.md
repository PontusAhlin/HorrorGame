# Upgrading to version 2.2.0 of Adaptive Performance

This page describes how to upgrade from an older version of Adaptive Performance to version 2.2.0.

## Upgrading from Adaptive Performance 2.x.x

There are also obsolete APIs which are auto updated with the Unity Script Updater:

The AdaptivePerformanceScalerSettings change on of the property members from 'AdaptiveShadowCascades => AdaptiveShadowCascade';

There are breaking changes which require manual adoption if you changed previously non-exposed scaler properties in the their settings file.

### Auto Upgrade
Adaptive Performance will upgrade the scaler settings file once you open the setting UI panel on the appropriate platform.

Properties effected are:

* scale
* minBound
* maxBound

If you modified any of those properties manually the auto upgrader will not be able to upgrade them and Unity prints folowing message in the console to inform you which properties it could not upgrade so you can change them manually:

```csharp
"[Adaptive Performance] Upgraded Adaptive Performance Settings but did not upgrade modified AdaptiveMSAA.maxBound"
```

Following scaler settings need upgrading if auto upgrade fails:

* AdaptiveFramerate.scale from -1 to 1
* AdaptiveBatching.scale from -1 to 1
* AdaptiveBatching.minBound from -1 to 0
* AdaptiveBatching.maxBound from -1 to 1
* AdaptiveLOD.scale from -1 to 1
* AdaptiveLOD.minBound from -1 to 0.4f
* AdaptiveLOD.maxBound from -1 to 1
* AdaptiveLut.scale from -1 to 1
* AdaptiveLut.minBound from -1 to 0
* AdaptiveLut.maxBound from -1 to 1
* AdaptiveMSAA.scale from -1 to 1
* AdaptiveMSAA.minBound from -1 to 0
* AdaptiveMSAA.maxBound from -1 to 1
* AdaptiveShadowDistance.scale from -1 to 1
* AdaptiveShadowDistance.minBound from -1 to 0
* AdaptiveShadowDistance.maxBound from -1 to 1
* AdaptiveShadowmapResolution.scale from -1 to 1
* AdaptiveShadowmapResolution.minBound from -1 to 0
* AdaptiveShadowmapResolution.maxBound from -1 to 1
* AdaptiveShadowQuality.scale from -1 to 1
* AdaptiveShadowQuality.minBound from -1 to 0
* AdaptiveShadowQuality.maxBound from -1 to 1
* AdaptiveSorting.scale from -1 to 1
* AdaptiveSorting.minBound from -1 to 0
* AdaptiveSorting.maxBound from -1 to 1
* AdaptiveTransparency.scale from -1 to 1
* AdaptiveTransparency.minBound from -1 to 0
* AdaptiveTransparency.maxBound from -1 to 1

-1 is used if a setting is not in use. With the new scaler settings, Unity uses those values and they need to be replaced with their functional value.
