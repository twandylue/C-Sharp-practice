using System.Reflection;
using MVC.Respository;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;

namespace MVC.GraphQL.Extensions
{
    public class UseApplicationDbContextAttribute : ObjectFieldDescriptorAttribute
    {
        public override void OnConfigure(
            IDescriptorContext context,
            IObjectFieldDescriptor descriptor,
            MemberInfo member)
        {
            descriptor.UseDbContext<PostgresDBContext>();
        }
    }
}