using RoR2;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace JinxMod.Controllers
{
    public class RocketJumpController : MonoBehaviour
    {
        public Rigidbody rigidbody;
        public ProjectileImpactEventCaller projectileImpactEventCaller;

        private void Awake()
        {
            this.rigidbody = base.GetComponent<Rigidbody>();
            if (NetworkServer.active)
            {
                this.projectileImpactEventCaller = GetComponent<ProjectileImpactEventCaller>();
                if (projectileImpactEventCaller)
                {
                    projectileImpactEventCaller.impactEvent.AddListener(new UnityAction<ProjectileImpactInfo>(this.OnImpact));
                }
            }
        }

        private void OnDestroy()
        {
            if (projectileImpactEventCaller)
            {
                projectileImpactEventCaller.impactEvent.RemoveListener(new UnityAction<ProjectileImpactInfo>(this.OnImpact));
            }
        }
        private void OnImpact(ProjectileImpactInfo projectileImpactInfo)
        {
            Collider[] objectsInRange = Physics.OverlapSphere(projectileImpactInfo.estimatedPointOfImpact, 25f);

            foreach (Collider collision in objectsInRange)
            {
                CharacterBody characterBody = collision.GetComponent<CharacterBody>();
                if (characterBody && characterBody?.bodyIndex == BodyCatalog.FindBodyIndex("JinxBody"))
                {
                    AddExplosionForce(characterBody, 5000f, projectileImpactInfo.estimatedPointOfImpact, 25f, 0f);
                }
            }
        }
        private void AddExplosionForce(CharacterBody characterBody, float explosionForce, Vector3 explosionPosition, float explosionRadius, float upliftModifier = 0)
        {
            var dir = (characterBody.corePosition - explosionPosition);
            float wearoff = 1 - (dir.magnitude / explosionRadius);
            Vector3 baseForce = dir.normalized * explosionForce * wearoff;
            characterBody.characterMotor.ApplyForce(baseForce);

            if (upliftModifier != 0)
            {
                float upliftWearoff = 1 - upliftModifier / explosionRadius;
                Vector3 upliftForce = Vector3.up * explosionForce * upliftWearoff;
                characterBody.characterMotor.ApplyForce(upliftForce);
            }
        }
    }
}
