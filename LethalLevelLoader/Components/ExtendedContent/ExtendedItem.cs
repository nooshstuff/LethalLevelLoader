﻿using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LethalLevelLoader
{
    [CreateAssetMenu(menuName = "LethalLevelLoader/ExtendedItem")]
    public class ExtendedItem : ExtendedContent
    {
        [field: SerializeField] public Item Item { get; set; }
        [field: SerializeField] public string PluralisedItemName { get; set; } = string.Empty;
        [field: SerializeField] public bool IsBuyableItem { get; set; }

        [field: SerializeField] public LevelMatchingProperties LevelMatchingProperties { get; set; }
        [field: SerializeField] public DungeonMatchingProperties DungeonMatchingProperties { get; set; }

        [field: SerializeField] public string OverrideInfoNodeDescription { get; set; } = string.Empty;
        [field: SerializeField] public string OverrideBuyNodeDescription { get; set; } = string.Empty;
        [field: SerializeField] public string OverrideBuyConfirmNodeDescription { get; set; } = string.Empty;

        public TerminalNode BuyNode { get; internal set; }
        public TerminalNode BuyConfirmNode { get; internal set; }
        public TerminalNode BuyInfoNode { get; internal set; }

        public int CreditsWorth
        {
            get
            {
                if (BuyNode != null && BuyConfirmNode != null)
                {
                    BuyNode.itemCost = Item.creditsWorth;
                    BuyConfirmNode.itemCost = Item.creditsWorth;
                }
                else
                    Debug.LogWarning("BuyNode And/Or BuyConfirm Node Missing!");
                return (Item.creditsWorth);
            }
            set
            {
                if (value >= 0)
                {
                    if (BuyNode != null && BuyConfirmNode != null)
                    {
                        BuyNode.itemCost = value;
                        BuyConfirmNode.itemCost = value;
                    }
                    else
                        Debug.LogWarning("BuyNode And/Or BuyConfirm Node Missing!");
                    Item.creditsWorth = value;
                }
            }
        }

        public static ExtendedItem Create(Item newItem, ExtendedMod extendedMod, ContentType contentType)
        {
            ExtendedItem extendedItem = ScriptableObject.CreateInstance<ExtendedItem>();
            extendedItem.Item = newItem;
            extendedItem.name = newItem.itemName.SkipToLetters().RemoveWhitespace() + "ExtendedItem";
            extendedItem.ContentType = contentType;
            extendedMod.RegisterExtendedContent(extendedItem);

            extendedItem.TryCreateMatchingProperties();

            return (extendedItem);
        }

        public void Initialize()
        {
            DebugHelper.Log("Initializing Custom Item: " + Item.itemName + ". Is Buyable: " + IsBuyableItem + ". Is Scrap: " + Item.isScrap);

            TryCreateMatchingProperties();

            Patches.StartOfRound.allItemsList.itemsList.Add(Item);
            if (IsBuyableItem)
                TerminalManager.CreateItemTerminalData(this);
        }

        internal override void TryCreateMatchingProperties()
        {
            if (LevelMatchingProperties == null)
                LevelMatchingProperties = ScriptableObject.CreateInstance<LevelMatchingProperties>();
            if (DungeonMatchingProperties == null)
                DungeonMatchingProperties = ScriptableObject.CreateInstance<DungeonMatchingProperties>();
        }

        public void SetLevelMatchingProperties(LevelMatchingProperties newLevelMatchingProperties)
        {
            if (Plugin.Instance != null)
                Debug.LogError("SetLevelMatchingProperties() Should Only Be Used In Editor!");
            LevelMatchingProperties = newLevelMatchingProperties;
        }
    }
}
