using System;
using System.Collections.Generic;
using System.Linq;
using AIChara;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace StopBotheringMeComponents
{
    public static class Hooks
    {
        public static ManualLogSource Logger { get; set; }
        
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Studio.Map), nameof(Studio.Map.LoadMap), typeof(int))]
        // ReSharper disable once UnusedMember.Global
        public static void LoadMap(Studio.Map __instance, int _no)
        {
            // Clear existing shits 
            LightmapSettings.lightProbes = null;
            LightmapSettings.lightmaps = null;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Studio.Map), nameof(Studio.Map.LoadMapCoroutine), typeof(int), typeof(bool))]
        // ReSharper disable once UnusedMember.Global
        public static void LoadMap(Studio.Map __instance, int _no, bool _wait)
        {
            LoadMap(__instance, _no);
        }
    }
}