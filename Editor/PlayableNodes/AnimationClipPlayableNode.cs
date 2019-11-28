using System.Text;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace GraphVisualizer
{
    public class AnimationClipPlayableNode : PlayableNode
    {
        public AnimationClipPlayableNode(Playable content, float weight = 1.0f)
            : base(content, weight)
        {
        }

        public override string GetLabel()
        {
            var p = (Playable)content;
            if (p.IsValid())
            {
                var acp = (AnimationClipPlayable)p;
                var clip = acp.GetAnimationClip();
                return clip ? clip.name : "(none)";
            }
            return "(invalid)";
        }

        public override bool TryGetProgress(out float progress)
        {
            var p = (Playable)content;
            if (p.IsValid())
            {
                var acp = (AnimationClipPlayable)p;
                var clip = acp.GetAnimationClip();
                if (clip != null)
                {
                    if (clip.isLooping)
                        progress = (float)((acp.GetTime() % clip.length) / clip.length);
                    else
                        progress = Mathf.Clamp01((float)(acp.GetTime() / clip.length));
                    return true;
                }
            }
            progress = 0f;
            return false;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine(base.ToString());

            var p = (Playable) content;
            if (p.IsValid())
            {
                var acp = (AnimationClipPlayable) p;
                var clip = acp.GetAnimationClip();
                sb.AppendLine(InfoString("Clip", clip ? clip.name : "(none)"));
                if (clip)
                {
                    sb.AppendLine(InfoString("ClipLength", clip.length));
                }
                sb.AppendLine(InfoString("ApplyFootIK", acp.GetApplyFootIK()));
                sb.AppendLine(InfoString("ApplyPlayableIK", acp.GetApplyPlayableIK()));
            }

            return sb.ToString();
        }
    }
}