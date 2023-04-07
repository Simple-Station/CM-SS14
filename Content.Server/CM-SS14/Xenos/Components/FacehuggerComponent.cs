namespace Content.Server.CMSS14.Xenos.Components;

[RegisterComponent]
public sealed class FacehuggerComponent : Component
{
    [ViewVariables(VVAccess.ReadWrite)] [DataField("neutered")]
    public bool Neutered = false;
}
