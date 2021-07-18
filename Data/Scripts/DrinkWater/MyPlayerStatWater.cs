using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace DrinkWater
{
    public class MyStatPlayerWater : IMyHudStat
    {
        public MyStatPlayerWater()
        {
            Id = MyStringHash.GetOrCompute("player_water");
        }

        private float m_currentValue;
        private string m_valueStringCache;

        public MyStringHash Id { get; protected set; }

        public float CurrentValue
        {
            get { return m_currentValue; }
            protected set
            {
                if (m_currentValue == value)
                {
                    return;
                }
                m_currentValue = value;
                m_valueStringCache = null;
            }
        }

        public virtual float MaxValue => 1f;
        public virtual float MinValue => 0.0f;

        public string GetValueString()
        {
            if (m_valueStringCache == null)
            {
                m_valueStringCache = ToString();
            }
            return m_valueStringCache;
        }

        public void Update()
        {
            List<IMyPlayer> players = new List<IMyPlayer>();
            MyAPIGateway.Players.GetPlayers(players);
            IMyPlayer player = players[0];
            this.CurrentValue = player.Character.GetSuitGasFillLevel(new MyDefinitionId(typeof(MyObjectBuilder_GasProperties), "Water"));

        }

        public override string ToString() => string.Format("{0:0}", (float)(CurrentValue * 100.0));
    }
}