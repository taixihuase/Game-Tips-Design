using JsonData;
using System.Collections.Generic;

namespace Core.Character
{
    public class PlayerInfo
    {
        public long playerId;
        public string playerName;
        public int sex;
        public int job;
        public int level;
        public Dictionary<int, long> attrs;
    }
}
