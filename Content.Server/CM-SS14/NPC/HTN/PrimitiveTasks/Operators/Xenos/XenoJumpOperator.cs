using System.Threading;
using System.Threading.Tasks;
using Content.Server.CMSS14.NPC.Components;
using Content.Server.NPC;
using Content.Server.NPC.Components;
using Content.Server.NPC.HTN;
using Content.Server.NPC.HTN.PrimitiveTasks;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Components;
using Robust.Shared.Audio;

namespace Content.Server.CMSS14.NPC.HTN.PrimitiveTasks.Operators.Xenos;

public sealed class XenoJumpOperator : HTNOperator
{
    [Dependency] private readonly IEntityManager _entManager = default!;

    /// <summary>
    /// Key that contains the target entity.
    /// </summary>
    [DataField("targetKey", required: true)]
    public string TargetKey = default!;

    /// <summary>
    /// Minimum damage state that the target has to be in for us to consider attacking.
    /// </summary>
    [DataField("targetState")]
    public MobState TargetState = MobState.Alive;

    // Like movement we add a component and pass it off to the dedicated system.

    public override async Task<(bool Valid, Dictionary<string, object>? Effects)> Plan(NPCBlackboard blackboard,
        CancellationToken cancelToken)
    {
        // Don't attack if they're already as wounded as we want them.
        if (!blackboard.TryGetValue<EntityUid>(TargetKey, out var target, _entManager))
        {
            return (false, null);
        }

        if (_entManager.TryGetComponent<MobStateComponent>(target, out var mobState) &&
            mobState.CurrentState != null &&
            mobState.CurrentState > TargetState)
        {
            return (false, null);
        }

        return (true, null);
    }

    public override void Startup(NPCBlackboard blackboard)
    {
        base.Startup(blackboard);
        var ranged = _entManager.EnsureComponent<NPCXenoJumpComponent>(blackboard.GetValue<EntityUid>(NPCBlackboard.Owner));
    }

    public override void Shutdown(NPCBlackboard blackboard, HTNOperatorStatus status)
    {
        base.Shutdown(blackboard, status);
        _entManager.RemoveComponent<NPCXenoJumpComponent>(blackboard.GetValue<EntityUid>(NPCBlackboard.Owner));
        blackboard.Remove<EntityUid>(TargetKey);
    }

    public override HTNOperatorStatus Update(NPCBlackboard blackboard, float frameTime)
    {
        base.Update(blackboard, frameTime);
        var owner = blackboard.GetValue<EntityUid>(NPCBlackboard.Owner);
        var status = HTNOperatorStatus.Continuing;

        Logger.DebugS("htn", "XenoJumpOperator.Update()");

        if (_entManager.TryGetComponent<NPCXenoJumpComponent>(owner, out var combat))
        {
            // Success
            if (combat.HasJumped)
            {
                status = HTNOperatorStatus.Finished;
            }
            else
            {
                switch (combat.Status)
                {
                    case CombatStatus.TargetUnreachable:
                    case CombatStatus.NotInSight:
                        status = HTNOperatorStatus.Failed;
                        break;
                    case CombatStatus.Normal:
                        status = HTNOperatorStatus.Continuing;
                        break;
                    default:
                        status = HTNOperatorStatus.Failed;
                        break;
                }
            }
        }

        Logger.DebugS("htn", "XenoJumpOperator.Update() - returning {0}", status);

        return status;
    }
}
