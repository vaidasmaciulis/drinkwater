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
	public class Session : MySessionComponentBase
	{
		private static int skippedTicks = 0;
		private IMyPlayer player;
		private MyEntityStatComponent statComp;
		private MyEntityStat water;
		private MyEntityStat food;
		private MyEntityStat sleep;
		private bool isDedicated;
		private const float WATER_USAGE = 0.03f;
		private const float WATER_DAMAGE = 0.015f;
		private const float FOOD_USAGE = 0.015f;
		private const float FOOD_DAMAGE = 1f;
		private const float SLEEP_USAGE = 0.01f;
		private const float SLEEP_DAMAGE = 0.75f;
		private const float SLEEP_GAIN_SITTING = 1f;
		private const float SLEEP_GAIN_SLEEPING = 100f;

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			isDedicated = MyAPIGateway.Utilities.IsDedicated;
			if (isDedicated)
			{
				return;
			}
			UpdateAfterSimulation100();
		}

		public override void UpdateAfterSimulation()
		{
			if (isDedicated)
			{
				return;
			}
			if (skippedTicks++ <= 100)
			{
				if (skippedTicks % 10 == 0)
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
			if (player?.Character == null || statComp == null || water == null || food == null || sleep == null)
			{
				return;
			}
			if (player.Character.EnabledHelmet)
			{
				if (water.StatRegenLeft > 0)
				{
					player.Character.SwitchHelmet();
					MyAPIGateway.Utilities.ShowNotification("Helmet opened to drink!", 3000);
				}
				else if (food.StatRegenLeft > 0)
				{
					player.Character.SwitchHelmet();
					MyAPIGateway.Utilities.ShowNotification("Helmet opened to eat!", 3000);
				}
			}
		}

		public void UpdateAfterSimulation100()
		{
			player = MyAPIGateway.Session?.Player;
			statComp = player?.Character?.Components?.Get<MyEntityStatComponent>();
			if (statComp == null)
			{
				return;
			}
			statComp.TryGetStat(MyStringHash.GetOrCompute("Water"), out water);
			statComp.TryGetStat(MyStringHash.GetOrCompute("Food"), out food);
			statComp.TryGetStat(MyStringHash.GetOrCompute("Sleep"), out sleep);
			if (water == null || food == null || sleep == null)
			{
				return;
			}

			if (water.Value <= 0)
			{
				player.Character.DoDamage(WATER_DAMAGE, MyDamageType.Unknown, true);
			}

			if (food.Value <= 0)
			{
				player.Character.DoDamage(FOOD_DAMAGE, MyDamageType.Unknown, true);
			}

			if (sleep.Value <= 0)
			{
				player.Character.DoDamage(SLEEP_DAMAGE, MyDamageType.Unknown, true);
			}

			bool inCryoOrBed = false;

			if (player.Character.CurrentMovementState == MyCharacterMovementEnum.Sitting &&
				(player.Controller.ControlledEntity as IMyShipController) != null &&
				!(player.Controller.ControlledEntity as IMyShipController).CanControlShip)
			{
				//Sitting, but not working
				inCryoOrBed = player.Controller.ControlledEntity.ToString().StartsWith("MyCryoChamber");
				float sleepGain = inCryoOrBed ? SLEEP_GAIN_SLEEPING : SLEEP_GAIN_SITTING;
				sleep.Increase(sleepGain, null);
			}
			else
			{
				sleep.Decrease(SLEEP_USAGE, null);
			}

			if (!inCryoOrBed)
			{
				food.Decrease(FOOD_USAGE, null);
				water.Decrease(WATER_USAGE, null);
			}
		}
	}
}
