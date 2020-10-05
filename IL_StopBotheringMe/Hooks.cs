using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using StudioMap = Studio.Map;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

namespace StopBotheringMeComponents
{
    public static class Hooks
    {
        public static ManualLogSource Logger { get; set; }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(StudioMap), nameof(StudioMap.LoadMap), typeof(int))]
        public static void LoadMap(StudioMap __instance, int _no)
        {
            // Clear existing shits 
            LightmapSettings.lightProbes = null;
            LightmapSettings.lightmaps = null;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(StudioMap), nameof(StudioMap.LoadMapCoroutine), typeof(int), typeof(bool))]
        public static void LoadMap(StudioMap __instance, int _no, bool _wait)
        {
            LoadMap(__instance, _no);
        }
    }
}