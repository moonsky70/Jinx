using JinxMod.SkillStates;
using JinxMod.SkillStates.BaseStates;
using System.Collections.Generic;
using System;

namespace JinxMod.Modules
{
    public static class States
    {
        internal static void RegisterStates()
        {
            Modules.Content.AddEntityState(typeof(Switcharoo));

            Modules.Content.AddEntityState(typeof(Shoot));

            Modules.Content.AddEntityState(typeof(Roll));

            Modules.Content.AddEntityState(typeof(ThrowBomb));
        }
    }
}