using DiiaDocsUploader.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiiaDocsUploader.EntityConfigurations;

public class BranchConfiguration : IEntityTypeConfiguration<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder.Property(b => b.Id)
            .ValueGeneratedNever();
        
        builder.Ignore(b => b.DeliveryTypes);
        builder.Ignore(b => b.OfferRequestType);
    }
}