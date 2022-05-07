﻿using BepInEx;
using JinxMod.Controller;
using JinxMod.Modules.Survivors;
using JinxMod.SkillStates;
using R2API;
using R2API.Utils;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Permissions;
using R2API.Networking;
using UnityEngine;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace JinxMod
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.xoxfaby.BetterUI", BepInDependency.DependencyFlags.SoftDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, MODNAME, MODVERSION)]
    [R2APISubmoduleDependency(new string[]
    {
        "PrefabAPI",
        "LanguageAPI",
        "SoundAPI",
        "DamageAPI",
        "RecalculateStatsAPI",
        nameof(NetworkingAPI)
    })]

    public class JinxPlugin : BaseUnityPlugin
    {
        // if you don't change these you're giving permission to deprecate the mod-
        //  please change the names to your own stuff, thanks
        //   this shouldn't even have to be said
        public const string MODUID = "com.Lemonlust.JinxMod";
        public const string MODNAME = "JinxMod";
        public const string MODVERSION = "1.0.7";

        // a prefix for name tokens to prevent conflicts- please capitalize all name tokens for convention
        public const string DEVELOPER_PREFIX = "Lemonlust";

        public static JinxPlugin instance;
        public static DamageAPI.ModdedDamageType jinxDamage;

        public static bool betterUIInstalled = false;

        private void Awake()
        {
            instance = this;
            try
            {
                if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.xoxfaby.BetterUI")) betterUIInstalled = true;

                jinxDamage = DamageAPI.ReserveDamageType();
                Log.Init(Logger);
                Modules.Assets.Initialize(); // load assets and read config
                Modules.Config.ReadConfig();
                Modules.States.RegisterStates(); // register states for networking
                Modules.Buffs.RegisterBuffs(); // add and register custom buffs/debuffs
                Modules.Projectiles.RegisterProjectiles(); // add and register custom projectiles
                Modules.Tokens.AddTokens(); // register name tokens
                //Modules.ItemDisplays.PopulateDisplays(); // collect item display prefabs for use in our display rules
                NetworkingAPI.RegisterMessageType<RefreshStacksMessage>();

                // survivor initialization
                new MyCharacter().Initialize();

                // now make a content pack and add it- this part will change with the next update
                new Modules.ContentPacks().Initialize();

                Hook();
                if (betterUIInstalled)
                {
                    AddBetterUI();
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message + " - " + e.StackTrace);
            }
        }
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private void AddBetterUI()
        {
            BetterUI.ProcCoefficientCatalog.AddSkill("JinxPowPow", "Pow-Pow!", PowPow.procCoefficient);
            BetterUI.ProcCoefficientCatalog.AddSkill("JinxFishBones", "Fishbones!", FishBones.procCoefficient);
            BetterUI.ProcCoefficientCatalog.AddSkill("JinxZap", "Zap!", Zap.procCoefficient);
            BetterUI.ProcCoefficientCatalog.AddSkill("JinxMegaRocket", "Super Mega Death Rocket!", MegaRocket.procCoefficient);
        }

        private void Hook()
        {
            RoR2.GlobalEventManager.onCharacterDeathGlobal += GlobalEventManager_onCharacterDeathGlobal;
            R2API.RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender.HasBuff(Modules.Buffs.revdUp))
            {
                args.attackSpeedMultAdd += ((sender.GetBuffCount(Modules.Buffs.revdUp) * RevdUpController.attackSpeedBonusCoefficient));
            }

            GetExcitedController getExcitedController = sender.GetComponent<GetExcitedController>();
            if (getExcitedController && sender.HasBuff(Modules.Buffs.getExcitedSpeedBuff))
            {
                var currentDuration = getExcitedController.currentDuration;
                args.moveSpeedMultAdd += Mathf.Max(Mathf.Min(currentDuration * .125f, 0.75f), 0f);
                args.attackSpeedMultAdd += .25f;
            }
        }

        private void GlobalEventManager_onCharacterDeathGlobal(DamageReport damageReport)
        {
            if (damageReport is null) return;
            if (damageReport.victimBody is null) return;
            if (damageReport.attackerBody is null) return;

            if (damageReport.victimTeamIndex != TeamIndex.Player && damageReport.attackerBody.bodyIndex == BodyCatalog.FindBodyIndex("JinxBody") && (damageReport.victimIsChampion || damageReport.victimIsBoss || damageReport.victimIsElite))
            {
                GetExcitedController getExcitedController = damageReport.attackerBody.GetComponent<GetExcitedController>();
                getExcitedController.GetExcited();
            }
        }
    }
}