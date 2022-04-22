using RoR2;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace JinxMod.Controllers
{
    [RequireComponent(typeof(ProjectileController))]
    public class JinxMegaImpact : ProjectileExplosion, IProjectileImpactBehavior
    {
        private float age;
        public override void Awake()
        {
            base.Awake();
        }

        public void FixedUpdate()
        {
            age += Time.fixedDeltaTime;
        }
        public void OnProjectileImpact(ProjectileImpactInfo impactInfo)
        {
            List<HurtBox> HurtBoxes = new List<HurtBox>();
            HurtBoxes = new SphereSearch
            {
                radius = 16f,
                mask = LayerIndex.entityPrecise.mask,
                origin = impactInfo.estimatedPointOfImpact
            }.RefreshCandidates().FilterCandidatesByHurtBoxTeam(TeamMask.GetEnemyTeams(this.projectileController.teamFilter.teamIndex)).FilterCandidatesByDistinctHurtBoxEntities().GetHurtBoxes().ToList();


            foreach (HurtBox hurtbox in HurtBoxes)
            {
                float damageDistance = this.projectileDamage.damage * (Mathf.Min(1, age * 0.20f));
                DamageInfo damageInfo = new DamageInfo();
                damageInfo.damage = damageDistance + (hurtbox.healthComponent.missingCombinedHealth * 0.35f);
                damageInfo.crit = false;
                damageInfo.damageColorIndex = DamageColorIndex.Item;
                damageInfo.attacker = (this.projectileController.owner ? this.projectileController.owner.gameObject : null);
                damageInfo.inflictor = base.gameObject;
                damageInfo.position = impactInfo.estimatedPointOfImpact;
                damageInfo.force = this.projectileDamage.force * base.transform.forward;
                damageInfo.procChainMask = this.projectileController.procChainMask;
                damageInfo.procCoefficient = this.projectileController.procCoefficient;
                hurtbox.healthComponent.TakeDamage(damageInfo);
            }
        }
    }
}
