# Adaptive Performance Visual Scripting

After installing Adaptive Performance, Unity's [Visual Scripting](https://docs.unity3d.com/2021.2/Documentation/Manual/com.unity.visualscripting.html) system includes [units](https://docs.unity3d.com/Packages/com.unity.visualscripting@latest/index.html?subfolder=/manual/vs-understanding-units.html) you can use to access Adaptive Performance metrics without implementing anything in C#.

When you install the Visual Scripting package, Unity automatically activates the Adaptive Performance units. When you add Adaptive Performance to a project with existing Visual Scripting graphs, you must recompile the units. To do this, select **Edit** &gt; **Project Settings** &gt; **Visual Scripting** &gt; **Node Library** &gt; **Regenerate Units**.

![Visual Scripting Regenerate Node setting.](Images/VisualScripting/vs-regenerate.png)

After Unity regenerates Adaptive Performance units, you can add new units in your graph. To do this, right-click on the background of any script graph. You can find the units in the Adaptive Performance submenu.

![Visual Scripting Fuzzy entries.](Images/VisualScripting/vs-fuzzy.png)

## Units

This section contains information on the Adaptive Performance-related units.

![Adaptive Performance Visual Scripting units overview.](Images/VisualScripting/vs-overview.png)

After you install Adaptive Performance, the **Visual Scripting Fuzzy finder** will display the following additional units:

### Thermal

You can find the thermal nodes in the fuzzy search under AdaptivePerformance/Thermal.

Use the On Thermal Metric event unit to receive updates when the thermal warning levels change.

![Adaptive Performance thermal metric event unit.](Images/VisualScripting/vs-onthermal.png)

For read-only properties, the Thermal Metric unit includes the temperature level and trend as well as the warning levels.

![Adaptive Performance thermal unit.](Images/VisualScripting/vs-thermal.png)

For more information about thermals see the [user guide](user-guide.md#device-thermal-state-feedback).

### Performance

There are many different performance units available. You can find them in the fuzzy search under AdaptivePerformance/Performance.

#### Bottleneck

Use the On Bottleneck event unit to receive updates when the state of a bottleneck changes.

![Adaptive Performance bottleneck even unit.](Images/VisualScripting/vs-bottleneck.png)

For more information about the bottleneck feature see the [user guide](user-guide.md#performance-bottleneck).

#### Frametiming

The Frame Timing unit provides different frametime metrics.

![Adaptive Performance frametiming unit.](Images/VisualScripting/vs-frametiming.png)

For more information about the frametimings see the [user guide](user-guide.md#frame-timing).

##### FPS

The FPS (Frames per Second) items provides with an accurate representation of how many frames per seconds are rendered.

![Adaptive Performance frametiming unit.](Images/VisualScripting/vs-fps.png)

#### Clusterinfo

The Cluster Info unit provides information about CPU cores.

![Adaptive Performance clusterinfo unit.](Images/VisualScripting/vs-clusterinfo.png)

For more information about the Cluster Info feature see the [user guide](user-guide.md#cluster-info).

#### Performance Levels

Use the On Performance Level event unit to recieve updates when CPU or GPU levels change.
![Adaptive Performance on performance levels event unit.](Images/VisualScripting/vs-performancelevels.png)

To actively change performance levels use the Set Performance Level unit.

![Adaptive Performance set performance levels unit.](Images/VisualScripting/vs-setperformancelevels.png)

To see the current CPU and GPU performance levels use the Get Performance Level unit.

![Adaptive Performance performance levels read only unit.](Images/VisualScripting/vs-getperformancelevels.png)

For more information about boost mode see the [user guide](user-guide.md#configuring-cpu-and-gpu-performance-levels).

#### Boost

Use the On Boost event unit to receive updates when the CPU or GPU boost starts or ends.

![Adaptive Performance on boost event unit.](Images/VisualScripting/vs-boost.png)

To activate the CPU or GPU boost use the Set Boost unit.

![Adaptive Performance boost unit.](Images/VisualScripting/vs-setboost.png)

To see the current status of the CPU or GPU boost use the Get Boost unit.

![Adaptive Performance boost unit.](Images/VisualScripting/vs-getboost.png)

For more information about boost mode see the [user guide](user-guide.md#boost-mode).

### Indexer and Scalers

To see the current thermal or performance action or when the next evaluation happens use the Get Indexer Data unit.

![Adaptive Performance scaler unit.](Images/VisualScripting/vs-indexer.png)

Use the On Level Scaler event unit to recieve updates when and to which level a scaler changes.

![Adaptive Performance scaler unit.](Images/VisualScripting/vs-scaler.png)

For more information about the individual scalers see the [user guide](user-guide.md#indexer-and-scalers).
