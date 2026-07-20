using ActsFromThePast;
using ActsFromThePastTweaks.ActsFromThePastTweaksCode.Powers;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;

namespace ActsFromThePastTweaks.ActsFromThePastTweaksCode.Patches;

[HarmonyPatch(typeof(GremlinNob), "Bellow")]
public static class AddNewGremlinNobEnrage
{
    [HarmonyPostfix]
    static async Task PostfixBellow(Task __result, GremlinNob __instance, IReadOnlyList<Creature> targets)
    { 
        await Cmd.Wait(1f);
        
        if (!__instance.Creature.HasPower<EnrageReplacementPower>())
            await PowerCmd.Apply<EnrageReplacementPower>(
                new ThrowingPlayerChoiceContext(), __instance.Creature,
                AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 2), 
                __instance.Creature, null);
    }
}