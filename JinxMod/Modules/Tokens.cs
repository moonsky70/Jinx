﻿using JinxMod.SkillStates;
using R2API;
using System;

namespace JinxMod.Modules
{
    internal static class Tokens
    {
        //<color=#c9aa71> tan
        //<color=#ffffff> white
        //<color=#f68835> orange
        //<color=#d62d20> red
        //<color=#AF1AAF> purple
        //<color=#F5EE99> yellow
        //<color=#1F995C> green
        internal static void AddTokens()
        {
            #region Jinx
            string prefix = JinxPlugin.DEVELOPER_PREFIX + "_JINX_BODY_";

            string desc = "Jinx is a skilled fighter who makes use of a wide arsenal of weaponry to take down his foes.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Sword is a good all-rounder while Boxing Gloves are better for laying a beatdown on more powerful foes." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Pistol is a powerful anti air, with its low cooldown and high damage." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Roll has a lingering armor buff that helps to use it aggressively." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Bomb can be used to wipe crowds with ease." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left, searching for a new identity.";
            string outroFailure = "..and so he vanished, forever a blank slate.";

            LanguageAPI.Add(prefix + "NAME", "Jinx");
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "The Chosen One");
            LanguageAPI.Add(prefix + "LORE", "sample lore");
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Get Excited!");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", "Whenever Jinx scores a <color=#c9aa71>takedown</color> against Bosses, Champions, or Elites, she gains <color=#F5EE99>175% bonus movement speed</color> which decays over 6 seconds.");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_POWPOW_NAME", "Pow-Pow");
            LanguageAPI.Add(prefix + "PRIMARY_POWPOW_DESCRIPTION",
                "Basic attacks with Pow-Pow deal " + $"<color=#f68835>3x{100f * PowPow.damageCoefficient}% damage</color>" + " and generate a <color=#c9aa71>stack</color> of Rev'd up for 2.5 seconds, stacking up to 3 times, with the duration refreshing on subsequent attacks with Pow-Pow." + Environment.NewLine
            + "Rev'd up grants <color=#f68835>30% bonus attack speed.</color>");

            LanguageAPI.Add(prefix + "PRIMARY_FISHBONES_NAME", "Fishbones");
            LanguageAPI.Add(prefix + "PRIMARY_FISHBONES_DESCRIPTION",
              "Basic attacks with Fishbones deal " + $"<color=#f68835>{100f * FishBones.damageCoefficient}% damage</color>" + " to the primary target as well as surrounding enemies." + Environment.NewLine
            + "Jinx can Rocket Jump with Fishbones." + Environment.NewLine
            + "Jinx nullifies fall damage if she's knocked back by her own Rocket.");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_GUN_NAME", "Zap");
            LanguageAPI.Add(prefix + "SECONDARY_GUN_DESCRIPTION", Helpers.shockingPrefix + "Jinx fires a shock blast that deals " + $"<color=#f68835>{100f * Zap.damageCoefficient}% damage.</color> ");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_ROLL_NAME", "Switcharoo");
            LanguageAPI.Add(prefix + "UTILITY_ROLL_DESCRIPTION", "Jinx switches between Pow-Pow, her minigun, and Fishbones, her rocket launcher." + Environment.NewLine
            + "Switcharoo reset's Pow-Pow's and Fishbone's attack timer." + Environment.NewLine
            + "Jinx can Rocket Jump with Fishbones." + Environment.NewLine
            + "Jinx nullifies fall damage if she's knocked back by her own Rocket.");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_BOMB_NAME", "Super Mega Death Rocket!");
            LanguageAPI.Add(prefix + "SPECIAL_BOMB_DESCRIPTION", "Jinx fires a rocket in the target direction, The rocket explodes upon colliding with an enemy or terrain, dealing damage to them and all enemies in an area around them." + Environment.NewLine
            + $"<color=#f68835>{100f * MegaRocket.damageCoefficient}% - {100f * (MegaRocket.damageCoefficient * 2)}% damage</color> depending on projectile travel time <color=#d62d20>(+35% of target's missing health)</color>" + Environment.NewLine
            + "Bonus damage based on <color=#1F995C>missing health</color> is based on each unit's own <color=#1F995C>missing health.</color>");
            #endregion
            #endregion
        }
    }
}