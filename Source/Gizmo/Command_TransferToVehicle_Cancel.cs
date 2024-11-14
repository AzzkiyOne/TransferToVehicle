using Vehicles;
using Verse;

namespace TransferToVehicle;

internal class Command_TransferToVehicle_Cancel : Command_Action
{
    public static Command_TransferToVehicle_Cancel Instance { get; }
    public Command_TransferToVehicle_Cancel()
    {
        defaultLabel = "Cancel transfer";
        defaultDesc = "Cancel transfer of selected things to any vehicle's cargo.";
        icon = VehicleTex.CancelPackCargoIcon[(uint)VehicleType.Land];
        Order = 1001f;
        action = Action;
    }
    static Command_TransferToVehicle_Cancel()
    {
        Instance = new Command_TransferToVehicle_Cancel();
    }
    private static void Action()
    {
        foreach (var thing in Command_TransferToVehicle_Order.GetSelectedTransferableThings())
        {
            thing.CancelTransferToAnyVehicle();
        }
    }
}
