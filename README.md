# PlayableGraph Visualizer #
## Introduction ##
The PlayableGraph Visualizer window can be used to display any *PlayableGraph*.
The tool can be used in both Play and Edit mode and will always reflect the current state of the graph.
Playable Handles in the graph are represented by colored nodes, varying according to their type. Wire color intensity indicates the local weight of the blending.
## Setup ##
- Download the release that matches your current Unity version, or the latest if there your Unity version is more recent than the latest release.
- Copy the content of this repos Asset folder into a the Asset folder of an Unity Project.  You will need to do this for every project.
## Window ##
- You can open the Timeline Visualizer in **Window > PlayableGraph Visualizer**.
## Usage ##
- Open any scene that contains at least one *PlayableGraph*.
- Register your *PlayableGraph* with the method GraphVisualizerClient.Show(PlayableGraph, string).
- Select the *PlayableGraph* to display in the top-left combo box.
- Click on a Node to display more information about the associated Playable Handle.
## Notes ##
- This tool was previously named Timeline Visualizer, but was renamed as we are refactoring it to support *PlayableGraph* stored in different types of component.
- If your *PlayableGraph* is only available in Play mode, you will not be able to see it in Edit mode.
