using BepInEx;
using JinxMod.Controller;
using JinxMod.Modules.Survivors;
using R2API;
using R2API.Utils;
using RoR2;
using RoR2.Skills;
using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;
using UnityEngine;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace JinxMod
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, MODNAME, MODVERSION)]
    [R2APISubmoduleDependency(new string[]
    {
        "PrefabAPI",
        "LanguageAPI",
        "SoundAPI",
        "DamageAPI"
    })]

    public class JinxPlugin : BaseUnityPlugin
    {
        // if you don't change these you're giving permission to deprecate the mod-
        //  please change the names to your own stuff, thanks
        //   this shouldn't even have to be said
        public const string MODUID = "com.Lemonlust.JinxMod";
        public const string MODNAME = "JinxMod";
        public const string MODVERSION = "0.0.1";

        // a prefix for name tokens to prevent conflicts- please capitalize all name tokens for convention
        public const string DEVELOPER_PREFIX = "Lemonlust";

        public static JinxPlugin instance;
        public static DamageAPI.ModdedDamageType jinxDamage;

        private void Awake()
        {
            instance = this;
            jinxDamage = DamageAPI.ReserveDamageType();
            Log.Init(Logger);
            Modules.Assets.Initialize(); // load assets and read config
            Modules.Config.ReadConfig();
            Modules.States.RegisterStates(); // register states for networking
            Modules.Buffs.RegisterBuffs(); // add and register custom buffs/debuffs
            Modules.Projectiles.RegisterProjectiles(); // add and register custom projectiles
            Modules.Tokens.AddTokens(); // register name tokens
            //Modules.ItemDisplays.PopulateDisplays(); // collect item display prefabs for use in our display rules

            // survivor initialization
            new MyCharacter().Initialize();

            // now make a content pack and add it- this part will change with the next update
            new Modules.ContentPacks().Initialize();

            Hook();
        }

        private void Hook()
        {
            // run hooks here, disabling one is as simple as commenting out the line
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);

            // a simple stat hook, adds armor after stats are recalculated
            if (self)
            {
                float _level = Mathf.Floor(self.level / 4f);
                if (self.HasBuff(Modules.Buffs.revdUp))
                {
                    self.attackSpeed += self.attackSpeed * (self.GetBuffCount(Modules.Buffs.revdUp) * RevdUpController.attackSpeedBonusCoefficient);
                }

                if (self.HasBuff(Modules.Buffs.lethalBuff))
                {
                    float count = self.GetBuffCount(Modules.Buffs.lethalBuff);
                    self.attackSpeed += self.attackSpeed * (count * 0.10f);
                }

                if (self.HasBuff(Modules.Buffs.conquerorBuff))
                {
                    float count = self.GetBuffCount(Modules.Buffs.conquerorBuff);
                    self.damage += count * (0.6f + (_level * 0.045f));
                }

                if (self.HasBuff(Modules.Buffs.movementSpeedBuff))
                {
                    self.moveSpeed += self.moveSpeed * (0.30f + (_level * 0.05f));
                }
            }
        }
    }
}