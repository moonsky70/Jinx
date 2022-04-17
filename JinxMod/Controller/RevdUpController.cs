using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace JinxMod.Controller
{
    public class RevdUpController : MonoBehaviour
    {
        public int maxStacks = 3;
        public float duration = 2.5f;
        public static float attackSpeedBonusCoefficient = 0.30f;
        private static BuffDef revdUp = Modules.Buffs.revdUp;
        private CharacterBody body;

        private void Awake()
        {
            this.body = base.GetComponent<CharacterBody>();
        }

        public void AddStack()
        {
            this.body.AddTimedBuff(revdUp, duration, maxStacks);
        }
    }
}
