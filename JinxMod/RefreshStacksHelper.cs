using BepInEx.Bootstrap;
using NetworkedTimedBuffs;
using R2API.Networking;
using R2API.Networking.Interfaces;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace JinxMod
{
	public static class RefreshStacksHelper
	{
		public static void RefreshStacks(this CharacterBody body, BuffIndex index, float duration, int maxStacks, bool setToMax = false)
		{
			var amount = 0;
			for (var i = 0; i < body.timedBuffs.Count; i++)
			{

				var timedBuff = body.timedBuffs[i];
				if (timedBuff.buffIndex != index) continue;
				amount++;
				if (timedBuff.timer < duration * amount)
				{
					timedBuff.timer = duration * amount;
					if (Chainloader.PluginInfos.ContainsKey("bubbet.networkedtimedbuffs"))
						NetworkBuffTimer(body, i, timedBuff.timer);
				}
			}
			if (setToMax)
            {
				for (var i = amount; i < maxStacks; i++)
				{
					body.AddTimedBuff(BuffCatalog.GetBuffDef(index), duration * (i + 1), maxStacks);
				}

				return;
			}

			if (amount < maxStacks)
			{
				body.AddTimedBuff(BuffCatalog.GetBuffDef(index), duration * (amount + 1), maxStacks);
			}
		}

		public static void NetworkBuffTimer(CharacterBody body, int index, float duration)
		{
			// private static void UpdateTimer(CharacterBody body, int index, float duration) why the fuck did i make this method private; time to copy paste its body
			if (!NetworkServer.active) return;
			if (!body.isPlayerControlled && NetworkedTimedBuffsPlugin.onlySyncPlayers.Value) return;
			new SyncTimedBuffUpdate(body.networkIdentity.netId, index, duration).Send(NetworkDestination.Clients);
		}
	}
}