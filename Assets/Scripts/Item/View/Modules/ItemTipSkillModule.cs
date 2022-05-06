using Core;
using CsvManager;
using Item.Enum;
using Item.Model;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace Item.View.Modules
{
    public class ItemTipSkillModule : ItemTipModule
    {
        public override int moduleType => ItemTipModuleType.Skill;

        [SerializeField]
        private Image skillIcon;
        [SerializeField]
        private TextMeshProUGUI skillName;
        [SerializeField]
        private TextMeshProUGUI skillLevel;
        [SerializeField]
        private TextMeshProUGUI skillType;
        [SerializeField]
        private TextMeshProUGUI skillDesc;

        public override void SetData(BaseItemData itemData)
        {
            base.SetData(itemData);

            ItemTipType tipType = EnumUtil.GetEnumByDescription<ItemTipType>(itemData.itemRes.type);
            if (tipType == ItemTipType.Skill)
            {
                var skillId = Convert.ToInt32(itemData.itemRes.effects.typeValues[0].value);
                var skillRes = SkillCfgManager.Inst().GetItemById(skillId);
                if (skillRes != null)
                {
                    skillName.text = skillRes.name;
                    skillLevel.text = "Lv." + skillRes.level;
                    skillType.text = GetSkillType(skillRes.type);
                    skillDesc.text = skillRes.desc;

                    var atlas = AssetLoader.LoadAsset<SpriteAtlas>("ui/atlas/atlas_icon.spriteatlasv2");
                    if (atlas != null)
                    {
                        skillIcon.overrideSprite = atlas.GetSprite(skillRes.icon);
                    }
                }
            }

            IsValid = tipType == ItemTipType.Skill;
            OnSetDataFinished();
        }

        private string GetSkillType(string type)
        {
            switch(type)
            {
                case "ACTIVE": return "主动技能";
                case "PASSIVE": return "被动技能";
                default: return "";
            }
        }
    }
}
