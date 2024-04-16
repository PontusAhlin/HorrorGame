# Adaptive Performance provider guide

Adaptive Performance requires platform providers to supply it with necessary data. Loaders, such as the [Samsung (Android)](https://docs.unity3d.com/Packages/com.unity.adaptiveperformance.samsung.android@latest/index.html) provider, are generally provided via their own package, with the exception of the [Device Simulator (Desktop)](simulator.md) plugin.

## Lifecycle management

The Adaptive Performance management code enables you to manage the lifecycle of Adaptive Performance providers without the need for boilerplate code. The `AdaptivePerformanceManagerSettings` class provides a scriptable object that an app can use to start, stop, initialize, and deinitialize a set of subsystems defined in an `AdaptivePerformanceLoader` instance. Your Adaptive Performance provider uses this as the main driver.

Providers must create a subclass of `AdaptivePerformanceLoader` to make a loader available for their particular runtime scheme.

The `AdaptivePerformanceLoader` interface looks like this:

```csharp
public abstract class AdaptivePerformanceLoader : ScriptableObject
{
    public virtual bool Initialize() { return false; }

    public virtual bool Start() { return false; }

    public virtual bool Stop() { return false; }

    public virtual bool Deinitialize() { return false; }

    public abstract T GetLoadedSubsystem<T>() where T : class, ISubsystem;

    public abstract ISubsystem GetDefaultSubsystem();

    public abstract IAdaptivePerformanceSettings GetSettings();
}
```

To handle subsystem management in a type-safe manner, derive from the `AdaptivePerformanceLoaderHelper` class. For an example, see `/Editor/DeviceSimulator/Management/SimulatorProviderLoader.cs`.

An `AdaptivePerformanceLoader` is a [ScriptableObject](https://docs.unity3d.com/Manual/class-ScriptableObject.html), which means you can create one or more instances of it. Each `AdaptivePerformanceLoader` subclass defines the subsystems and their load order, and manages the set of subsystems they require.

Add all the `AdaptivePerformanceLoader` instances you created to the **Loaders** property of the **Adaptive Performance Manager Settings**, and arrange them in the order you want them to load. When initializing Loaders, **Adaptive Performance Manager Settings** calls each `AdaptivePerformanceLoader` instance it has a reference to, in the order you specify, and attempts to initialize each one. The first loader to initialize successfully becomes the active loader, and Unity stops all further attempts to initialize other loaders. Once that happens, you can query the static `AdaptivePerformanceManagerSettings.ActiveLoader` instance to access the active loader. If all loaders fail to initialize, Unity sets `activeLoader` to null.

Scene-based automatic lifecycle management hooks into the following `MonoBehaviour` callback points:

|Callback|Lifecycle step|
|---|---|
|OnEnable|Find the first loader that initialized successfully and set `ActiveLoader`.|
|Start|Start all subsystems.|
|OnDisable|Stop all subsystems.|
|OnDestroy|Deinitialize all subsystems and remove the `ActiveLoader` instance.|

Application lifetime-based automatic lifecycle management hooks into the following callback points:

|Callback|Lifecycle step|
|---|---|
|Runtime initialization after assemblies loaded|Find the first Loader that succeeds initialization and set `ActiveLoader`.|
|Runtime initialization before splash screen displays|Start all subsystems.|
|OnDisable|Stop all subsystems.|
|OnDestroy|Deintialize all subsystems and remove the `ActiveLoader` instance.|

## Configuring build and runtime settings through Project Settings

A provider might need additional settings to help manage build issues or runtime configuration. To do this, add an `AdaptivePerformanceConfigurationData` attribute to a ScriptableObject, and define a set of properties you want to surface to allow users to control configuration. Unity displays configuration options in the **Adaptive Performance** section of the **Project Settings** window.

Unity manages the lifecycle of one instance of the class marked with the attribute through the [EditorBuildSettings](https://docs.unity3d.com/ScriptReference/EditorBuildSettings.html) config object API. If you don't provide a dedicated UI, configuration settings are displayed in the **Project Settings** window using the standard **Scriptable Object** UI Inspector. You can create a custom **Editor** for your configuration settings type, which then replaces the standard Inspector in the **Project Settings** window.

The provider needs to handle getting the settings from `EditorUserBuildSettings` into the built application. You can do this with a custom build processing script. If you only need to make sure that you have access to the same settings at runtime, you can derive from `AdaptivePerformanceBuildHelper<T>`. This is a generic abstract base class that takes the build settings stored in `EditorUserBuildSettings` and gets them into the built application for runtime access.

The simplest build script for your package would look like this:

```csharp
public class MyBuildProcessor : AdaptivePerformanceBuildHelper<MySettings>
{
    public override string BuildSettingsKey { get { return "MyPackageSettingsKey"; } }
}
```

You can override the build processing steps from `IPreprocessBuildWithReport` and `IPostprocessBuildWithReport`, but make sure you call to the base class implementation. If you don’t, your settings won't transfer to the built application.

```csharp
public class MyBuildProcessor : AdaptivePerformanceBuildHelper<MySettings>
{
    public override string BuildSettingsKey { get { return "MyPackageSettingsKey"; } }

    public override void OnPreprocessBuild(BuildReport report)
    {
        base.OnPreprocessBuild(report);
        // Do your work here
    }

    public override void OnPostprocessBuild(BuildReport report)
    {
        base.OnPreprocessBuild(report);
        // Do your work here
    }
}
```

If you want to support different settings per platform at build time, you can override `UnityEngine.Object SettingsForBuildTargetGroup(BuildTargetGroup buildTargetGroup)` and use the `buildTargetGroup` attribute to retrieve the appropriate platform settings. By default, this method uses the key associated with the settings instance to copy the entire settings object from `EditorUserBuildSettings` to `PlayerSettings`.

```csharp
public class MyBuildProcessor : AdaptivePerformanceBuildHelper<MySettings>
{
    public override string BuildSettingsKey { get { return "MyPackageSettingsKey"; } }

    public override UnityEngine.Object SettingsForBuildTargetGroup(BuildTargetGroup buildTargetGroup)
    {
        // Get platform specific settings and return them. Use something like the following
        // for simple settings data that isn't platform specific.
        UnityEngine.Object settingsObj = null;
        EditorBuildSettings.TryGetConfigObject(BuildSettingsKey, out settingsObj);
        if (settingsObj == null || !(settingsObj is T))
            return null;

        return settingsObj;
    }
}
```

## Package metadata

Your provider must provide metadata information for it to be usable by the Adaptive Performance provider management system. Your provider must implement the following interfaces:

* IAdaptivePerformancePackage
* IAdaptivePerformancePackageMetadata
* IAdaptivePerformanceLoaderMetadata

The system will use .NET reflection to find all types implementing the **IAdaptivePerformancePackage** interface. It will then attempt to instantiate each one and populate the metadata store with the information provided by each instance.

## Example: Simple, minimal package information setup

```
    class MyPackage : IAdaptivePerformancePackage
    {
        private class MyLoaderMetadata : IAdaptivePerformanceLoaderMetadata
        {
            public string loaderName { get; }
            public string loaderType { get; }
            public List<BuildTargetGroup> supportedBuildTargets { get; }
        }

        private class MyPackageMetadata : IAdaptivePerformancePackageMetadata
        {
            public string packageName { get; }
            public string packageId { get; }
            public string settingsType { get; }
            public List<IAdaptivePerformanceLoaderMetadata> loaderMetadata { get; }
        }

        private static IAdaptivePerformancePackageMetadata s_Metadata = new MyPackageMetadata(){
                packageName = "My AdaptivePerformance Provider",
                packageId = "my.AdaptivePerformance.package",
                settingsType = "My.Package.MyPackageSettings",
                loaderMetadata = new List<IAdaptivePerformanceLoaderMetadata>() {
                new MyLoaderMetadata() {
                        loaderName = "My Loader",
                        loaderType = "My.Package.MyLoader",
                        supportedBuildTargets = new List<BuildTargetGroup>() {
                            BuildTargetGroup.Standalone,
                            BuildTargetGroup.Android,
                            BuildTargetGroup.iOS
                        }
                    },
                }
            };

        public IAdaptivePerformancePackageMetadata metadata => s_Metadata;

        public bool PopulateNewSettingsInstance(ScriptableObject obj)
        {
            MyPackageSettings packageSettings = obj as MyPackageSettings;
            if (packageSettings != null)
            {
                // Do something here if you need to...
            }
            return false;
        }
    }
```

## Package initialization

Implementing the Package Metadata allows the Adaptive Performance provider management system to automatically create and initialize your loaders and settings instances. The system will pass any new instances of your settings to the `PopulateNewSettingsInstance` method to allow your provider to initialize the new instance data after it's created, if needed.

## Installing the Adaptive Performance provider management

Adaptive Performance includes the provider management, so you shouldn't need to install it. If you do need to install it, follow the instructions in the [Package Manager documentation](https://docs.unity3d.com/Packages/com.unity.package-manager-ui@latest/index.html).

### Adaptive Performance Provider Management Content

This version of Adaptive Performance contains the following:

|**Feature**|**Description**|
|:---|:---|
|**AdaptivePerformanceManagerSettings**|**ScriptableObject** that you can use to manage `AdaptivePerformanceLoader` instances and their lifecycle.|
|**AdaptivePerformanceLoader**|Base class all loaders should derive from. It provides a basic API that the **AdaptivePerformanceManagerSettings** can use to manage lifecycle, and a simple API you can use to request specific subsystems from the loader.|
|**AdaptivePerformanceConfigurationData**|Attribute that allows for build and runtime settings to be hosted in the **Adaptive Performance** section of the **Project Settings** window. All instances use the name supplied in the script as part of the attribute. The management feature uses the **EditorBuildSettings** config object API, stored with the key provided in the attribute, to maintain and manage the lifecycle for one instance of the build settings. To access the configuration settings instance, retrieve the instance associated with the chosen key (as set in the attribute) from **EditorBuildSettings**.|
|**AdaptivePerformancePackageInitializationBase**|Helper class to derive from that simplifies package initialization. Helps to create default instances of the package's `AdaptivePerformanceLoader` and default settings when you install the package. Initialization only runs once, and you shouldn't depend on the user to create the specified instances on their own.|
|**AdaptivePerformanceBuildHelper**|Abstract class useful for handling some of the boilerplate around moving settings from the Editor to the runtime. If you derive from this class and specify the appropriate settings type, the system moves settings of that type from `EditorUserBuildSettings` to `PlayerSettings` so they can be used at runtime.|
|**AdaptivePerformanceGeneralSettings**|Contains settings that apply to all Adaptive Performance Providers, rather than any single provider.|
