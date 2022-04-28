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
        public void ResetStopWatch()
        {
            stopwatch = 0f;
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
                    if (NetworkServer.active)
                    {
                        ResetStopWatch();
                        this.body.RemoveBuff(Modules.Buffs.revdUp);
                    }
                }
            }
            else
            {
                ResetStopWatch();
            }
        }
    }
}
