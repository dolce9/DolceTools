// Assets/DolceTools/NDMF/DLTCommentStripper.cs
using UnityEngine;
using nadena.dev.ndmf;

namespace DolceTools.NDMF
{
    public class DLTCommentStripper : Plugin<DLTCommentStripper>
    {
        public override string QualifiedName => "dolcetools.strip-dltcomment";

        protected override void Configure()
        {
            InPhase(BuildPhase.Optimizing)
                .BeforePlugin("com.anatawa12.avatar-optimizer")
                .Run("Remove DLTComment components", ctx =>
                {
                    if (ctx.AvatarRootObject == null) return;

                    var targets = ctx.AvatarRootObject
                        .GetComponentsInChildren<DolceTools.DLTComment>(true);

                    int count = 0;

                    foreach (var c in targets)
                    {
                        if (c == null) continue;
                        Object.DestroyImmediate(c, true);
                        count++;
                    }

                    Debug.Log($"[DolceTools] Removed DLTComment: {count}");
                });
        }
    }
}