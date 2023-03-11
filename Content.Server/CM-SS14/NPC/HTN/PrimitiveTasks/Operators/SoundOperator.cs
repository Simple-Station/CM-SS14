using Content.Server.NPC;
using Content.Server.NPC.HTN;
using Content.Server.NPC.HTN.PrimitiveTasks;
using Robust.Shared.Audio;

namespace Content.Server.CMSS14.NPC.HTN.PrimitiveTasks.Operators;

public sealed class SoundOperator : HTNOperator
{
    private SharedAudioSystem _audio = default!;

    [DataField("sound", required: true)]
    public SoundSpecifier Sound = default!;

    public override void Initialize(IEntitySystemManager sysManager)
    {
        base.Initialize(sysManager);
        _audio = IoCManager.Resolve<IEntitySystemManager>().GetEntitySystem<SharedAudioSystem>();
    }

    public override HTNOperatorStatus Update(NPCBlackboard blackboard, float frameTime)
    {
        var source = blackboard.GetValue<EntityUid>(NPCBlackboard.Owner);

        _audio.PlayPvs(Sound, source);
        return base.Update(blackboard, frameTime);
    }
}
