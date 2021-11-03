using HotChocolate.Types;
using MVC.Models;

namespace MVC.GraphQL.Platforms
{
    public class PlatformType : ObjectType<Platform>
    {
        protected override void Configure(IObjectTypeDescriptor<Platform> descriptor)
        {
            descriptor.Description("Represents any software or service that has a coomand line interface");
            descriptor
                .Field(p => p.LicenseKey).Ignore();
        }
    } 
}