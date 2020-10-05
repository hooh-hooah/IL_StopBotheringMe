using System;
using BepInEx.Configuration;
using KKAPI.Chara;
using UnityEngine;
using UnityEngine.Rendering;

namespace StopBotheringMeComponents
{
    public static class Configs
    {
        public static ConfigEntry<bool> EnableProbeVolume { get; private set; }
        public static ConfigEntry<bool> HighQualityVolume { get; private set; }
        public static ConfigEntry<bool> UseHighQualityResolution { get; private set; }
        public static ConfigEntry<float> ProxyProbeDensity { get; private set; }
        public static ConfigEntry<int> ProxyProbeResolution { get; private set; }

        public static void FollowConfigDensity(LightProbeProxyVolume volume)
        {
            if (ReferenceEquals(null, volume)) return;
            volume.resolutionMode = UseHighQualityResolution.Value ? LightProbeProxyVolume.ResolutionMode.Automatic : LightProbeProxyVolume.ResolutionMode.Custom;
            if (UseHighQualityResolution.Value)
            {
                volume.probeDensity = Mathf.Clamp(ProxyProbeDensity.Value, 0.01f, 1f);
            }
            else
            {
                volume.gridResolutionX = ProxyProbeResolution.Value;
                volume.gridResolutionY = ProxyProbeResolution.Value;
                volume.gridResolutionZ = ProxyProbeResolution.Value;
            }
        }

        public static void FollowConfigQuality(LightProbeProxyVolume volume)
        {
            if (ReferenceEquals(null, volume)) return;
            volume.qualityMode = HighQualityVolume.Value ? LightProbeProxyVolume.QualityMode.Normal : LightProbeProxyVolume.QualityMode.Low;
        }

        public static void FollowConfigEnable(LightProbeProxyVolume volume)
        {
            if (ReferenceEquals(null, volume) || volume == null) return;
            volume.enabled = EnableProbeVolume.Value;

            UpdateRenderers(volume);
        }

        public static void UpdateRenderers(LightProbeProxyVolume volume)
        {
            foreach (var renderer in volume.transform.GetComponentsInChildren<Renderer>())
                if (EnableProbeVolume.Value)
                {
                    renderer.lightProbeProxyVolumeOverride = volume.gameObject;
                    renderer.lightProbeUsage = LightProbeUsage.UseProxyVolume;
                }
                else
                {
                    renderer.lightProbeProxyVolumeOverride = null;
                    renderer.lightProbeUsage = LightProbeUsage.BlendProbes;
                }
        }

        public static void InitializeConfigs(ConfigFile pluginConfig)
        {
            CharacterApi.RegisterExtraBehaviour<CharaController>(Constant.GUID);
            
            EnableProbeVolume = pluginConfig.Bind("StopBotheringMe", "Enable Probe Volume", true,
                new ConfigDescription("It will interpolate light probes and cast calculated lighting to character."));

            EnableProbeVolume.SettingChanged += delegate
            {
                foreach (var lightProbeProxyVolume in CharaController.ProxyVolumes.Values) FollowConfigEnable(lightProbeProxyVolume);
            };

            HighQualityVolume = pluginConfig.Bind("StopBotheringMe", "High Quality Proxy Volume", true,
                new ConfigDescription("The probe proxy will try to utilize high quality mode blending."));

            HighQualityVolume.SettingChanged += delegate
            {
                foreach (var lightProbeProxyVolume in CharaController.ProxyVolumes.Values) FollowConfigQuality(lightProbeProxyVolume);
            };

            UseHighQualityResolution = pluginConfig.Bind("StopBotheringMe", "Use Hiqh Quality Resolution", false,
                new ConfigDescription("Make light probe proxy more denser. It's REALLY expensive."));
            ProxyProbeDensity = pluginConfig.Bind("StopBotheringMe", "Proxy Probe Density", 0.3f
                , new ConfigDescription("Determines how dense light probe proxy is (Requires HQ Resolution, VERY EXPENSIVE)."));
            ProxyProbeResolution = pluginConfig.Bind("StopBotheringMe", "Proxy Probe Resolution", 4,
                new ConfigDescription("Determines how many light probe will be in the proxy  (X*Y*Z)"));

            void OnQualityChanged(object sender, EventArgs e)
            {
                foreach (var lightProbeProxyVolume in CharaController.ProxyVolumes.Values) FollowConfigDensity(lightProbeProxyVolume);
            }

            ProxyProbeDensity.SettingChanged += OnQualityChanged;
            ProxyProbeResolution.SettingChanged += OnQualityChanged;
            UseHighQualityResolution.SettingChanged += OnQualityChanged;
        }
    }
}