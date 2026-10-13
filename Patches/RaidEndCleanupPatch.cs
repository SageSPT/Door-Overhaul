using System.Reflection;
using EFT;
using DoorOverhaul.Doors;
using HarmonyLib;
using SPT.Reflection.Patching;

namespace DoorOverhaul.Patches
{
    internal class RaidEndCleanupPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(GameWorld), nameof(GameWorld.OnDestroy));
        }

        [PatchPostfix]
        private static void Postfix()
        {
            PeekDriver.Cancel();
            DoorPusher.Clear();
            KickWeaponForwardPatch.Clear();
            DoorOpenOverrides.Clear();
            Destruction.DestructionVisuals.Clear();
            Destruction.DestructionFragments.Clear();
            Destruction.DoorDamageIntake.Clear();
            Destruction.DoorDestructionRegistry.Clear();
        }
    }
}
