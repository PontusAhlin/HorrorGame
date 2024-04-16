# Upgrading to version 2.0.1 of Adaptive Performance

This page describes how to upgrade from an older version of Adaptive Performance to version 2.0.1.

## Upgrading from Adaptive Performance 1.x.x

There are 3 breaking changes which require manual adoption.

### Startup Settings
The `StartupSettings` class is removed in 2.0. Please remove your code and use the Adaptive Performance settings in the Editor instead.

If you need to access the settings via scripts during startup you can see the `AdaptivePerformanceSettings.cs` in the **Environment Sample** for more information.

### Performance Level Control
`IDevicePerformanceLevelControl` signature has changed and requires to pass in a parameter references for CPU and GPU level.

**Note:** we recommend to use the `Automatic Performance` feature instead of adjusting the levels of CPU and GPu on your own.

### Test System
The TestAdaptivePerformanceSystem was replaces by the SimulatorSubsystem.
