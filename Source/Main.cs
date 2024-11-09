using HarmonyLib;
using Verse;

namespace TransferToVehicle;

[StaticConstructorOnStartup]
public static class Main
{
    static Main()
    {
        new Harmony("Azzkiy.TransferToVehicle").PatchAll();
    }
}
