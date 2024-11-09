using System.Collections.Generic;
using System.Linq;
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
            return __result.Append(Command_TransferToVehicle.Instance);
        }

        return __result;
    }
}
