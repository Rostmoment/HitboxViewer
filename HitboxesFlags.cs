using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HitboxViewer
{
    [Flags]
    public enum HitboxesFlags
    {
        None = 0,
        Trigger = 1 << 0,
        NotTrigger = 1 << 1,
        SphereBoxNavMeshObstacle = 1 << 2,
        BoxNavMeshObstacle = 1 << 3
    }
}
