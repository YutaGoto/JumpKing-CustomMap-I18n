using HarmonyLib;
using JumpKing.MiscSystems.LocationText;
using JumpKing;
using JumpKing.Mods;
using System;
using Steamworks;
using System.Diagnostics;
using System.Reflection;
using JumpKing.Workshop;
using System.Linq;

namespace JumpKing_CustomMap_i18n
{
    [JumpKingMod("YutaGoto.JumpKing_CustomMap_i18n")]
    public static class ModEntry
    {
        private static readonly string harmonyId = "YutaGoto.JumpKing_CustomMap_i18n";
        internal static readonly Harmony harmony = new Harmony(harmonyId);
        internal static string[] Tags = new string[0];
        internal static string currentLanguage = "english";

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
            // see: https://partner.steamgames.com/doc/store/localization/languages
            currentLanguage = SteamApps.GetCurrentGameLanguage();

            NewMenuFactory();
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
            string text = "gui\\location_settings";

            try
            {
                string fileString = Game1.instance.contentManager.root + text + "." + currentLanguage + ".xml";
                AccessTools.StaticFieldRefAccess<LocationSettings>("JumpKing.MiscSystems.LocationText.LocationTextManager:_settings") = XmlSerializerHelper.Deserialize<LocationSettings>(fileString);

                return false;
            }
            catch (Exception)
            {
                return true;
            }
        }

        private static void NewMenuFactory()
        {
            MethodInfo getLevelTitle = AccessTools.TypeByName("MenuFactory").Method("GetLevelTitle");
            MethodInfo newGetLevelTitle = AccessTools.Method(typeof(ModEntry), "NewGetLevelTitle");
            harmony.Patch(getLevelTitle, postfix: new HarmonyMethod(newGetLevelTitle));
        }

        private static void NewGetLevelTitle(ref string __result)
        {
            if (Game1.instance.contentManager.level != null)
            {
                Tags = XmlSerializerHelper.Deserialize<Level.LevelSettings>(Game1.instance.contentManager.root + "\\" + Level.FileName).Tags;
                if (Tags == null) return;

                foreach (string tag in Tags)
                {
                    if (tag.Contains($"{currentLanguage}Title"))
                    {
                        string[] strings = tag.Split('=');
                        __result = strings[1];
                    }
                }
            }
        }
    }
}
