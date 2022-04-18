using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace JinxMod.SkillStates
{
    public class FishBones : BaseSkillState
    {
        public static float damageCoefficient = 8f;
        public static float procCoefficient = 1f;
        public static float baseDuration = 2.00f;
        public static float throwForce = 80f;

        private float duration;
        private float fireTime;
        private bool hasFired;
        private Animator animator;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = FishBones.baseDuration / this.attackSpeedStat;
            this.fireTime = 0.11f;
            base.characterBody.SetAimTimer(2f);
            this.animator = base.GetModelAnimator();

            if (this.animator.GetBool("isMoving") || (!(this.animator.GetBool("isGrounded"))))
            {
                base.PlayAnimation("Gesture, Override", "fishbonesattack");
            }
            else if ((!(this.animator.GetBool("isMoving"))) && this.animator.GetBool("isGrounded"))
            {
                base.PlayAnimation("FullBody, Override", "fishbonesattack");
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        private void Fire()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;
                Util.PlaySound("Play_JinxFishBonesShoot", base.gameObject);

                if (base.isAuthority)
                {
                    FireMissile();
                }
            }
        }

        private void FireMissile()
        {
            Ray aimRay = base.GetAimRay();
            base.AddRecoil(-1f * 6f, -2f * 6f, -0.5f * 6f, 0.5f * 6f);
            base.characterBody.AddSpreadBloom(1.5f);
            GameObject projectilePrefab = Modules.Projectiles.missilePrefab;
            bool isCrit = Util.CheckRoll(this.characterBody.crit, this.characterBody.master);
            FireProjectileInfo fireProjectileInfo = new FireProjectileInfo
            {
                projectilePrefab = projectilePrefab,
                position = aimRay.origin,
                rotation = Util.QuaternionSafeLookRotation(aimRay.direction),
                procChainMask = default(ProcChainMask),
                target = null,
                owner = this.characterBody.gameObject,
                damage = this.characterBody.damage * FishBones.damageCoefficient,
                crit = isCrit,
                force = 200f,
                damageColorIndex = DamageColorIndex.Default
            };
            ProjectileManager.instance.FireProjectile(fireProjectileInfo);
            //MissileUtils.FireMissile(aimRay.origin, this.characterBody, default(ProcChainMask), null, this.characterBody.damage * num, isCrit, projectilePrefab, DamageColorIndex.Default, aimRay.direction, 200f, true);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.fireTime)
            {
                this.Fire();
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