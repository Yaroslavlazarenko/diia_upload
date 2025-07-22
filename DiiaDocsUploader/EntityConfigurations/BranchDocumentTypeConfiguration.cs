using DiiaDocsUploader.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiiaDocsUploader.EntityConfigurations;

public class BranchDocumentTypeConfiguration : IEntityTypeConfiguration<BranchDocumentType>
{
    public void Configure(EntityTypeBuilder<BranchDocumentType> builder)
    {
        builder
            .HasKey(bdt => new { bdt.DocumentTypeId, bdt.BranchId });
    }
}