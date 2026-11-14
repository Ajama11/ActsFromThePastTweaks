using System.Reflection;
using System.Reflection.Emit;
using ActsFromThePast;
using ActsFromThePast.Acts.TheBeyond.Enemies;
using ActsFromThePastTweaks.ActsFromThePastTweaksCode.Powers;
using BaseLib.Utils.Patching;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Powers;

namespace ActsFromThePastTweaks.ActsFromThePastTweaksCode.Patches;

[HarmonyPatch]
public static class RemoveOriginalGremlinNobEnrage
{
    [HarmonyPatch(typeof(GremlinNob), "Bellow", MethodType.Async)]
    [HarmonyTranspiler]
    static List<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        CodeMatcher matcher = new CodeMatcher(instructions)
            .MatchStartForward([
                new CodeMatch(OpCodes.Ldloc_1),  // IL_00c0 V_1
                new CodeMatch(OpCodes.Call),     // get_Creature()
                new CodeMatch(OpCodes.Callvirt), // get_CombatState()
                new CodeMatch(OpCodes.Dup),      // 
                new CodeMatch(OpCodes.Brtrue_S), // 
                new CodeMatch(OpCodes.Pop),      // 
            ])
            .ThrowIfInvalid("RemoveOriginalGremlinNobEnrage could not find the correct position")
            .RemoveUntilForward([
                new CodeMatch(OpCodes.Leave_S) // IL_0138, right before the beq destination
            ]);

        return matcher.InstructionEnumeration().ToList();
    }
}