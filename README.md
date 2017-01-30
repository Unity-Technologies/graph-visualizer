# PlayableGraph Visualizer #
## Introduction ##
The PlayableGraph Visualizer window can be used to display any *PlayableGraph*.
The tool can be used in both Play and Edit mode and will always reflect the current state of the graph.
Playable Handles in the graph are represented by colored nodes, varying according to their type. Wire color intensity indicates the local weight of the blending.
## Usage ##
- Copy the content of this repos into a Unity Project.
- Open any scene that contains at least one *PlayableGraph*.
- Register your *PlayableGraph* with the method GraphVisualizerClient.Show(PlayableGraph, string).
- Open the Timeline Visualizer in **Window > PlayableGraph Visualizer**.
- Select the *PlayableGraph* to display in the top-left combo box.
- Click on a Node to display more information about the associated Playable Handle.
## Notes ##
- This tool was previously named Timeline Visualizer, but was renamed as we are refactoring it to support *PlayableGraph* stored in different types of component.
- If your *PlayableGraph* is only available in Play mode, you will not be able to see it in Edit mode.
