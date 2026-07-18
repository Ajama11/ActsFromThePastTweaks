using ActsFromThePast.Acts.TheBeyond.Enemies;
using ActsFromThePast.Powers;
using ActsFromThePastTweaks.ActsFromThePastTweaksCode.Powers;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;

namespace ActsFromThePastTweaks.ActsFromThePastTweaksCode.Patches;

[HarmonyPatch(typeof(Transient))]
public static class ReplaceTransientShifting
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(Transient.AfterAddedToRoom))]
    static bool PrefixAfterAddedToRoom(Transient __instance, ref Task __result)
    {
        __result = AfterAddedToRoomReplacement(__instance);
        return false;
    }

    private static async Task AfterAddedToRoomReplacement(Transient transient)
    {
        await PowerCmd.Apply<FadingPower>(new ThrowingPlayerChoiceContext(), transient.Creature, 
            AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 6, 5), 
            transient.Creature, null);
        
        await PowerCmd.Apply<ShiftingReplacementPower>(new ThrowingPlayerChoiceContext(),
            transient.Creature, 1, transient.Creature, null);
    }
}