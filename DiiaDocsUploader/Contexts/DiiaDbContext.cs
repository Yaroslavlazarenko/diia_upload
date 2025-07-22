using DiiaDocsUploader.Entity;
using DiiaDocsUploader.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace DiiaDocsUploader.Contexts;

public class DiiaDbContext : DbContext
{
    public DiiaDbContext(DbContextOptions<DiiaDbContext> options) : base(options) {}

    public DbSet<DocumentMetadata> DocumentMetadatas { get; set; }
    
    public DbSet<DocumentFile> DocumentFiles { get; set; }
    
    public DbSet<Branch> Branches { get; set; } = null!;
    
    public DbSet<DocumentType> DocumentTypes { get; set; } = null!;
    
    public DbSet<Offer> Offers { get; set; } = null!;

    public DbSet<OfferDocumentType> OfferDocumentTypes { get; set; } = null!;

    public DbSet<BranchDocumentType> BranchDocumentTypes { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BranchConfiguration());
        modelBuilder.ApplyConfiguration(new DocumentFileConfiguration());
        modelBuilder.ApplyConfiguration(new DocumentMetadataConfiguration());
        modelBuilder.ApplyConfiguration(new DocumentTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OfferConfiguration());
        modelBuilder.ApplyConfiguration(new BranchDocumentTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OfferDocumentTypeConfiguration());
    }
}