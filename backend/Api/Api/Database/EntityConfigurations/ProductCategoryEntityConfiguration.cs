namespace Api.Database.EntityConfigurations;

public class ProductCategoryEntityConfiguration :
    IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(
        EntityTypeBuilder<ProductCategory> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasColumnType("NVARCHAR(100)")
            .IsRequired();

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.Property(x => x.Description)
            .HasColumnType("NVARCHAR(500)");

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);

        builder.Property(x => x.CreatedAt)
            .HasColumnType("datetime");

        builder.Property(x => x.UpdatedAt)
            .HasColumnType("datetime");
    }
}
