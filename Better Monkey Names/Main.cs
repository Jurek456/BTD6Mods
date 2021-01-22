using Assets.Scripts.Models;
using Assets.Scripts.Unity.Bridge;
using Assets.Scripts.Unity.UI_New.InGame.TowerSelectionMenu.TowerSelectionMenuThemes;
using Harmony;
using Il2CppSystem.Collections.Generic;
using System.IO;
using Assets.Scripts.Unity;
using MelonLoader;
using NKHook6.Api.Extensions;
using System;

namespace Better_Monkey_Names

{
    public class Main : MelonMod
    {
        private static readonly string Dir = $"{Directory.GetCurrentDirectory()}\\Mods\\BetterMonkeyNames";
        private static readonly string Config = $"{Dir}\\additionalNames.txt";

        public static Dictionary<string, string> moreNames = new Dictionary<string, string>();


        public override void OnApplicationStart()
        {
            base.OnApplicationStart();
            moreNames.Add("CaveMonkey", "Cave Monkey");
            moreNames.Add("AdmiralBrickell", "Admiral Brickell");
            moreNames.Add("Adora", "Adora");
            moreNames.Add("BananaFarmer", "Banana Farmer");
            moreNames.Add("Benjamin", "Benjamin");
            moreNames.Add("CaptainChurchill", "Captain Churchill");
            moreNames.Add("EnergisingTotem", "Energising Totem");
            moreNames.Add("Etienne", "Etienne");
            moreNames.Add("Ezili", "Ezili");
            moreNames.Add("Gwendolin", "Gwendolin");
            moreNames.Add("Marine", "Marine");
            moreNames.Add("NaturesWardTotem", "Natures Ward Totem");
            moreNames.Add("ObynGreenfoot", "Obyn Greenfoot");
            moreNames.Add("PatFusty", "Pat Fusty");
            moreNames.Add("Pontoon", "Pontoon");
            moreNames.Add("PortableLake", "Portable Lake");
            moreNames.Add("Quincy", "Quincy");
            moreNames.Add("SacrificialTotem", "Sacrificial Totem");
            moreNames.Add("Sentry", "Sentry");
            moreNames.Add("SentryBoom", "Sentry Boom");
            moreNames.Add("SentryCold", "Sentry Cold");
            moreNames.Add("SentryCrushing", "Sentry Crushing");
            moreNames.Add("SentryEnergy", "Sentry Energy");
            moreNames.Add("SentryParagon", "Sentry Paragon");
            moreNames.Add("StrikerJones", "Striker Jones");
            moreNames.Add("SunAvatarMini", "Sun Avatar Mini");
            moreNames.Add("TechBot", "Tech Bot");
            moreNames.Add("TrueSunAvatarMini", "True Sun Avatar Mini");
            Directory.CreateDirectory($"{Dir}");
            if (File.Exists(Config))
            {
                MelonLogger.Log("Reading config file");
                using (StreamReader sr = File.OpenText(Config))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        try
                        {
                            var index = s.IndexOf('=');
                            var monkeyName = s.Substring(0, index);
                            var newMonkeyName = s.Substring(index + 1);
                            if (moreNames.ContainsKey(monkeyName))
                            {
                                moreNames[monkeyName] = newMonkeyName;
                            }
                        }
                        catch (Exception e)
                        {
                            MelonLogger.LogError("Malformed line " + s);
                            e.GetType();
                        }

                    }
                }
            }
            else
            {
                MelonLogger.Log("Creating config file");
                using (StreamWriter sw = File.CreateText(Config))
                {
                    foreach (var name in moreNames.Keys)
                    {
                        sw.WriteLine(name + "=" + moreNames[name]);
                    }
                }
                MelonLogger.Log("Done Creating");
            }
            MelonLogger.Log("Better Monkey Names mod loaded!");
        }


        [HarmonyPatch(typeof(TSMThemeAmbidextrousRangs), nameof(TSMThemeAmbidextrousRangs.TowerInfoChanged))]
        public class TSMTheme_Patch
        {

            [HarmonyPostfix]

            public static void Postfix(TSMThemeAmbidextrousRangs __instance, TowerToSimulation tower)
            {
                if (Game.instance.getProfileModel().HasPurchasedNamedMonkeys() == true)
                {
                    Model towerModel = tower.tower.model;
                    Dictionary<string, string> Names = Game.instance.getProfileModel().namedMonkeyNames;
                    foreach (string key in Names.Keys)
                    {
                        string monkeyName = key.Replace("1", "");
                        if (towerModel.name.Contains(monkeyName))
                        {
                            __instance.towerName.text = Names[key];
                        }
                        else
                        {
                            foreach (string name in moreNames.Keys)
                            {
                                if (towerModel.name.Contains(name))
                                {
                                    __instance.towerName.text = moreNames[name];
                                }
                            }
                        }
                    }
                }



            }
        }

    }
    

}


