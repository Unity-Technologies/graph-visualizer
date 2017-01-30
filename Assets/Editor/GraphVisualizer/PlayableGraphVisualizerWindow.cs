using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using GraphVisualizer;
using UnityEngine.Experimental.Director;
using UnityEditor.Animations;

public class PlayableGraphVisualizerWindow : EditorWindow, IHasCustomMenu
{
    private struct PlayableGraphInfo
    {
        public PlayableGraph graph;
        public string name;
    }

    private IGraphRenderer m_Renderer;
    private IGraphLayout m_Layout;

    private PlayableGraphInfo m_CurrentGraphInfo;
    private GraphSettings m_GraphSettings;
    private bool m_AutoScanScene = true;

    #region Configuration

    private static readonly float s_ToolbarHeight = 17f;
    private static readonly float s_DefaultMaximumNormalizedNodeSize = 0.8f;
    private static readonly float s_DefaultMaximumNodeSizeInPixels = 100.0f;
    private static readonly float s_DefaultAspectRatio = 1.5f;

    #endregion
    private PlayableGraphVisualizerWindow()
    {
        m_GraphSettings.maximumNormalizedNodeSize = s_DefaultMaximumNormalizedNodeSize;
        m_GraphSettings.maximumNodeSizeInPixels = s_DefaultMaximumNodeSizeInPixels;
        m_GraphSettings.aspectRatio = s_DefaultAspectRatio;
        m_GraphSettings.showLegend = true;
        m_AutoScanScene = true;
    }

    [MenuItem("Window/PlayableGraph Visualizer")]
    public static void ShowWindow()
    {
        GetWindow<PlayableGraphVisualizerWindow>("Playable Graph Visualizer");
    }

    private PlayableGraphInfo GetSelectedGraphInToolBar(IList<PlayableGraphInfo> graphs, PlayableGraphInfo currentGraph)
    {
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.Width(position.width));

        List<string> options = new List<string>(graphs.Count);// = graphs.Select(d => d.ToString()).ToArray();
        foreach (var g in graphs)
        {
            options.Add(g.name);
        }

        int currentSelection = graphs.IndexOf(currentGraph);
        int newSelection = EditorGUILayout.Popup(currentSelection != -1 ? currentSelection : 0, options.ToArray(), GUILayout.Width(200));

        PlayableGraphInfo selectedDirector = new PlayableGraphInfo();
        if (newSelection != -1)
            selectedDirector = graphs[newSelection];

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        return selectedDirector;
    }

    private void ShowMessage(string msg)
    {
        GUIStyle centeredStyle = GUI.skin.GetStyle("Label");
        centeredStyle.alignment = TextAnchor.UpperCenter;
        int width = 15 * msg.Length;
        GUI.Label(new Rect(0.5f * (Screen.width - width), 0.5f * (Screen.height - 50), width, 50), msg, centeredStyle);
    }

    void Update()
    {
        // If in Play mode, refresh the graph each update.
        if (EditorApplication.isPlaying)
            Repaint();
    }

    void OnInspectorUpdate()
    {
        // If not in Play mode, refresh the graph less frequently.
        if (!EditorApplication.isPlaying)
            Repaint();
    }

    void OnGUI()
    {
        // Create a list of all the playable graphs extracted.
        IList<PlayableGraphInfo> graphInfos = new List<PlayableGraphInfo>();

        PlayableGraphInfo info;

        // If we requested, we extract automatically the PlayableGraphs from all the components
        // that are in the current scene.
        if (m_AutoScanScene)
        {
#if PLAYABLE_DIRECTOR
            // This code could be generalized, maybe if we added a IHasPlayableGraph Interface.
            IList<PlayableDirector> directors = FindObjectsOfType<PlayableDirector>();
            if (directors != null)
            {
                foreach (var director in directors)
                {
                    if (director.playableGraph.IsValid())
                    {
                        info.name = director.name;
                        info.graph = director.playableGraph;
                        graphInfos.Add(info);
                    }
                }
            }
#endif
#if ANIMATOR_5_6
            IList<Animator> animators = FindObjectsOfType<Animator>();
            if (animators != null)
            {
                foreach (var animator in animators)
                {
                    if (animator.playableGraph.IsValid())
                    {
                        info.name = animator.name;
                        info.graph = animator.playableGraph;
                        graphInfos.Add(info);
                    }
                }
            }
#endif
        }

        if (GraphVisualizerClient.GetGraphs() != null)
        {
            foreach (var clientGraph in GraphVisualizerClient.GetGraphs())
            {
                if (clientGraph.Key.IsValid())
                {
                    info.name = clientGraph.Value;
                    info.graph = clientGraph.Key;
                    graphInfos.Add(info);
                }
            }
        }

        // Early out if there is no graphs.
        if (graphInfos.Count == 0)
        {
            ShowMessage("No PlayableGraph in the scene");
            return;
        }

        GUILayout.BeginVertical();
        m_CurrentGraphInfo = GetSelectedGraphInToolBar(graphInfos, m_CurrentGraphInfo);
        GUILayout.EndVertical();

        if (!m_CurrentGraphInfo.graph.IsValid())
        {
            ShowMessage("Selected PlayableGraph is invalid");
            return;
        }

        var graph = new PlayableGraphVisualizer(m_CurrentGraphInfo.graph);
        graph.Refresh();

        if (graph.IsEmpty())
        {
            ShowMessage("Selected PlayableGraph is empty");
            return;
        }

        if (m_Layout == null)
            m_Layout = new ReingoldTilford();

        m_Layout.CalculateLayout(graph);

        var graphRect = new Rect(0, s_ToolbarHeight, position.width, position.height - s_ToolbarHeight);

        if (m_Renderer == null)
            m_Renderer = new DefaultGraphRenderer();

        m_Renderer.Draw(m_Layout, graphRect, m_GraphSettings);
    }

#region Custom_Menu

    public virtual void AddItemsToMenu(GenericMenu menu)
    {
        menu.AddItem(new GUIContent("Legend"), m_GraphSettings.showLegend, ToggleLegend);
        menu.AddItem(new GUIContent("Auto Scan Scene"), m_AutoScanScene, ToggleAutoScanScene);
    }
    void ToggleLegend()
    {
        m_GraphSettings.showLegend = !m_GraphSettings.showLegend;
    }
    void ToggleAutoScanScene()
    {
        m_AutoScanScene = !m_AutoScanScene;
    }

#endregion
}
