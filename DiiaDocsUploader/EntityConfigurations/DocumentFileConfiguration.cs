using DiiaDocsUploader.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiiaDocsUploader.EntityConfigurations;

public class DocumentFileConfiguration : IEntityTypeConfiguration<DocumentFile>
{
    public void Configure(EntityTypeBuilder<DocumentFile> builder)
    {
        builder.HasOne(b=> b.DocumentMetadata)
            .WithMany()
            .HasForeignKey(p=> p.DeepLinkId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}