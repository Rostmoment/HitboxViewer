using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HitboxViewer.Constants
{
    public static class UIConstants
    {
        public static readonly Color mainBackgroundColor = new Color(0.08f, 0.08f, 0.08f);
        public static readonly Color titleBackgroundColor = new Color(0.05f, 0.05f, 0.05f);

        public static readonly Color flagBackgroundColor = new Color(0.09f, 0.09f, 0.09f);
        public static readonly Color flagDescriptionColor = new Color(0.785f, 0.785f, 0.785f);

        public static readonly Color configBackgroundColor = new Color(0.09f, 0.09f, 0.09f); 
        public static readonly Color configDescriptionColor = new Color(0.785f, 0.785f, 0.785f);

        public static readonly Color greenButtonColor = new Color(0, 0.39f, 0);
        public static readonly Color redButtonColor = new Color(0.39f, 0, 0);

        public static readonly Color enabledToggleTextColor = Color.green;
        public static readonly Color disabledToggleTextColor = Color.red;

        public const int SPACING = 4;
        public const int CONFIG_SPACING = 17;
        public const int BLOCK_SPACING = 5;
    }
}
