using EntityStates;
using RoR2;
using UnityEngine;

namespace JinxMod.SkillStates
{
    public class Shoot : BaseSkillState
    {
        public static float damageCoefficient = Modules.StaticValues.gunDamageCoefficient;
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.6f;
        public static float force = 400f;
        public static float recoil = 1f;
        public static float range = 256f;
        public static GameObject tracerEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");

        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;
        private float bulletStopWatch;
        private int bulletCount = 3;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = Shoot.baseDuration / this.attackSpeedStat;
            this.fireTime = 0.2f * this.duration;
            base.characterBody.SetAimTimer(2f);
            this.muzzleString = "Muzzle";

            base.PlayAnimation("LeftArm, Override", "ShootGun", "ShootGun.playbackRate", 1.8f);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        private void Fire()
        {
            base.characterBody.AddSpreadBloom(1.5f);
            EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, base.gameObject, this.muzzleString, false);
            Util.PlaySound("JinxShootPistol", base.gameObject);

            if (base.isAuthority)
            {
                Ray aimRay = base.GetAimRay();
                base.AddRecoil(-1f * Shoot.recoil, -2f * Shoot.recoil, -0.5f * Shoot.recoil, 0.5f * Shoot.recoil);

                new BulletAttack
                {
                    bulletCount = 1,
                    aimVector = aimRay.direction,
                    origin = aimRay.origin,
                    damage = Shoot.damageCoefficient * this.damageStat,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.Generic,
                    falloffModel = BulletAttack.FalloffModel.DefaultBullet,
                    maxDistance = Shoot.range,
                    force = Shoot.force,
                    hitMask = LayerIndex.CommonMasks.bullet,
                    minSpread = 0f,
                    maxSpread = 0f,
                    isCrit = base.RollCrit(),
                    owner = base.gameObject,
                    muzzleName = muzzleString,
                    smartCollision = false,
                    procChainMask = default(ProcChainMask),
                    procCoefficient = procCoefficient,
                    radius = 0.75f,
                    sniper = false,
                    stopperMask = LayerIndex.CommonMasks.bullet,
                    weapon = null,
                    tracerEffectPrefab = Shoot.tracerEffectPrefab,
                    spreadPitchScale = 0f,
                    spreadYawScale = 0f,
                    queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                    hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol2.hitEffectPrefab,
                }.Fire();
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (bulletStopWatch < this.fireTime / 3f)
            {
                bulletStopWatch += Time.fixedDeltaTime;
            }
            if (base.fixedAge >= this.fireTime && bulletStopWatch > this.fireTime / 3f && bulletCount > 0)
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