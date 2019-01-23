# About PlayableGraph Visualizer

Use the *PlayableGraph Visualizer* package to have a visual representation of the
Playable graphs instantiated in the scene.

# Installing PlayableGraph Visualizer

To install this package, follow the instructions in the
[Package Manager documentation](https://docs.unity3d.com/Packages/com.unity.package-manager-ui@latest/index.html).

# Using PlayableGraph Visualizer

- Open the PlayableGraph Visualizer in **Window > Analysis > PlayableGraph Visualizer**
- Open any scene that contains at least one `PlayableGraph`
- In the top-left list, select the `PlayableGraph` to display in the window
- Click on the nodes to display more information about the associated Playable handle

_Note:_
- You can show just your `PlayableGraph` using `GraphVisualizerClient.Show(PlayableGraph)` in the code
- If your `PlayableGraph` is only available in Play mode, you will not be able to see it in Edit mode

# Technical details

## Requirements

This version of *PlayableGraph Visualizer* is compatible with the following versions of the Unity Editor:

* 2018.1 and later (recommended)

## Package contents

The following table indicates the structure of the package:

|Location|Description|
|---|---|
|`Editor/`|Contains the editor scripts for the new *PlayableGraph Visualizer* window.|
|`Runtime/GraphVisualizerClient.cs`|Contains the class allowing the user to register specific Playable graphs.|

