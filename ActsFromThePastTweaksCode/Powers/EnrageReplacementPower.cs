using ActsFromThePastTweaks.ActsFromThePastTweaksCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace ActsFromThePastTweaks.ActsFromThePastTweaksCode.Powers;

public class EnrageReplacementPower()
    : ActsFromThePastTweaksPower
{
    public override string CustomPackedIconPath => 
        ImageHelper.GetImagePath("atlases/power_atlas.sprites/enrage_power.tres");
    public override string CustomBigIconPath => 
        ImageHelper.GetImagePath("powers/enrage_power.png");
    
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    private const string ApplierName = "ApplierName";
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new StringVar(ApplierName)
    ];
    
    public override async Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        if (applier is { IsMonster: true })
            ((StringVar) DynamicVars[ApplierName]).StringValue = applier.Monster!.Title.GetFormattedText();
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Type == CardType.Skill)
        {
            await PowerCmd.Apply<DrawnIrePower>(choiceContext, cardPlay.Card.Owner.Creature, 
                Amount, Owner, null);
        }
    }
}