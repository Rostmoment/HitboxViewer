using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HitboxViewer.Flags
{
    public static class FlagsExtensions
    {
        private static readonly Dictionary<HitboxesFlags, string> descriptions = new Dictionary<HitboxesFlags, string>()
        {
            [HitboxesFlags.Trigger] = "Show where isTrigger equals to true",
            [HitboxesFlags.NotTrigger] = "Show where isTrigger equals to false",
            [HitboxesFlags.BoxNavMeshObstacle] = "Show boxes",
            [HitboxesFlags.CapsuleNavMeshObstacle] = "Show capsules",
        };

        private static readonly Dictionary<HitboxesFlags, string> names = new Dictionary<HitboxesFlags, string>()
        {
            [HitboxesFlags.NotTrigger] = "Not Trigger",
            [HitboxesFlags.BoxNavMeshObstacle] = "Box",
            [HitboxesFlags.CapsuleNavMeshObstacle] = "Capsule"
        };
        public static readonly HitboxesFlags[] all = Enum.GetValues(typeof(HitboxesFlags)).Cast<HitboxesFlags>().Where(f => f != HitboxesFlags.None).ToArray();

        extension(HitboxesFlags flag)
        {
            public bool IsSingle => (flag != HitboxesFlags.None) && ((flag & (flag - 1)) == 0);

            public string Description => descriptions[flag];
            public string Name
            {
                get
                {
                    if (names.TryGetValue(flag, out string name))
                        return name;

                    return flag.ToString();
                }
            }
        }
    }
}
