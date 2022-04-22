using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace JinxMod.Controller
{
    public class GetExcitedController : MonoBehaviour
    {
        private BuffDef buffDef = Modules.Buffs.getExcitedSpeedBuff;
        public float baseDuration = 6f;
        public float decayRate = 1f;
        public float stopwatch;
        public float currentDuration = 6f;
        private CharacterBody body;

        private void Awake()
        {
            this.body = base.GetComponent<CharacterBody>();

        }
        public void GetExcited()
        {
            ResetTimer();
            if (this.body.HasBuff(buffDef))
            {
                return;
            }
            if (NetworkServer.active)
            {
                this.body.AddBuff(buffDef);
            }
        }

        private void ResetTimer()
        {
            stopwatch = 0f;
            currentDuration = 6f;
        }

        private void FixedUpdate()
        {
            if (this.body.HasBuff(buffDef))
            {
                stopwatch += Time.fixedDeltaTime;
                currentDuration -= Time.fixedDeltaTime;
                if (stopwatch > baseDuration)
                {
                    if (NetworkServer.active)
                    {
                        this.body.RemoveBuff(buffDef);
                        ResetTimer();
                    }
                }
            }
        }

    }
}
