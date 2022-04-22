using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace JinxMod.Modules
{
    public static class Buffs
    {
        // armor buff gained during roll
        internal static BuffDef armorBuff;
        internal static BuffDef revdUp;

        internal static BuffDef conquerorBuff;

        internal static BuffDef electrocuteDebuff;
        internal static BuffDef lethalBuff;
        internal static BuffDef movementSpeedBuff;
        internal static BuffDef getExcitedSpeedBuff;


        internal static BuffDef phaseRushDebuff;
        internal static void RegisterBuffs()
        {
            armorBuff = AddNewBuff("JinxArmorBuff", RoR2.LegacyResourcesAPI.Load<Sprite>("Textures/BuffIcons/texBuffGenericShield"), Color.white, false, false);
            revdUp = AddNewBuff("JinxRevdUp", Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("Rev_d_up"), Color.white, true, false);

            conquerorBuff = AddNewBuff("ConquerorBuff", Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("Conqueror_rune"), Color.white, true, false);

            lethalBuff = AddNewBuff("LethalBuff", Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("Lethal_Tempo_rune"), Color.white, true, false);

            phaseRushDebuff = AddNewBuff("PhaseRushDebuff", Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("Phase_Rush_rune"), Color.white, true, true);

            electrocuteDebuff = AddNewBuff("ElectrocuteDebuff", Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("Electrocute_rune"), Color.white, true, true);

            movementSpeedBuff = AddNewBuff("MovementSpeedBuff", Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("Phase_Rush_rune"), Color.white, true, false);

            getExcitedSpeedBuff = AddNewBuff("GetExcitedMovementSpeedBuff", Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("Jinx_Passive"), Color.white, true, false);

        }

        // simple helper method
        internal static BuffDef AddNewBuff(string buffName, Sprite buffIcon, Color buffColor, bool canStack, bool isDebuff)
        {
            BuffDef buffDef = ScriptableObject.CreateInstance<BuffDef>();
            buffDef.name = buffName;
            buffDef.buffColor = buffColor;
            buffDef.canStack = canStack;
            buffDef.isDebuff = isDebuff;
            buffDef.eliteDef = null;
            buffDef.iconSprite = buffIcon;

            Modules.Content.AddBuffDef(buffDef);

            return buffDef;
        }
    }
}