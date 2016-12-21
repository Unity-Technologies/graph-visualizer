using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using GraphVisualizer;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class TimelineVisualizerWindow : EditorWindow
{
    private IGraphRenderer m_Renderer;
    private IGraphLayout m_Layout;

    private PlayableDirector m_CurrentDirector;

    private static readonly float s_ToolbarHeight = 17f;

    private TimelineVisualizerWindow()
    {
            
    }

    [MenuItem("Window/Timeline Visualizer")]
    public static void ShowWindow()
    {
        GetWindow<TimelineVisualizerWindow>("Timeline Visualizer");
    }

    private PlayableDirector GetSelectedDirectorInToolBar(IList<PlayableDirector> directors, PlayableDirector currentDirector)
    {
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.Width(position.width));

        string[] options = directors.Select(d => d.gameObject.name).ToArray();

        int currentSelection = directors.IndexOf(currentDirector);
        int newSelection = EditorGUILayout.Popup(currentSelection != -1 ? currentSelection : 0, options, GUILayout.Width(200));

        PlayableDirector selectedDirector = null;
        if (newSelection != -1)
            selectedDirector = directors[newSelection];

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
        // Parse the scene for any Director.
        IList<PlayableDirector> directors = FindObjectsOfType<PlayableDirector>();

        if (directors == null || directors.Count == 0)
        {
            ShowMessage("No PlayableDirector in the scene");
            return;
        }

        GUILayout.BeginVertical();
        m_CurrentDirector = GetSelectedDirectorInToolBar(directors, m_CurrentDirector);
        GUILayout.EndVertical();

        if (m_CurrentDirector == null)
        {
            ShowMessage("Selected PlayableDirector is invalid");
            return;
        }

        if (m_CurrentDirector.playableAsset == null)
        {
            ShowMessage("Selected PlayableDirector is not associated to a Playable Graph");
            return;
        }

        var graph = new PlayableGraphVisualizer(m_CurrentDirector.playableGraph);
        graph.Refresh();

        if (m_Layout == null)
            m_Layout = new ReingoldTilford();

        m_Layout.CalculateLayout(graph);

        var graphRect = new Rect(0, s_ToolbarHeight, position.width, position.height - s_ToolbarHeight);

        if (m_Renderer == null)
            m_Renderer = new DefaultGraphRenderer();

        m_Renderer.Draw(m_Layout, graphRect);
    }
}
