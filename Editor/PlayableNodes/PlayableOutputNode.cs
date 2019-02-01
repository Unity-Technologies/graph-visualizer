using System;
using System.Text;
using UnityEngine.Playables;
using UnityEditor.Playables;

namespace GraphVisualizer
{
    public class PlayableOutputNode : SharedPlayableNode
    {
        public PlayableOutputNode(PlayableOutput content)
            : base(content, content.GetWeight(), true)
        {
        }

        public override Type GetContentType()
        {
            PlayableOutput po = PlayableOutput.Null;
            try
            {
                po = (PlayableOutput) content;
            }
            catch
            {
                // Ignore.
            }

            return po.IsOutputValid() ? po.GetPlayableOutputType() : null;
        }

        public override string GetContentTypeShortName()
        {
            // Remove the extra Playable at the end of the Playable types.
            string shortName = base.GetContentTypeShortName();
            string cleanName = RemoveFromEnd(shortName, "PlayableOutput") + "Output";
            return string.IsNullOrEmpty(cleanName) ? shortName : cleanName;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine(InfoString("Handle", GetContentTypeShortName()));

            var po = (PlayableOutput) content;
            if (po.IsOutputValid())
            {
#if UNITY_2019_1_OR_NEWER
                sb.AppendLine(InfoString("Name", po.GetEditorName()));
#endif
                sb.AppendLine(InfoString("IsValid", po.IsOutputValid()));
                sb.AppendLine(InfoString("Weight", po.GetWeight()));
#if UNITY_2018_2_OR_NEWER
                sb.AppendLine(InfoString("SourceOutputPort", po.GetSourceOutputPort()));
#else
                sb.AppendLine(InfoString("SourceInputPort", po.GetSourceInputPort()));
#endif
            }

            return sb.ToString();
        }
    }
}
