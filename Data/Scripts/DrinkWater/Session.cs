using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Utils;

namespace DrinkWater
{
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
	public class Session : MySessionComponentBase
	{
		private const int TICKS_TO_SKIP = 300;
		private const float WATER_USAGE = 0.1f;
		private const float WATER_DAMAGE = 5f;
		private const float FOOD_USAGE = 0.03f;
		private const float FOOD_DAMAGE = 3f;
		private const float SLEEP_USAGE = 0.02f;
		private const float SLEEP_DAMAGE = 2f;
		private const float SLEEP_GAIN_SITTING = 3f;
		private const float SLEEP_GAIN_SLEEPING = 100f;

		private static List<IMyPlayer> players = new List<IMyPlayer>();
		private static int skippedTicks = 0;

		public override void UpdateAfterSimulation()
		{
			if (skippedTicks++ < TICKS_TO_SKIP)
			{
				return;
			}
			skippedTicks = 0;

			players.Clear();
			MyAPIGateway.Players.GetPlayers(players);

			foreach (IMyPlayer player in players)
			{
				if (player.IsBot == true)
				{
					continue;
				}
				MyEntityStatComponent statComp = player.Character?.Components?.Get<MyEntityStatComponent>();
				MyInventoryBase inventory = (MyInventoryBase)player.Character?.GetInventory();
				if (statComp == null || inventory == null)
				{
					continue;
				}

				MyEntityStat water = GetPlayerStat(statComp, "Water");
				MyEntityStat food = GetPlayerStat(statComp, "Food");
				MyEntityStat sleep = GetPlayerStat(statComp, "Sleep");

				inventory.ContentsRemoved += (item, point) =>
				{
					string objectIdString = item.Content.GetObjectId().ToString();

					if (objectIdString.Contains("ConsumableItem"))
					{
						//Make sure it was not just any consumable removed from inventory
						if (water.HasAnyEffect())
						{
							if (player.Character.EnabledHelmet)
							{
								player.Character.SwitchHelmet();
								MyAPIGateway.Utilities.ShowNotification("Helmet opened to drink!", 3000);
							}
						}
						if (food.HasAnyEffect())
						{
							if (player.Character.EnabledHelmet)
							{
								player.Character.SwitchHelmet();
								MyAPIGateway.Utilities.ShowNotification("Helmet opened to eat!", 3000);
							}
						}
					}
				};

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

		private MyEntityStat GetPlayerStat(MyEntityStatComponent statComp, string statName)
		{
			MyEntityStat stat;
			statComp.TryGetStat(MyStringHash.GetOrCompute(statName), out stat);
			return stat;
		}
	}
}
