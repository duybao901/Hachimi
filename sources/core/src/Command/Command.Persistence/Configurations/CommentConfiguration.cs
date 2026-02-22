//using Command.Domain.Entities;
//using Command.Persistence.Contants;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//internal class CommentConfiguration : IEntityTypeConfiguration<Comment>
//{
//    public void Configure(EntityTypeBuilder<Comment> builder)
//    {
//        builder.ToTable(TableNames.Comments);
//        builder.HasKey(x => x.Id);
//        builder.Property(x => x.Content).HasMaxLength(1000).IsRequired();
//        builder.Property(x => x.CreatedOnUtc).IsRequired();
//        builder.HasIndex(x => x.PostId);
//    }
//}
