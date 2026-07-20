using ActsFromThePastTweaks.ActsFromThePastTweaksCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace ActsFromThePastTweaks.ActsFromThePastTweaksCode.Powers;

public class DrawnIrePower() : ActsFromThePastTweaksPower
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    private const string ApplierName = "ApplierName";
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new StringVar(ApplierName)
    ];
    
    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        if (applier is { IsMonster: true })
            ((StringVar) DynamicVars[ApplierName]).StringValue = applier.Monster!.Title.GetFormattedText();
        
        return Task.CompletedTask;
    }

    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer,
        CardModel? cardSource, CardPlay? cardPlay)
    {
        if (target != Owner || !props.IsPoweredAttack()) return 0;

        return dealer == Applier ? Amount : 0;
    }

    public override async Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
    {
        if (!wasRemovalPrevented && creature == Applier)
            await PowerCmd.Remove(this);
    }
}