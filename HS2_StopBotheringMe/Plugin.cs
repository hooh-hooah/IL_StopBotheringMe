using BepInEx;
using HarmonyLib;
using StopBotheringMeComponents;

// Has dependency on moreaccessories by jaon6694
[BepInPlugin(Constant.GUID, "HS2_StopBotheringMe", Constant.VERSION)]
[BepInProcess("StudioNEOV2")]
public class StopBotheringMePlugin : BaseUnityPlugin
{
    private void Awake()
    {
        Hooks.Logger = Logger;
        Configs.InitializeConfigs(Config);
        Harmony.CreateAndPatchAll(typeof(Hooks));
    }
}