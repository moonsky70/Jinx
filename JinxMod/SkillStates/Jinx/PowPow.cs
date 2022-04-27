using EntityStates;
using JinxMod.Controller;
using RoR2;
using RoR2.Audio;
using System;
using UnityEngine;
using UnityEngine.Networking;
using static RoR2.BulletAttack;

namespace JinxMod.SkillStates
{
    public class PowPow : BaseSkillState
    {
        public static float damageCoefficient = 1.25f;
        public static float procCoefficient = .7f;
        public static float baseDuration = 1.0f;
        public static float force = 400f;
        public static float recoil = 1f;
        public static float range = 256f;
        public static GameObject tracerEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");

        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;

        private RevdUpController revdUpController;
        public Animator animator { get; private set; }

        private float bulletStopWatch;
        private int bulletCount = 3;
        private RocketController rocketController;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = PowPow.baseDuration / this.attackSpeedStat;
            this.fireTime = 0.1f * this.duration;
            base.characterBody.SetAimTimer(2f);
            this.muzzleString = "PowPowMuzzle";
            this.animator = base.GetModelAnimator();
            this.revdUpController = base.GetComponent<RevdUpController>();

            if (this.animator.GetBool("isMoving") || (!(this.animator.GetBool("isGrounded"))))
            {
                base.PlayAnimation("Gesture, Override", "powpowattack");
            }
            else if ((!(this.animator.GetBool("isMoving"))) && this.animator.GetBool("isGrounded"))
            {
                base.PlayAnimation("FullBody, Override", "powpowattack");
            }
            Util.PlaySound("Play_JinxPowPowShoot", base.gameObject);
            this.rocketController = base.GetComponent<RocketController>();
            if (this.rocketController)
            {
                this.rocketController.attacks++;
            }

            if (NetworkServer.active && base.characterBody.GetBuffCount(Modules.Buffs.revdUp) < 3)
            {
                base.characterBody.AddBuff(Modules.Buffs.revdUp);
                this.revdUpController.ResetStopWatch();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        private void Fire()
        {
            base.characterBody.AddSpreadBloom(1.5f);
            EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, base.gameObject, this.muzzleString, false);
            if (base.isAuthority)
            {
                Ray aimRay = base.GetAimRay();
                base.AddRecoil(-1f * PowPow.recoil, -2f * PowPow.recoil, -0.5f * PowPow.recoil, 0.5f * PowPow.recoil);

                new BulletAttack
                {
                    bulletCount = 1,
                    aimVector = aimRay.direction,
                    origin = aimRay.origin,
                    damage = PowPow.damageCoefficient * this.damageStat,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.Generic,
                    falloffModel = BulletAttack.FalloffModel.DefaultBullet,
                    maxDistance = PowPow.range,
                    force = PowPow.force,
                    hitMask = LayerIndex.CommonMasks.bullet,
                    minSpread = 0f,
                    maxSpread = base.characterBody.spreadBloomAngle,
                    isCrit = base.RollCrit(),
                    owner = base.gameObject,
                    muzzleName = muzzleString,
                    smartCollision = true,
                    procChainMask = default(ProcChainMask),
                    procCoefficient = procCoefficient,
                    radius = 0.75f,
                    sniper = false,
                    stopperMask = LayerIndex.CommonMasks.bullet,
                    weapon = null,
                    tracerEffectPrefab = PowPow.tracerEffectPrefab,
                    spreadPitchScale = 0f,
                    spreadYawScale = 0f,
                    queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                    hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol2.hitEffectPrefab,
                    hitCallback = bulletHitCallback,
                }.Fire();


            }
        }

        private bool bulletHitCallback(BulletAttack bulletAttack, ref BulletHit hitInfo)
        {
            var result = BulletAttack.defaultHitCallback(bulletAttack, ref hitInfo);
            if (hitInfo.hitHurtBox)
            {
                if(NetworkServer.active) PointSoundManager.EmitSoundServer(Modules.Assets.bulletHitSoundEvent.index, hitInfo.hitHurtBox.transform.position);
            }
            return result;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (bulletStopWatch < this.fireTime / bulletCount)
            {
                bulletStopWatch += Time.fixedDeltaTime;
            }
            if (base.fixedAge >= this.fireTime && bulletStopWatch > this.fireTime / bulletCount && bulletCount > 0)
            {
                bulletStopWatch = 0f;
                bulletCount--;
                if (!this.hasFired) this.Fire();
                if (bulletCount <= 0)
                {
                    this.hasFired = true;
                }
            }

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}