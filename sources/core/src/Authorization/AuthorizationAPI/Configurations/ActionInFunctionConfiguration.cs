using AuthorizationAPI.Contants;
using AuthorizationAPI.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthorizationAPI.Configurations;

internal class ActionInFunctionConfiguration : IEntityTypeConfiguration<ActionInFunction>
{
    public void Configure(EntityTypeBuilder<ActionInFunction> builder)
    {
        builder.ToTable(TableNames.ActionInFunctions);

        builder.HasKey(ActionInFunctions => new { ActionInFunctions.ActionId, ActionInFunctions.FunctionId });
    }
}
