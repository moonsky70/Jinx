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
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", "Whenever Jinx scores a takedown against an enemy within 3 seconds of damaging them, she gains 175% bonus movement speed which decays over 6 seconds");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_POWPOW_NAME", "Pow-Pow");
            LanguageAPI.Add(prefix + "PRIMARY_POWPOW_DESCRIPTION",
                "Basic attacks with Pow-Pow generate a stack of Rev'd up for 2.5 seconds, stacking up to 3 times, with the duration refreshing on subsequent attacks with Pow-Pow." + Environment.NewLine
            +   "Rev'd up grants 30% bonus attack speed.");

            LanguageAPI.Add(prefix + "PRIMARY_FISHBONES_NAME", "Fishbones");
            LanguageAPI.Add(prefix + "PRIMARY_FISHBONES_DESCRIPTION",
                "Basic attacks with Fishbones deal 800% damage to the primary target as well as surrounding enemies." + Environment.NewLine
            + "Jinx can Rocket Jump with Fishbones." + Environment.NewLine
            + "Jinx nullifies fall damage if she's knocked back by her own Rocket.");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_GUN_NAME", "Zap");
            LanguageAPI.Add(prefix + "SECONDARY_GUN_DESCRIPTION", "Jinx fires a shock blast that deals 1600% damage.");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_ROLL_NAME", "Switcharoo");
            LanguageAPI.Add(prefix + "UTILITY_ROLL_DESCRIPTION", "Jinx switches between Pow-Pow, her minigun, and Fishbones, her rocket launcher." + Environment.NewLine
            + "Switcharoo reset's Pow-Pow's and Fishbone's attack timer." + Environment.NewLine
            + "Jinx can Rocket Jump with Fishbones." + Environment.NewLine
            + "Jinx nullifies fall damage if she's knocked back by her own Rocket.");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_BOMB_NAME", "Bomb");
            LanguageAPI.Add(prefix + "SPECIAL_BOMB_DESCRIPTION", "Bomb.");
            #endregion
            #endregion

            LanguageAPI.Add("JINX_CONQUEROR_NAME", "<color=#ffa700>Conqueror</color>");
            LanguageAPI.Add("JINX_CONQUEROR_DESC", "<color=#c9aa71>Successful attacks & abilties</color> against enemies grant <color=#ffffff>1</color> stack of conqueror up to 12 stacks. Each stack of Conqueror grants <color=#f68835>0.6</color> <color=#d62d20>(+0.045 every 4 levels)</color> bonus base damage. While fully stacked you <color=#c9aa71>heal</color> for <color=#008744>6% of any damage from abilities dealt to enemies.</color>");

            LanguageAPI.Add("JINX_LETHAL_NAME", "<color=#ffa700>Lethal Tempo</color>");
            LanguageAPI.Add("JINX_LETHAL_DESC", "<color=#c9aa71>Successful attacks & abilties</color> against enemies grant <color=#ffffff>1</color> stack of lethal tempo up to 6 stacks. Gain <color=#f68835>10%</color> bonus attack speed for each stack up to <color=#f68835>60%</color> bonus attack speed at maximum stacks.");

            LanguageAPI.Add("JINX_PHASE_RUSH_NAME", "<color=#ffa700>Phase Rush</color>");
            LanguageAPI.Add("JINX_PHASE_RUSH_DESC", "<color=#c9aa71>Successful attacks & abilties</color> generate <color=#c9aa71>1</color> stack against enemies. Applying <color=#ffffff>3</color> stacks to a target within a 4 second period grants you <color=#f68835>30%</color> <color=#d62d20>(+5% every 4 levels)</color> bonus <color=#c9aa71>movement speed</color> for 3 seconds. Grants the bonus <color=#c9aa71>movement speed</color> on kill.");

            LanguageAPI.Add("JINX_ELECTROCUTE_NAME", "<color=#ffa700>Electrocute</color>");
            LanguageAPI.Add("JINX_ELECTROCUTE_DESC", "<color=#c9aa71>Successful attacks & abilties</color> generate <color=#c9aa71>1</color> stack against enemies. Applying <color=#ffffff>3</color> stacks to a target within a 3 second period causes them to be struck by lightning after a 1-second delay, dealing them <color=#f68835>600%</color> <color=#d62d20>(+75% every 4 levels)</color> damage. Electrocute has a 5 second cooldown per target.");
        }
    }
}