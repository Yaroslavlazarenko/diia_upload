using DiiaDocsUploader.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiiaDocsUploader.EntityConfigurations;

public class OfferDocumentTypeConfiguration : IEntityTypeConfiguration<OfferDocumentType>
{
    public void Configure(EntityTypeBuilder<OfferDocumentType> builder)
    {
        builder
            .HasKey(odt => new { odt.DocumentTypeId, odt.OfferId });
    }
}