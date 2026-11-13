// using System.Reflection;
// using System.Reflection.Emit;
// using ActsFromThePast;
// using ActsFromThePast.Acts.TheBeyond.Enemies;
// using ActsFromThePastTweaks.ActsFromThePastTweaksCode.Powers;
// using BaseLib.Utils.Patching;
// using HarmonyLib;
// using MegaCrit.Sts2.Core.Commands;
// using MegaCrit.Sts2.Core.Entities.Ascension;
// using MegaCrit.Sts2.Core.Entities.Players;
// using MegaCrit.Sts2.Core.GameActions.Multiplayer;
// using MegaCrit.Sts2.Core.Helpers;
// using MegaCrit.Sts2.Core.Models.Powers;
//
// namespace ActsFromThePastTweaks.ActsFromThePastTweaksCode.Patches;
//
// [HarmonyPatch]
// public static class RemoveOriginalGremlinNobEnrage
// {
//     [HarmonyPatch(typeof(GremlinNob), "Bellow", MethodType.Async)]
//     [HarmonyTranspiler]
//     static List<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
//     {
//         // MethodInfo applyNewEnrage = typeof(ReplaceGremlinNobEnrage).Method(nameof(ApplyNewEnrage));
//         
//         CodeMatcher matcher = new CodeMatcher(instructions)
//             .MatchStartForward([
//                 new CodeMatch(OpCodes.Ldarg_0),
//                 new CodeMatch(OpCodes.Ldarg_0),
//                 new CodeMatch(OpCodes.Ldfld),
//                 new CodeMatch(OpCodes.Call),
//                 new CodeMatch(OpCodes.Callvirt),
//                 new CodeMatch(OpCodes.Callvirt),
//                 new CodeMatch(OpCodes.Callvirt),
//                 new CodeMatch(OpCodes.Ldc_I4_2),
//                 new CodeMatch(OpCodes.Bgt_S)
//             ])
//             .ThrowIfInvalid("RemoveOriginalGremlinNobEnrage could not find the correct position")
//             .RemoveUntilForward([
//                 new CodeMatch(OpCodes.Leave_S)
//                 // new CodeMatch(OpCodes.Stloc_3)
//             ]);
//             // .Insert([
//             //     new CodeInstruction(OpCodes.Ldarg_0),
//             //     new CodeInstruction(OpCodes.Call, applyNewEnrage)
//             // ]);
//
//         return matcher.InstructionEnumeration().ToList();
//     }
//
//     // private static void ApplyNewEnrage(GremlinNob gremlinNob)
//     // {
//     //     MainFile.Logger.Info("The transpiler is running!");
//     //     
//     //     TaskHelper.RunSafely(PowerCmd.Apply<WeakPower>(
//     //         new ThrowingPlayerChoiceContext(), gremlinNob.Creature,
//     //         AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 2),
//     //         gremlinNob.Creature, null));
//     //     
//     //     MainFile.Logger.Info("The transpiler has finished running!");
//     // }
// }