using System.Linq.Expressions;
using System.Reflection;

namespace SimpleValidator.Internal.ExpressionHelpers;

/// <summary>
/// Checks if any member in the expression is nullable base n nullability context.
/// </summary>
internal sealed class NullabilityMembersChecker : ExpressionVisitor
{
    private static readonly NullabilityInfoContext _context = new();

    public bool IsSafeFromNullableMembers { get; private set; } = true;

    protected override Expression VisitMember(MemberExpression node)
    {
        ArgumentNullException.ThrowIfNull(node);

        MemberTypes memberType = node.Member.MemberType;

        if (IsSafeFromNullableMembers)
        {
            switch (memberType)
            {
                case MemberTypes.Event:
                    EventInfo eventInfo = (EventInfo)node.Member;
                    IsSafeFromNullableMembers = _context.Create(eventInfo).ReadState == NullabilityState.NotNull;
                    break;
                case MemberTypes.Field:
                    FieldInfo fieldInfo = (FieldInfo)node.Member;
                    IsSafeFromNullableMembers = _context.Create(fieldInfo).ReadState == NullabilityState.NotNull;
                    break;
                case MemberTypes.Property:
                    PropertyInfo propertyInfo = (PropertyInfo)node.Member;
                    IsSafeFromNullableMembers = _context.Create(propertyInfo).ReadState == NullabilityState.NotNull;
                    break;
                default:
                    break;
            }
        }

        return base.VisitMember(node);
    }
}
