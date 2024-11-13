using System.Collections.Generic;
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
    // Same thing that Vehicles.Dialog_LoadCargo does.
    private void Action(LocalTargetInfo target)
    {
        if (target.Thing is VehiclePawn vehicle)
        {
            var cargoToTransfer = new List<TransferableOneWay>();

            foreach (var obj in Find.Selector.SelectedObjects)
            {
                if (obj is Thing thing && thing.def.EverHaulable)
                {
                    var transferableOneWay = TransferableUtility.TransferableMatching(thing, cargoToTransfer, TransferAsOneMode.PodsOrCaravanPacking);

                    if (transferableOneWay == null)
                    {
                        transferableOneWay = new TransferableOneWay();
                        cargoToTransfer.Add(transferableOneWay);
                    }

                    if (transferableOneWay.things.Contains(thing) == false)
                    {
                        transferableOneWay.things.Add(thing);
                        transferableOneWay.AdjustTo(transferableOneWay.CountToTransfer + thing.stackCount);
                    }
                }
            }

            vehicle.cargoToLoad = cargoToTransfer;
            vehicle.Map.GetCachedMapComponent<VehicleReservationManager>().RegisterLister(vehicle, ReservationType.LoadVehicle);
        }
    }
}
