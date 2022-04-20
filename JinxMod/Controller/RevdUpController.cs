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
        private float stopwatch = 0f;

        private void Awake()
        {
            this.body = base.GetComponent<CharacterBody>();
        }

        public void AddStack()
        {
            if (NetworkServer.active)
            {
                if (this.body.GetBuffCount(revdUp) < 3)
                {
                    this.body.AddBuff(revdUp);
                    stopwatch = 0;
                }

            }
        }

        public void RemoveStack()
        {
            if (NetworkServer.active)
            {
                this.body.RemoveBuff(revdUp);
                stopwatch = 0;
            }
        }
        private void FixedUpdate()
        {
            if (this.body.GetBuffCount(revdUp) > 0)
            {
                if (stopwatch < duration)
                {
                    stopwatch += Time.fixedDeltaTime;
                }
                if (stopwatch > duration)
                {
                    RemoveStack();
                }
            }
        }
    }
}
