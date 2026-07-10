using ActsFromThePast.Acts.TheBeyond.Enemies;
using ActsFromThePastTweaks.ActsFromThePastTweaksCode.Powers;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace ActsFromThePastTweaks.ActsFromThePastTweaksCode.Patches;

[HarmonyPatch(typeof(TimeEater))]
public static class ReplaceTimeEaterTimeWarp
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(TimeEater.AfterAddedToRoom))]
    static bool PrefixAfterAddedToRoom(TimeEater __instance, ref Task __result)
    {
        __result = AfterAddedToRoomReplacement(__instance);
        return false;
    }

    private static async Task AfterAddedToRoomReplacement(TimeEater timeEater)
    {
        foreach (Player player in timeEater.Creature.CombatState!.Players)
        {
            TimeWarpReplacementPower? power = await PowerCmd.Apply<TimeWarpReplacementPower>(new ThrowingPlayerChoiceContext(),
                timeEater.Creature, 1, timeEater.Creature, null);

            power?.AssignPlayer(player);
        }
    }
}