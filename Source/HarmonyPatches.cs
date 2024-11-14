using System.Collections.Generic;
using HarmonyLib;
using Verse;

namespace TransferToVehicle;

[HarmonyPatch(typeof(Thing), nameof(Thing.GetGizmos))]
internal static class HarmonyPatch_Thing_GetGizmos
{
    internal static IEnumerable<Gizmo> Postfix(IEnumerable<Gizmo> __result, Thing __instance)
    {
        IEnumerator<Gizmo> enumerator = __result.GetEnumerator();

        while (enumerator.MoveNext())
        {
            yield return enumerator.Current;
        }

        if (__instance.CanBeTransferredToVehiclesCargo())
        {
            yield return Command_TransferToVehicle_Order.Instance;

            if (__instance.IsOrderedToBeTransferredToAnyVehicle())
            {
                yield return Command_TransferToVehicle_Cancel.Instance;
            }
        }
    }
}
