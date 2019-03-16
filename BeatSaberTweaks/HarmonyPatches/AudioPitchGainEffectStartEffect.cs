using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatSaberTweaks.HarmonyPatches
{
    [HarmonyPatch(typeof(AudioPitchGainEffect))]
    [HarmonyPatch("StartEffect")]
    [HarmonyPatch(new Type[] { typeof(float), typeof(Action) })]
    class AudioPitchGainEffectStartEffect
    {
        static bool Prefix(AudioPitchGainEffect __instance, float volumeScale, Action finishCallback)
        {
            __instance.StartCoroutine(__instance.StartEffectCoroutine(Settings.MusicVolume, finishCallback));

            // Return false to tell Harmony not to run the original function as well
            return false;
        }
    }
}
