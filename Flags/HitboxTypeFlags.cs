using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HitboxViewer.Flags
{
    public class HitboxTypeFlags
    {
        public HitboxTypeFlags(HitboxesFlags potential)
        {
            Enabled = HitboxesFlags.None;
            Potentional = potential;
        }

        public HitboxDefinition hitboxType;

        public HitboxesFlags Potentional { get; }
        public HitboxesFlags Enabled { get; private set; }


        public void Enable(HitboxesFlags flag)
        {
            BasePlugin.Logger.LogInfo($"Flag {flag} is set to enable");

            if (!Enabled.HasFlag(flag) && HasFlag(flag))
                Enabled |= flag;
        }

        public void Disable(HitboxesFlags flag)
        {
            BasePlugin.Logger.LogInfo($"Flag {flag} is set to disable");
            if (Enabled.HasFlag(flag) && HasFlag(flag))
                Enabled &= ~flag;
        }

        public void EnableAll()
        {
            BasePlugin.Logger.LogInfo("Enabling all");
            Enabled = Potentional;
        }
        public void DisableAll()
        {
            BasePlugin.Logger.LogInfo("Disabling all");
            Enabled = HitboxesFlags.None;
        }

        public void SetAll(bool enabled)
        {
            if (enabled)
                EnableAll();
            else
                DisableAll();
        }
        public void SetEnabled(bool enabled, HitboxesFlags flag)
        {
            if (enabled)
                Enable(flag);
            else
                Disable(flag);
        }

        /// <summary>
        /// True if flag is enabled
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public bool IsEnabled(HitboxesFlags flag) => Enabled.HasFlag(flag);
        /// <summary>
        /// True if has this flag as potentional
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public bool HasFlag(HitboxesFlags flag) => Potentional.HasFlag(flag);
    }
}
