using System;
using System.Collections.Generic;
using System.Text;
using GraphVisualizer;
using UnityEngine.Experimental.Director;

public class PlayableGraphNode : Node
{
    public PlayableGraphNode(PlayableHandle content, float weight = 1, bool active = false)
        : base(content, weight, active)
    {
    }

    public override Type GetContentType()
    {
        Playable p = null;
        try
        {
            p = ((PlayableHandle)content).GetObject<Playable>();
        }
        catch
        {
            // Ignore.
        }
        return p == null ? null : p.GetType();
    }

    public override string GetContentTypeShortName()
    {
        // Remove the extra Playable at the end of the Playable types.
        string shortName = base.GetContentTypeShortName();
        string cleanName = RemoveFromEnd(shortName, "Playable");
        return string.IsNullOrEmpty(cleanName) ? shortName : cleanName;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.AppendLine(InfoString("Handle", GetContentTypeShortName()));

        var h = (PlayableHandle)content;

        sb.AppendLine(InfoString("IsValid", h.IsValid()));

        if (h.IsValid())
        {
            sb.AppendLine(InfoString("IsDone", h.isDone));
            sb.AppendLine(InfoString("InputCount", h.inputCount));
            sb.AppendLine(InfoString("OutputCount", h.outputCount));
            sb.AppendLine(InfoString("PlayState", h.playState));
            sb.AppendLine(InfoString("Speed", h.speed));
            sb.AppendLine(InfoString("Duration", h.duration));
            sb.AppendLine(InfoString("Time", h.time));
            //        sb.AppendLine(InfoString("Animation", h.animatedProperties));
        }

        return sb.ToString();
    }

    private static string InfoString(string key, double value)
    {
        return String.Format(
            ((Math.Abs(value) < 100000.0) ? "<b>{0}:</b> {1:#.###}" : "<b>{0}:</b> {1:E4}") , key, value);
    }

    private static string InfoString(string key, int value)
    {
        return String.Format("<b>{0}:</b> {1:D}", key, value);
    }

    private static string InfoString(string key, object value)
    {
        return "<b>" + key + ":</b> " + (value ?? "(none)");
    }

    private static string RemoveFromEnd(string str, string suffix)
    {
        if (str.EndsWith(suffix))
        {
            return str.Substring(0, str.Length - suffix.Length);
        }
        return str;
    }
}

public class PlayableGraphVisualizer : Graph
{
    private PlayableGraph m_PlayableGraph;

    public PlayableGraphVisualizer(PlayableGraph playableGraph)
    {
        m_PlayableGraph = playableGraph;
    }

    protected override void Populate()
    {
        if (!m_PlayableGraph.IsValid()) return;
        int roots = m_PlayableGraph.rootPlayableCount;
        for (int i = 0; i < roots; i++)
        {
            AddNodeHierarchy(CreateNodeFromPlayableHandle(m_PlayableGraph.GetRootPlayable(i), 1.0f));
        }
    }

    protected override IEnumerable<Node> GetChildren(Node node)
    {
        // Children are the PlayableHandle Inputs.
        return GetInputsNode((PlayableHandle)node.content);
    }

    private List<Node> GetInputsNode(PlayableHandle h)
    {
        var inputs = new List<Node>();
        for (int port = 0; port < h.inputCount; ++port)
        {
            PlayableHandle playableHandle = h.GetInput(port);
            if (playableHandle.IsValid())
            {
                float weight = h.GetInputWeight(port);
                Node node = CreateNodeFromPlayableHandle(playableHandle, weight);
                inputs.Add(node);
            }
        }
        return inputs;
    }

    private PlayableGraphNode CreateNodeFromPlayableHandle(PlayableHandle h, float weight)
    {
        return new PlayableGraphNode(h, weight, h.playState == PlayState.Playing);
    }

    private static bool HasValidOuputs(PlayableHandle h)
    {
        for (int port = 0; port < h.outputCount; ++port)
        {
            PlayableHandle playableHandle = h.GetOutput(port);
            if (playableHandle.IsValid())
            {
                return true;
            }
        }
        return false;
    }
}
