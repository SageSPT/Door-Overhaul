using System.Reflection;
using EFT;
using EFT.Interactive;
using EFT.UI;
using DoorOverhaul.Actions;
using HarmonyLib;
using SPT.Reflection.Patching;

namespace DoorOverhaul.Patches
{
    internal class AddDoorActionsPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(InteractionContextHelper), nameof(InteractionContextHelper.GetAvailableActions),
                new[] { typeof(GamePlayerOwner), typeof(Door) });
        }

        [PatchPostfix]
        private static void Postfix(AvailableInteractionState __result, GamePlayerOwner owner, Door door)
        {
            if (__result == null || door == null)
            {
                return;
            }

            Player player = owner?.Player;
            if (player == null || !player.IsYourPlayer)
            {
                return;
            }

            DoorActions.AppendMissing(__result.Actions, owner, door);
        }
    }
}
