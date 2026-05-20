namespace Api.Database.EntityConfigurations;

public class ProductEntityConfiguration :
    IEntityTypeConfiguration<Product>
{
    public void Configure(
        EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(x => x.Price)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true)
            .IsRequired();

        
        builder.HasOne(x => x.ProductCategory)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.ProductCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.CreatedAt)
            .HasColumnType("datetime");

        builder.Property(x => x.UpdatedAt)
            .HasColumnType("datetime");


        builder.HasIndex(x => x.Name);

        builder.HasIndex(x => x.ProductCategoryId);

        builder.HasIndex(x => x.IsActive);
    }
}
