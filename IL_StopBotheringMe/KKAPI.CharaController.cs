using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AIChara;
using KKAPI;
using KKAPI.Chara;
using UnityEngine;

namespace StopBotheringMeComponents
{
    public class CharaController : CharaCustomFunctionController
    {
        public static readonly Dictionary<ChaControl, LightProbeProxyVolume> ProxyVolumes = new Dictionary<ChaControl, LightProbeProxyVolume>();

        protected override void OnDestroy()
        {
            if (ProxyVolumes.ContainsKey(ChaControl)) ProxyVolumes.Remove(ChaControl);
            base.OnDestroy();
        }

        [SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeInvocation")]
        private static IEnumerator InitializeLate(ChaControl control)
        {
            yield return new WaitForFixedUpdate();
            var gameObject = control.gameObject;
            var probeProxyVolume = gameObject.gameObject.GetOrAddComponent<LightProbeProxyVolume>();
            if (!ProxyVolumes.ContainsKey(control)) ProxyVolumes.Add(control, probeProxyVolume);
            probeProxyVolume.refreshMode = LightProbeProxyVolume.RefreshMode.Automatic;
            probeProxyVolume.boundingBoxMode = LightProbeProxyVolume.BoundingBoxMode.AutomaticLocal;
            probeProxyVolume.probePositionMode = LightProbeProxyVolume.ProbePositionMode.CellCenter;
            Configs.FollowConfigDensity(probeProxyVolume);
            Configs.FollowConfigQuality(probeProxyVolume);
            Configs.FollowConfigEnable(probeProxyVolume);
        }


        private static IEnumerator WaitUntilOtherProcess(LightProbeProxyVolume volume)
        {
            yield return new WaitForSecondsRealtime(.5f);
            Configs.UpdateRenderers(volume);
        }
        
        protected override void OnCoordinateBeingLoaded(ChaFileCoordinate coordinate, bool maintainState)
        {
            if (ProxyVolumes.TryGetValue(ChaControl, out var proxyVolume))
                StartCoroutine(WaitUntilOtherProcess(proxyVolume));
            base.OnCoordinateBeingLoaded(coordinate, maintainState);
        }


        protected override void OnCardBeingSaved(GameMode currentGameMode)
        {
        }

        protected override void OnReload(GameMode currentGameMode)
        {
            StartCoroutine(InitializeLate(ChaControl));
            base.OnReload(currentGameMode);
        }

        protected override void OnCoordinateBeingLoaded(ChaFileCoordinate coordinate)
        {
            base.OnCoordinateBeingLoaded(coordinate);
        }
    }
}