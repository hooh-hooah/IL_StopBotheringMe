using BepInEx;
using BepInEx.Harmony;
using StopBotheringMeComponents;

// Has dependency on moreaccessories by jaon6694
[BepInPlugin(GUID, "AI_StopBotheringMe", VERSION)]
[BepInProcess("StudioNEOV2")]
public class StopBotheringMePlugin : BaseUnityPlugin
{
    public const string GUID = "com.hooh.stopbotheringme";
    public const string VERSION = "1.0.0";

    private void Awake()
    {
        Hooks.Logger = Logger;
        HarmonyLib.Harmony.CreateAndPatchAll(typeof(Hooks));
    }
}