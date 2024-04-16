# Changelog
All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [4.0.1] - 2022-06-15

### Added
- Fix bug with scaler not being initialized with settings from the Editor UI properly when using profiles.
- Fix bug when building with IL2CPP and scaler types getting stripped out.
- Fix bug with manual lifetime tests failing occasionally.
- Fix bug with test provider and settings showing up in Project Settings.

## [4.0.0] - 2022-01-10

### Added
- Layer Culling Scaler
- Decals Scaler
- Visual Scripting
- Custom Scaler
- Physics Scaler
- Verified support for 2022 LTS

### Changed
- Fix case 1373192: warning 'IAdaptivePerformanceSettings.k_AssetVersion' when building the project.
- Change bottleneck profiler colors to reflect warning states correctly.
- Change boost documentation and timing values in the simulator to reflect the boost mode timing of 10s instead of the previous 15 seconds as it changes on the driver side.
- Fix frequency change error case (e.g. when boost is activated). Does not retry every frame. Manual retry is required.
- Fix simulator where temperature level and trend would not get updated when thermal action was updated.

## [3.0.0] - 2021-05-04

### Added
- Profiler Support
- Startup Boost Mode
- Verified support for 2021.2 and minimum support 2021.2 LTS+. Please use Adaptive Performance 1.x for earlier versions supporting 2018.3+ and Adaptive Performance 2.x for earlier versions supporting 2019.4+.

## [2.2.1] - 2021-06-06

### Changed
- Indexer Thermal action returns StateAction.Decrease and StateAction.FastDecrease instead of StateAction.Increase at 0.1 and 0.5 of thermal trend when not in throttling or throttling imminent to activate scalers sooner than Throttling immindent state.
- Scalers do not use switch-case to determine their value, instead use min, max scale and max level to calculate a scale and increment and apply it to their target when the level changes.
- Simulator integration for scalers. Offers now min, max scale and max level additionally to the level override to easily simulate the scaler behaviour.

### Added
- Fix FB 1321990: Sliders don't match their values when in the Simulator View Control Panel
- Scaler profiles
- Boost mode
- Cluster info
- Feature API
- Adaptive view distance
- More samples for boost, scaler profiles, cluster info, and adaptive view distance.

## [2.1.1] - 2021-02-03

### Added
- Fix FB 1304020: Min and Max values are not saved after it is typed and the labels are too long.
- Fix FB 1303986: When the settings view is narrowed "View license" text appears on top of the toggle.
- Fix FB 1297030: Scaler options Checkboxes are misaligned in the Simulator window.
- Fix FB 1296672: Errors are thrown on installing Adaptive Performance Package when Simulator window is open.

## [2.1.0] - 2020-10-12

### Added
- Added a transparency scaler.
- Added settings to control the Indexers thermal and performance actions when using the Device Simulator.
- Updated the version defines for the device simulator to support it in 2021.1 without package.

### Changed
- Session bugfixes for Adaptive Performance provider subsystem management.

## [2.0.2] - 2020-08-21

### Changed
- Provider downloader will now download latest available build instead of verified if verified version is below 2.0.0. This can happen on 2019 and 2020.1 as the verified package version is 1.x.

### Removed
- Folders and files which are not needed by Adaptive Performance from the package.

## [2.0.1] - 2020-08-10

### Changed
- Change capitalized sample path for Adaptive LUT to work on Linux.

### Removed
- Folders and files which are not needed by Adaptive Performance from the package.

## [2.0.0] - 2020-06-05

### Added
- Samples to show off different Adaptive Performance features.
- Indexer API which allows to create custom Scalers and a number of sample Scalers.
- Settings for Unified Settings Menu with provider installation via Settings.
- Adaptive Performance Simulator and Device Simulator extension to simulate Adaptive Performance events in the editor using the Device Simulator package.
- Verified support for 2020.2 and minimum support 2019 LTS+. Please use Adaptive Performance 1.x for earlier versions supporting 2018.3+.

## [1.2.2] - 2021-02-05

### Changed
- Fix package author to show up correctly in the package manager
- Sync version with Samsung package

## [1.2.0] - 2020-07-29

### Changed
- Updates to Subsystem Registry 1.2.0 to fix installation issues if internal subystem module is disabled.
- Update minimum required Unity version to 2019.4.

## [1.1.9] - 2020-07-23

### Changed
- Automatic Performance Mode: gpuUtilizationThreshold increased from 0.7 to 0.9 to increase effeciency.
- Automatic Performance Mode: gpuFactor increased from 0.72 to 0.92 to increase effeciency.
- Automatic Performance Mode: lower CPU and GPU levels at the same time instead of one at a time to increase effeciency and higher power savings.
- Changed Documentation to make clear that changing CPU and GPU levels is risky and the Automatic Performance Mode should be used instead.
- Fixed Automatic Performance Control flag to respect the function and not be read only anymore.
- Inrease GPU Active time Ratio.

## [1.1.6] - 2020-04-29

### Changed
- Fix Analytics system error with unloaded subsystem.

## [1.1.0] - 2019-11-07

### Changed
- Fixed initial version log to represent the correct version (subsystem)
- Fixed non reachable code warning.
- Fixed .net 3.5 breaking warning of unused variables and unassigned variables.

### Added
- Analytics events for subsystem manager and thermal status.

## [1.0.1] - 2019-08-29

### Changed
- Subsystem API to ensure compatibility with Subsystems in Unity 2019.3
- The default performance levels are to the maximum levels in automatic and manual mode

## [1.0.0] - 2019-08-19

### This is the first release of *Adaptive Performance*.
