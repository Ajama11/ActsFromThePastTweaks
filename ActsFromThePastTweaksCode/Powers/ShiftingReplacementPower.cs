using ActsFromThePast.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace ActsFromThePastTweaks.ActsFromThePastTweaksCode.Powers;

public class ShiftingReplacementPower() : ActsFromThePastTweaksPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.None;

    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target,
        DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target != Owner) return;
        if (result.TotalDamage <= 0) return;

        Flash();

        if (dealer != null)
        {
            Creature player = dealer;
            if (dealer.PetOwner != null) player = dealer.PetOwner.Creature;
            
            await PowerCmd.Apply<ShiftingWardPower>(choiceContext,
                player, result.TotalDamage,
                Owner, null);
        }
        else
        {
            // The original code doesn't care if dealer is null, and for dealer-less damage like Poison, it should fallback on the original behavior since we can't know who to give Ward to. And I think it's clearer to do it as Strength Down instead of giving Ward to all players.
            await PowerCmd.Apply<ShiftingStrengthDownReplacementPower>(choiceContext,
                Owner, result.TotalDamage,
                Owner, null);
        }
    }
}