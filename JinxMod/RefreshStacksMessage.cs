using R2API.Networking.Interfaces;
using RoR2;
using UnityEngine.Networking;

namespace JinxMod
{
	public class RefreshStacksMessage : INetMessage
	{
		private CharacterBody body;
		private BuffIndex index;
		private float duration;
		private int maxStacks;
		
		public RefreshStacksMessage(){}

		public RefreshStacksMessage(CharacterBody body, BuffIndex index, float duration, int maxStacks)
		{
			this.body = body;
			this.index = index;
			this.duration = duration;
			this.maxStacks = maxStacks;
		}

		public void Serialize(NetworkWriter writer)
		{
			writer.Write(body.networkIdentity.netId);
			writer.WritePackedUInt32((uint) index);
			writer.Write(duration);
			writer.Write(maxStacks);
		}

		public void Deserialize(NetworkReader reader)
		{
			var netObj = Util.FindNetworkObject(reader.ReadNetworkId());
			body = netObj.GetComponent<CharacterBody>();
			index = (BuffIndex) reader.ReadPackedUInt32();
			duration = reader.ReadSingle();
			maxStacks = reader.ReadInt32();
		}
		
		public void OnReceived()
		{
			body.RefreshStacks(index, duration, maxStacks);
		}
	}
}