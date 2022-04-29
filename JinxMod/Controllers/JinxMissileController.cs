using RoR2;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace JinxMod.Controller
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(ProjectileTargetComponent))]
	public class JinxMissileController : MonoBehaviour
	{
		private void Awake()
		{
			if (!NetworkServer.active)
			{
				base.enabled = false;
				return;
			}
			this.transform = base.transform;
			this.rigidbody = base.GetComponent<Rigidbody>();
			this.torquePID = base.GetComponent<QuaternionPID>();
			this.targetComponent = base.GetComponent<ProjectileTargetComponent>();
		}
		private void FixedUpdate()
		{
			this.timer += Time.fixedDeltaTime;
			if (this.timer < this.giveupTimer)
			{
				this.rigidbody.velocity = this.transform.forward * this.maxVelocity;
				if (this.targetComponent.target && this.timer >= this.delayTimer)
				{
					this.rigidbody.velocity = this.transform.forward * (this.maxVelocity + this.timer * this.acceleration);
					Vector3 vector = this.targetComponent.target.position + UnityEngine.Random.insideUnitSphere * this.turbulence - this.transform.position;
					if (vector != Vector3.zero)
					{
						Quaternion rotation = this.transform.rotation;
						Quaternion targetQuat = Util.QuaternionSafeLookRotation(vector);
						this.torquePID.inputQuat = rotation;
						this.torquePID.targetQuat = targetQuat;
						this.rigidbody.angularVelocity = this.torquePID.UpdatePID();
					}
				}
			}
			if (this.timer > this.deathTimer)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		private new Transform transform;
		private Rigidbody rigidbody;
		private ProjectileTargetComponent targetComponent;
		public float maxVelocity = 25f;
		public float rollVelocity = 0f;
		public float acceleration = 3f;
		public float delayTimer = 1f;
		public float giveupTimer = 8f;
		public float deathTimer = 10f;
		private float timer;
		private QuaternionPID torquePID;
		public float turbulence = 2f;
	}
}
