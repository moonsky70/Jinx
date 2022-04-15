using JinxMod.Controllers;
using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace JinxMod.Modules
{
    internal static class Projectiles
    {
        internal static GameObject bombPrefab;
        internal static GameObject missilePrefab;

        internal static void RegisterProjectiles()
        {
            CreateBomb();
            CreateMissile();
            AddProjectile(bombPrefab);
            AddProjectile(missilePrefab);
        }
        private static void CreateMissile()
        {
            missilePrefab = CloneProjectilePrefab("MissileProjectile", "JinxMissileProjectile");
            GameObject.Destroy(missilePrefab.GetComponent<MissileController>());
            GameObject.Destroy(missilePrefab.GetComponent<ProjectileSingleTargetImpact>());

            ProjectileSteerTowardTarget projectileSteerTowardTarget = missilePrefab.AddComponent<ProjectileSteerTowardTarget>();
            projectileSteerTowardTarget.rotationSpeed = 360f;
            projectileSteerTowardTarget.yAxisOnly = false;

            ProjectileSphereTargetFinder projectileSphereTargetFinder = missilePrefab.AddComponent<ProjectileSphereTargetFinder>();
            projectileSphereTargetFinder.lookRange = 5f;
            projectileSphereTargetFinder.onlySearchIfNoTarget = false;
            projectileSphereTargetFinder.allowTargetLoss = true;
            projectileSphereTargetFinder.testLoS = true;
            projectileSphereTargetFinder.targetSearchInterval = 0.1f;


            ProjectileImpactExplosion ImpactExplosion = missilePrefab.AddComponent<ProjectileImpactExplosion>();
            InitializeImpactExplosion(ImpactExplosion);

            ImpactExplosion.blastRadius = 16f;
            ImpactExplosion.destroyOnEnemy = true;
            ImpactExplosion.destroyOnWorld = true;
            ImpactExplosion.lifetime = 12f;
            ImpactExplosion.impactEffect = Modules.Assets.bombExplosionEffect;
            //bombImpactExplosion.lifetimeExpiredSound = Modules.Assets.CreateNetworkSoundEventDef("HenryBombExplosion");
            ImpactExplosion.timerAfterImpact = true;
            ImpactExplosion.lifetimeAfterImpact = 0.1f;

            ProjectileSimple projectileSimple = missilePrefab.AddComponent<ProjectileSimple>();
            projectileSimple.desiredForwardSpeed = 60f;
            projectileSimple.oscillate = false;
            projectileSimple.updateAfterFiring = true;
            projectileSimple.enableVelocityOverLifetime = false;

            BoxCollider boxCollider = missilePrefab.GetComponent<BoxCollider>();
            boxCollider.size = new Vector3(0.075f, 0.075f, 0.075f);

            Rigidbody rigidBody = missilePrefab.GetComponent<Rigidbody>();
            rigidBody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

            ProjectileController projectileController = missilePrefab.GetComponent<ProjectileController>();
            var ghostPrefab = PrefabAPI.InstantiateClone(projectileController.ghostPrefab, "MissileGhost", false);
            missilePrefab.transform.localScale *= 5;
            ghostPrefab.transform.localScale *= 5;
            projectileController.ghostPrefab = ghostPrefab;

            missilePrefab.AddComponent<ProjectileImpactEventCaller>();
            missilePrefab.AddComponent<RocketJumpController>();
        }

        private static void CreateBomb()
        {
            bombPrefab = CloneProjectilePrefab("CommandoGrenadeProjectile", "HenryBombProjectile");

            ProjectileImpactExplosion bombImpactExplosion = bombPrefab.GetComponent<ProjectileImpactExplosion>();
            InitializeImpactExplosion(bombImpactExplosion);

            bombImpactExplosion.blastRadius = 16f;
            bombImpactExplosion.destroyOnEnemy = true;
            bombImpactExplosion.lifetime = 12f;
            bombImpactExplosion.impactEffect = Modules.Assets.bombExplosionEffect;
            //bombImpactExplosion.lifetimeExpiredSound = Modules.Assets.CreateNetworkSoundEventDef("HenryBombExplosion");
            bombImpactExplosion.timerAfterImpact = true;
            bombImpactExplosion.lifetimeAfterImpact = 0.1f;

            ProjectileController bombController = bombPrefab.GetComponent<ProjectileController>();
            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("HenryBombGhost") != null) bombController.ghostPrefab = CreateGhostPrefab("HenryBombGhost");
            bombController.startSound = "";
        }

        internal static void AddProjectile(GameObject projectileToAdd)
        {
            Modules.Content.AddProjectilePrefab(projectileToAdd);
        }

        private static void InitializeImpactExplosion(ProjectileImpactExplosion projectileImpactExplosion)
        {
            projectileImpactExplosion.blastDamageCoefficient = 1f;
            projectileImpactExplosion.blastProcCoefficient = 1f;
            projectileImpactExplosion.blastRadius = 1f;
            projectileImpactExplosion.bonusBlastForce = Vector3.zero;
            projectileImpactExplosion.childrenCount = 0;
            projectileImpactExplosion.childrenDamageCoefficient = 0f;
            projectileImpactExplosion.childrenProjectilePrefab = null;
            projectileImpactExplosion.destroyOnEnemy = false;
            projectileImpactExplosion.destroyOnWorld = false;
            projectileImpactExplosion.falloffModel = RoR2.BlastAttack.FalloffModel.None;
            projectileImpactExplosion.fireChildren = false;
            projectileImpactExplosion.impactEffect = null;
            projectileImpactExplosion.lifetime = 0f;
            projectileImpactExplosion.lifetimeAfterImpact = 0f;
            projectileImpactExplosion.lifetimeRandomOffset = 0f;
            projectileImpactExplosion.offsetForLifetimeExpiredSound = 0f;
            projectileImpactExplosion.timerAfterImpact = false;

            projectileImpactExplosion.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;
        }

        private static GameObject CreateGhostPrefab(string ghostName)
        {
            GameObject ghostPrefab = Modules.Assets.mainAssetBundle.LoadAsset<GameObject>(ghostName);
            if (!ghostPrefab.GetComponent<NetworkIdentity>()) ghostPrefab.AddComponent<NetworkIdentity>();
            if (!ghostPrefab.GetComponent<ProjectileGhostController>()) ghostPrefab.AddComponent<ProjectileGhostController>();

            Modules.Assets.ConvertAllRenderersToHopooShader(ghostPrefab);

            return ghostPrefab;
        }

        private static GameObject CloneProjectilePrefab(string prefabName, string newPrefabName)
        {
            GameObject newPrefab = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/" + prefabName), newPrefabName);
            return newPrefab;
        }
    }
}