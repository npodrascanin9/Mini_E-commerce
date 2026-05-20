namespace Api.Database.EntityConfigurations;

public class ArticleEntityConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.ToTable("Articles");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Barcode)
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(x => x.Color)
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(x => x.Size)
            .HasMaxLength(20)
            .IsRequired(false);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .IsRequired();

        builder.Property(x => x.ExpirationDate)
            .IsRequired(false);


        #region FK relationships
        builder.HasOne(x => x.Product)
            .WithMany(x => x.Articles)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
        #endregion

        #region Indexes
        builder.HasIndex(x => x.ProductId);

        builder.HasIndex(x => x.Barcode);

        builder.HasIndex(x => x.IsActive);
        #endregion
    }
}
