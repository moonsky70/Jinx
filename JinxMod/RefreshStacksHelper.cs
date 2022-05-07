using BepInEx.Bootstrap;
using NetworkedTimedBuffs;
using R2API.Networking;
using R2API.Networking.Interfaces;
using RoR2;
using UnityEngine.Networking;

namespace JinxMod
{
	public static class RefreshStacksHelper
	{
		public static void RefreshStacks(this CharacterBody body, BuffIndex index, float duration, int maxStacks)
		{
			var amount = 0;
			for (var i = 0; i < body.timedBuffs.Count; i++)
			{
				var timedBuff = body.timedBuffs[i];
				if (timedBuff.buffIndex != index) continue;
				amount++;
				if (timedBuff.timer < duration)
				{
					timedBuff.timer = duration;
					if (Chainloader.PluginInfos.ContainsKey("bubbet.networkedtimedbuffs"))
						NetworkBuffTimer(body, i, duration);
				}
			}

			if (amount < maxStacks)
			{
				body.AddTimedBuff(BuffCatalog.GetBuffDef(index), duration, maxStacks);
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