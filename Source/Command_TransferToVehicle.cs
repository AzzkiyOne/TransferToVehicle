using RimWorld;
using SmashTools;
using UnityEngine;
using Vehicles;
using Verse;

namespace TransferToVehicle;

internal class Command_TransferToVehicle : Command_Target
{
    public static Command_TransferToVehicle Instance { get; }
    public Command_TransferToVehicle()
    {
        defaultLabel = "Transfer to vehicle";
        defaultDesc = "Transfer to vehicle.";
        icon = ContentFinder<Texture2D>.Get("UI/Gizmos/StartLoadVehicle");
        Order = 1000f;
        action = Action;
        targetingParams = TargetingParameters.ForPawns();
        targetingParams.validator = IsVehicle;
    }
    static Command_TransferToVehicle()
    {
        Instance = new Command_TransferToVehicle();
    }
    private bool IsVehicle(TargetInfo target)
    {
        return target.Thing is VehiclePawn;
    }
    // Same thing that Vehicles.Dialog_LoadCargo does,
    // but with ability to add more things to the list while loading is in progress.
    private void Action(LocalTargetInfo target)
    {
        if (target.Thing is VehiclePawn vehicle)
        {
            vehicle.cargoToLoad ??= [];

            foreach (var obj in Find.Selector.SelectedObjects)
            {
                if (obj is Thing thing && thing.def.EverHaulable)
                {
                    var transferableOneWay = TransferableUtility.TransferableMatching(thing, vehicle.cargoToLoad, TransferAsOneMode.PodsOrCaravanPacking);

                    if (transferableOneWay == null)
                    {
                        transferableOneWay = new TransferableOneWay();
                        vehicle.cargoToLoad.Add(transferableOneWay);
                    }

                    if (transferableOneWay.things.Contains(thing) == false)
                    {
                        transferableOneWay.things.Add(thing);
                        transferableOneWay.AdjustTo(transferableOneWay.CountToTransfer + thing.stackCount);
                    }
                }
            }

            vehicle.Map.GetCachedMapComponent<VehicleReservationManager>().RegisterLister(vehicle, ReservationType.LoadVehicle);
        }
    }
}
