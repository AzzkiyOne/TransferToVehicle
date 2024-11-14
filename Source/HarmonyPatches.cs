using System.Collections.Generic;
using HarmonyLib;
using Vehicles;
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
            yield return Command_TransferToVehicle.Instance;

            if (__instance.IsOrderedToBeTransferredToAnyVehicle())
            {
                yield return new Command_Action()
                {
                    defaultLabel = "Cancel transfer",
                    defaultDesc = "Cancel transfer of selected things to any vehicle's cargo.",
                    icon = VehicleTex.CancelPackCargoIcon[(uint)VehicleType.Land],
                    Order = 1001f,
                    action = __instance.CancelTransferToAnyVehicle,
                };
            }
        }
    }
}
