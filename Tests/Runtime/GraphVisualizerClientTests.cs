using NUnit.Framework;
using System.Linq;
using UnityEngine.Playables;

class GraphVisualizerClientTest
{
    [TearDown]
    public void TearDown()
    {
        // Clear graphs between tests, otherwise graphs are still referenced across tests.
        GraphVisualizerClient.ClearGraphs();
    }

    private static PlayableGraph CreatePlayableGraph(string name)
    {
        var graph = PlayableGraph.Create(name);
        ScriptPlayableOutput.Create(graph, "output");
        return graph;
    }

    [Test]
    public void CanShowGraph()
    {
        var graph1 = CreatePlayableGraph("test1");
        var graph2 = CreatePlayableGraph("test2");

        GraphVisualizerClient.Show(graph1);
        var graphs = GraphVisualizerClient.GetGraphs().ToArray();

        Assert.That(graphs.Length, Is.EqualTo(1));
        Assert.That(graphs[0].GetEditorName(), Is.EqualTo(graph1.GetEditorName()));

        GraphVisualizerClient.Show(graph2);
        graphs = GraphVisualizerClient.GetGraphs().ToArray();

        Assert.That(graphs.Length, Is.EqualTo(2));
        Assert.That(graphs[0].GetEditorName(), Is.EqualTo(graph1.GetEditorName()));
        Assert.That(graphs[1].GetEditorName(), Is.EqualTo(graph2.GetEditorName()));

        graph1.Destroy();
        graph2.Destroy();
    }

    [Test]
    public void CannotShowSameGraphTwice()
    {
        var graph1 = CreatePlayableGraph("test1");

        GraphVisualizerClient.Show(graph1);
        var graphs = GraphVisualizerClient.GetGraphs().ToArray();

        Assert.That(graphs.Length, Is.EqualTo(1));

        graph1.Destroy();
    }

    [Test]
    public void CanHideGraph()
    {
        var graph1 = CreatePlayableGraph("test1");
        var graph2 = CreatePlayableGraph("test2");

        GraphVisualizerClient.Show(graph1);
        GraphVisualizerClient.Show(graph2);
        var graphs = GraphVisualizerClient.GetGraphs().ToArray();

        Assert.That(graphs.Length, Is.EqualTo(2));
        Assert.That(graphs[0].GetEditorName(), Is.EqualTo(graph1.GetEditorName()));
        Assert.That(graphs[1].GetEditorName(), Is.EqualTo(graph2.GetEditorName()));

        GraphVisualizerClient.Hide(graph1);
        graphs = GraphVisualizerClient.GetGraphs().ToArray();

        Assert.That(graphs.Length, Is.EqualTo(1));
        Assert.That(graphs[0].GetEditorName(), Is.EqualTo(graph2.GetEditorName()));

        graph1.Destroy();
        graph2.Destroy();
    }

    [Test]
    public void CanClearGraphs()
    {
        var graph1 = CreatePlayableGraph("test1");
        var graph2 = CreatePlayableGraph("test2");

        GraphVisualizerClient.Show(graph1);
        GraphVisualizerClient.Show(graph2);
        var graphs = GraphVisualizerClient.GetGraphs().ToArray();

        Assert.That(graphs.Length, Is.EqualTo(2));

        GraphVisualizerClient.ClearGraphs();
        graphs = GraphVisualizerClient.GetGraphs().ToArray();

        Assert.That(graphs.Length, Is.EqualTo(0));

        graph1.Destroy();
        graph2.Destroy();
    }
}
