using ActsFromThePast;
using ActsFromThePastTweaks.ActsFromThePastTweaksCode.Extensions;
using ActsFromThePastTweaks.ActsFromThePastTweaksCode.Powers;
using BaseLib.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace ActsFromThePastTweaks.ActsFromThePastTweaksCode.Powers;

public class TimeWarpReplacementPower() : ActsFromThePastTweaksPower
{
    public override PowerType Type =>
        PowerType.Buff;
    public override PowerStackType StackType =>
        PowerStackType.Counter;
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;
    public override bool ShouldScaleInMultiplayer => false;
    public override int DisplayAmount => DynamicVars[CardCountKey].IntValue;

    private const int TakeAdditionalDamage = 2;
    private const int Countdown = 12;
    
    public Player? Player { get; private set; }
    private bool _isLocalPlayer;
    
    public override string CustomPackedIconPath => 
        _isLocalPlayer ? 
            $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath() : 
            $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_otherplayer.png".PowerImagePath();
    
    private const string PlayerNameKey = "PlayerName";
    private const string ApplierNameKey = "ApplierName";
    private const string CardCountKey = "CardCount";
    private const string CountdownKey = "Countdown";
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new StringVar(PlayerNameKey),
        new StringVar(ApplierNameKey),
        new (CardCountKey, 0),
        new (CountdownKey, Countdown)
    ];

    public void AssignPlayer(Player player)
    {
        Player = player;
        ((StringVar) DynamicVars[PlayerNameKey]).StringValue = player.Creature.Name;

        if (LocalContext.IsMe(player))
        {
            _isLocalPlayer = true;
        }

        foreach (NPower nPower in NCombatRoom.Instance!.GetCreatureNode(Owner)!._stateDisplay._powerContainer._powerNodes)
        {
            if (nPower.Model != this) continue;
            
            nPower.Reload();
            nPower._icon.Texture = nPower.Model.Icon;
        }
    }
    
    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        if (applier is { IsMonster: true })
            ((StringVar) DynamicVars[ApplierNameKey]).StringValue = applier.Monster!.Title.GetFormattedText();
        return Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Player) return;
        
        DynamicVars[CardCountKey].BaseValue++;
        InvokeDisplayAmountChanged();
        
        if (DynamicVars[CardCountKey].IntValue >= DynamicVars[CountdownKey].IntValue)
        {
            DynamicVars[CardCountKey].BaseValue = 0M;
            InvokeDisplayAmountChanged();
            
            Flash();

            if (LocalContext.IsMe(Player))
            {
                AFTPModAudio.Play("time_eater", "time_warp");
                BorderFlashEffect.PlayGold();
                NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(TimeWarpTurnEndEffect.Create());
            }
            else
            {
                switch (Config.PlayOtherPlayersTimeWarpEffects)
                {
                    case Config.TimeWarpEffects.Everything:
                        AFTPModAudio.Play("time_eater", "time_warp");
                        BorderFlashEffect.PlayGold();
                        NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(TimeWarpTurnEndEffect.Create());
                        break;
                    case Config.TimeWarpEffects.Visuals:
                        BorderFlashEffect.PlayGold();
                        NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(TimeWarpTurnEndEffect.Create());
                        break;
                    case Config.TimeWarpEffects.Audio:
                        AFTPModAudio.Play("time_eater", "time_warp");
                        break;
                    case Config.TimeWarpEffects.Nothing:
                    default:
                        break;
                }
            }
            
            PlayerCmd.EndTurn(Player, false);
            
            await PowerCmd.Apply<TimeWarpedPower>(
                choiceContext, 
                Player.Creature, TakeAdditionalDamage, 
                Owner, null);
        }
    }
}