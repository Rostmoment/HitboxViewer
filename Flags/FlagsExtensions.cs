using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HitboxViewer.Flags
{
    public static class FlagsExtensions
    {
        private static readonly Dictionary<HitboxesFlags, string> flagsDescriptions = new Dictionary<HitboxesFlags, string>()
        {
            [HitboxesFlags.Trigger] = "Show where isTrigger equals to true",
            [HitboxesFlags.NotTrigger] = "Show where isTrigger equals to false",
            [HitboxesFlags.BoxNavMeshObstacle] = "Show boxes",
            [HitboxesFlags.CapsuleNavMeshObstacle] = "Show capsules",
        };
        public static readonly HitboxesFlags[] all = Enum.GetValues(typeof(HitboxesFlags)).Cast<HitboxesFlags>().Where(f => f != HitboxesFlags.None).ToArray();


        public static string GetDescription(this HitboxesFlags flags) => flagsDescriptions[flags];

    }
}
