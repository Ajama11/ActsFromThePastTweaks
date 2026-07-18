using ActsFromThePastTweaks.ActsFromThePastTweaksCode.Extensions;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Powers;

namespace ActsFromThePastTweaks.ActsFromThePastTweaksCode.Powers;

public class ShiftingStrengthDownReplacementPower : CustomTemporaryPowerModelWrapper<ShiftingReplacementPower, StrengthPower>
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override bool InvertInternalPowerAmount => true;

    public override LocString Title => new ("powers", $"{Id.Entry.ToUpperInvariant()}.title");

    public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
    public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
}