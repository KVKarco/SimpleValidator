namespace SimpleValidator.Internal.ExpressionHelpers;

using System.Linq.Expressions;
using System.Reflection;

internal sealed class NullabilityMembersChecker : ExpressionVisitor
{
    private static readonly NullabilityInfoContext Context = new NullabilityInfoContext();

    public bool IsSafeFromNullableMembers { get; private set; } = true;

    protected override Expression VisitMember(MemberExpression node)
    {
        ArgumentNullException.ThrowIfNull(node);

        MemberTypes memberType = node.Member.MemberType;

        if (this.IsSafeFromNullableMembers)
        {
            switch (memberType)
            {
                case MemberTypes.Event:
                    EventInfo eventInfo = (EventInfo)node.Member;
                    this.IsSafeFromNullableMembers = Context.Create(eventInfo).ReadState == NullabilityState.NotNull;
                    break;
                case MemberTypes.Field:
                    FieldInfo fieldInfo = (FieldInfo)node.Member;
                    this.IsSafeFromNullableMembers = Context.Create(fieldInfo).ReadState == NullabilityState.NotNull;
                    break;
                case MemberTypes.Property:
                    PropertyInfo propertyInfo = (PropertyInfo)node.Member;
                    this.IsSafeFromNullableMembers = Context.Create(propertyInfo).ReadState == NullabilityState.NotNull;
                    break;
                default:
                    break;
            }
        }

        return base.VisitMember(node);
    }
}
