namespace SMF.SourceGenerator.Core.Types;
/// <summary>
/// The relationship type.
/// </summary>

public enum RelationshipType
{
    O2O,
    O2M,
    M2O,
    M2M,
}
public record RelationshipWith(TypeProperty HasRelations, TypeProperty WithRelationship, TypeProperty ForeignKey, RelationshipType RelationshipType);
