using CsvManager;
using UnityEngine;

namespace Core
{
    public class GameStart : MonoBehaviour
    {
        public delegate AbstractCfgManager CfgInstHandler();

        public event CfgInstHandler CfgInstEvent;

        private void Start()
        {
            RegistConfigManager();
        }

        private void RegistConfigManager()
        {
            CfgInstEvent += ItemCfgManager.Inst;
            CfgInstEvent += AttributeCfgManager.Inst;
            CfgInstEvent += EquipCfgManager.Inst;
            CfgInstEvent += MountCfgManager.Inst;
            CfgInstEvent += SkillCfgManager.Inst;
            CfgInstEvent += TreasureBoxCfgManager.Inst;
            CfgInstEvent += CurrencyCfgManager.Inst;

            CfgInstEvent.Invoke();
        }
    }
}
