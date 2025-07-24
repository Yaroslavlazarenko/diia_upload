using DiiaDocsUploader.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiiaDocsUploader.EntityConfigurations;

public class DocumentMetadataConfiguration : IEntityTypeConfiguration<DocumentMetadata>
{
    public void Configure(EntityTypeBuilder<DocumentMetadata> builder)
    {
        builder.HasKey(d => d.DeepLinkId);

        builder.Property(d => d.DeepLinkId)
            .ValueGeneratedNever();
        
        builder.HasMany(dm => dm.DocumentFiles)
            .WithOne(df => df.DocumentMetadata)
            .HasForeignKey(df => df.DeepLinkId)
            .HasPrincipalKey(dm => dm.DeepLinkId);
    }
}