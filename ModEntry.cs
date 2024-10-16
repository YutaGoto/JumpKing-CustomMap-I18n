﻿using HarmonyLib;
using JumpKing.MiscSystems.LocationText;
using JumpKing;
using JumpKing.Mods;
using System;
using Steamworks;

namespace JumpKing_CustomMap_i18n
{
    [JumpKingMod("YutaGoto.JumpKing_CustomMap_i18n")]
    public static class ModEntry
    {
        private static readonly string harmonyId = "YutaGoto.JumpKing_CustomMap_i18n";
        public static readonly Harmony harmony = new Harmony(harmonyId);

        /// <summary>
        /// Called by Jump King before the level loads
        /// </summary>
        [BeforeLevelLoad]
        public static void BeforeLevelLoad()
        {
#if DEBUG
            Debugger.Launch();
#endif
        }

        /// <summary>
        /// Called by Jump King when the level unloads
        /// </summary>
        [OnLevelUnload]
        public static void OnLevelUnload()
        {
            // Your code here
        }

        /// <summary>
        /// Called by Jump King when the Level Starts
        /// </summary>
        [OnLevelStart]
        public static void OnLevelStart()
        {
            NewSetSettingsData();
        }

        /// <summary>
        /// Called by Jump King when the Level Ends
        /// </summary>
        [OnLevelEnd]
        public static void OnLevelEnd()
        {
            // Your code here
        }

        public static bool NewSetSettingsData()
        {
            // see: https://partner.steamgames.com/doc/store/localization/languages

            string text = "gui\\location_settings";
            string langString = SteamApps.GetCurrentGameLanguage();

            try
            {
                string fileString = Game1.instance.contentManager.root + text + "." + langString + ".xml";
                AccessTools.StaticFieldRefAccess<LocationSettings>("JumpKing.MiscSystems.LocationText.LocationTextManager:_settings") = XmlSerializerHelper.Deserialize<LocationSettings>(fileString);

                return false;
            }
            catch (Exception)
            {
                return true;
            }
        }
    }
}
