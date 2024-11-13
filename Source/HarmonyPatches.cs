using System.Collections.Generic;
using HarmonyLib;
using Verse;

namespace TransferToVehicle;

[HarmonyPatch(typeof(Thing), nameof(Thing.GetGizmos))]
internal static class HarmonyPatch_Thing_GetGizmos
{
    internal static IEnumerable<Gizmo> Postfix(IEnumerable<Gizmo> __result, Thing __instance)
    {
        if (__instance.def.EverHaulable)
        {
            yield return Command_TransferToVehicle.Instance;
        }

        IEnumerator<Gizmo> enumerator = __result.GetEnumerator();

        while (enumerator.MoveNext())
        {
            yield return enumerator.Current;
        }
    }
}
