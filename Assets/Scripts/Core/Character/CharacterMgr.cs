using System.Collections.Generic;
using JsonData;

namespace Core.Character
{
    public class CharacterMgr
    {
        private static CharacterMgr inst;
        public static CharacterMgr Inst()
        {
            if (inst == null)
            {
                inst = new CharacterMgr();
            }
            return inst;
        }

        public PlayerInfo PlayerInfo
        {
            get;
            private set;
        }

        public CharacterMgr()
        {
            CreateTestPlayerInfo();
        }

        private void CreateTestPlayerInfo()
        {
            PlayerInfo = new PlayerInfo();
            PlayerInfo.playerId = 999999999999;
            PlayerInfo.playerName = "猫猫";
            PlayerInfo.job = 2;
            PlayerInfo.sex = 1;
            PlayerInfo.level = 499;
            PlayerInfo.attrs = new Dictionary<int, long>();
            PlayerInfo.attrs.Add(1001, 100);
            PlayerInfo.attrs.Add(1002, 100);
            PlayerInfo.attrs.Add(1003, 100);
            PlayerInfo.attrs.Add(2001, 999);
            PlayerInfo.attrs.Add(2002, 499); 
            PlayerInfo.attrs.Add(2003, 9999);
            PlayerInfo.attrs.Add(2004, 999);
            PlayerInfo.attrs.Add(2005, 499);
            PlayerInfo.attrs.Add(2006, 999);
            PlayerInfo.attrs.Add(3001, 999);
            PlayerInfo.attrs.Add(3002, 4999);
            PlayerInfo.attrs.Add(4001, 0);
            PlayerInfo.attrs.Add(4002, 7999);
            PlayerInfo.attrs.Add(4003, 3999);
        }


    }
}
