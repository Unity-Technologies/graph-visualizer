using UnityEngine;

namespace GraphVisualizer
{
    // Interface for rendering a tree layout to screen.
    public interface IGraphRenderer
    {
        void Draw(IGraphLayout graphLayout, Rect drawingArea);
        void Draw(IGraphLayout graphLayout, Rect drawingArea, NodeConstraints nodeConstraints);
    }

    // Customization of how the graph nodes should be rendered (size, distances and proportions)
    public struct NodeConstraints
    {
        // In layout units. If 1, node will be drawn as large as possible to avoid overlapping, and to respect aspect ratio
        public float maximumNormalizedNodeSize;

        // when the graph is very simple, the nodes can seem disproportionate relative to the graph. This limits their size
        public float maximumNodeSizeInPixels;

        // width / height; 1 represents a square node
        public float aspectRatio;
    }
}