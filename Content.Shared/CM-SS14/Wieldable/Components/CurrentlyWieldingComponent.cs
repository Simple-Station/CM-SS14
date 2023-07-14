using Content.Shared.Wieldable;
using Content.Shared.Wieldable.Components;

namespace Content.Shared.CMSS14.Wieldable.Components;

[RegisterComponent]
public sealed class CurrentlyWieldingComponent : Component
{
    public float WalkMod;
    public float SprintMod;
}
