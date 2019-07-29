using System.Collections.Generic;
using UnityEngine.Playables;

// Bridge between runtime and editor code: the graph created in runtime code can call GraphVisualizerClient.Show(...)
// and the EditorWindow will register itself with the client to display any available graph.
public class GraphVisualizerClient
{
    private static GraphVisualizerClient s_Instance;
    private List<PlayableGraph> m_Graphs = new List<PlayableGraph>();
    private Dictionary<PlayableGraph, string> m_GraphNames = new Dictionary<PlayableGraph, string>();

    public static GraphVisualizerClient instance
    {
        get
        {
            if (s_Instance == null)
                s_Instance = new GraphVisualizerClient();
            return s_Instance;
        }
    }

    ~GraphVisualizerClient()
    {
        m_Graphs.Clear();
    }

    public static void Show(PlayableGraph graph)
    {
#if UNITY_EDITOR
        Show(graph, graph.GetEditorName());
#else
        Show(graph, null);
#endif
    }

    public static void Show(PlayableGraph graph, string name)
    {
        if (!instance.m_Graphs.Contains(graph))
        {
            instance.m_Graphs.Add(graph);
        }
        instance.m_GraphNames[graph] = name;
    }

    public static void Hide(PlayableGraph graph)
    {
        instance.m_Graphs.Remove(graph);
        instance.m_GraphNames.Remove(graph);
    }

    public static void ClearGraphs()
    {
        instance.m_Graphs.Clear();
    }

    public static IEnumerable<PlayableGraph> GetGraphs()
    {
        return instance.m_Graphs;
    }

    public static string GetName(PlayableGraph graph)
    {
        if (instance.m_GraphNames.TryGetValue(graph, out var name))
        {
            return name;
        }
        return null;
    }
}
