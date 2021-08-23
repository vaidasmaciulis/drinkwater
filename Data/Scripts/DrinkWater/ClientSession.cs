using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.Utils;

namespace DrinkWater
{
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
	public class ClientSession : MySessionComponentBase
	{
		private static int skippedTicks = 0;
		private IMyCharacter character;
		private MyEntityStatComponent statComp;
		private MyEntityStat water;
		private MyEntityStat food;

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			UpdateAfterSimulation100();
		}

		public override void UpdateAfterSimulation()
		{
			if (skippedTicks++ <= 100)
			{
				if(skippedTicks % 10 == 0)
				{
					UpdateAfterSimulation10();
				}
			}
			else
			{
				skippedTicks = 0;
				UpdateAfterSimulation100();
			}
		}

		public void UpdateAfterSimulation10()
		{
			if (character == null || statComp == null || water == null || food == null)
			{
				return;
			}
			if (character.EnabledHelmet)
			{
				if (water.HasAnyEffect())
				{
					character.SwitchHelmet();
					MyAPIGateway.Utilities.ShowNotification("Helmet opened to drink!", 3000);
				}
				else if (food.HasAnyEffect())
				{
					character.SwitchHelmet();
					MyAPIGateway.Utilities.ShowNotification("Helmet opened to eat!", 3000);
				}
			}
		}

		public void UpdateAfterSimulation100()
		{
			character = MyAPIGateway.Session?.Player?.Character;
			statComp = character?.Components?.Get<MyEntityStatComponent>();
			if (statComp == null)
			{
				return;
			}
			statComp.TryGetStat(MyStringHash.GetOrCompute("Water"), out water);
			statComp.TryGetStat(MyStringHash.GetOrCompute("Food"), out food);
		}
	}
}
