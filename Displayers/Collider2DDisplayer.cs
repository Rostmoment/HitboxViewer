using HitboxViewer.Flags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HitboxViewer.Displayers
{
    public abstract class Collider2DDisplayer<T> : BaseDisplayer<T> where T : UnityEngine.Collider2D
    {
        public override HitboxesFlags HitboxFlags
        {
            get
            {
                if (GenericTarget.isTrigger)
                    return HitboxesFlags.Trigger;
                return HitboxesFlags.NotTrigger;
            }
        }
    }
}
