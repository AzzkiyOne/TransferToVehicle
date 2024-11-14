using System.Collections.Generic;
using RimWorld;
using Vehicles;
using Verse;

namespace TransferToVehicle;

internal class Command_TransferToVehicle : Command_Target
{
    public static Command_TransferToVehicle Instance { get; }
    public Command_TransferToVehicle()
    {
        defaultLabel = "Transfer to vehicle";
        defaultDesc = "Transfer selected things to the target vehicle's cargo.";
        icon = VehicleTex.PackCargoIcon[(uint)VehicleType.Land];
        Order = 1000f;
        action = Action;
        targetingParams = TargetingParameters.ForPawns();
        targetingParams.validator = IsVehicle;
    }
    static Command_TransferToVehicle()
    {
        Instance = new Command_TransferToVehicle();
    }
    private static bool IsVehicle(TargetInfo target)
    {
        return target.Thing is VehiclePawn;
    }
    private static IEnumerable<Thing> GetSelectedTransferableThings()
    {
        foreach (var obj in Find.Selector.SelectedObjects)
        {
            if (obj is Thing thing && thing.CanBeTransferredToVehiclesCargo())
            {
                yield return thing;
            }
        }
    }
    private static void Action(LocalTargetInfo target)
    {
        if (target.Thing is VehiclePawn vehicle)
        {
            GetSelectedTransferableThings().TransferToVehicle(vehicle);
        }
    }
}
