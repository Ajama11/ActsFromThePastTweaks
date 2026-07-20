using BaseLib.Config;

namespace ActsFromThePastTweaks.ActsFromThePastTweaksCode;

internal class Config : SimpleModConfig
{
    public enum TimeWarpEffects {Everything, Visuals, Audio, Nothing}
    [ConfigHoverTip]
    public static TimeWarpEffects PlayOtherPlayersTimeWarpEffects { get; set; } = TimeWarpEffects.Everything;
}