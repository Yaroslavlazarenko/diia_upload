using DiiaDocsUploader.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiiaDocsUploader.EntityConfigurations;

public class DocumentTypeConfiguration : IEntityTypeConfiguration<DocumentType>
{
    public void Configure(EntityTypeBuilder<DocumentType> builder)
    {
        builder.HasData(
            new DocumentType { Id = 1, NameDiia = "internal-passport", NameUa = "Паспорт громадянина України у формі ID-картки" },
            new DocumentType { Id = 2, NameDiia = "foreign-passport", NameUa = "Біометричний закордонний паспорт або закордонний паспорт" },
            new DocumentType { Id = 3, NameDiia = "taxpayer-card", NameUa = "РНОКПП" },
            new DocumentType { Id = 4, NameDiia = "user-birth-record", NameUa = "Свідоцтво про народження користувача" },
            new DocumentType { Id = 5, NameDiia = "birth-certificate", NameUa = "Свідоцтво про народження дитини" },
            new DocumentType { Id = 6, NameDiia = "reference-internally-displaced-person", NameUa = "Довідка внутрішньо переміщеної особи (ВПО)" },
            new DocumentType { Id = 7, NameDiia = "student-id-card", NameUa = "Студентський квиток" },
            new DocumentType { Id = 8, NameDiia = "pension-card", NameUa = "Пенсійне посвідчення" },
            new DocumentType { Id = 9, NameDiia = "name-change-act-record", NameUa = "Відомості АЗ про зміну імені" },
            new DocumentType { Id = 10, NameDiia = "marriage-act-record", NameUa = "Відомості АЗ про укладання шлюбу" },
            new DocumentType { Id = 11, NameDiia = "divorce-act-record", NameUa = "Відомості АЗ про розірвання шлюбу" },
            new DocumentType { Id = 12, NameDiia = "veteran-certificate", NameUa = "Посвідчення ветерана" },
            new DocumentType { Id = 13, NameDiia = "education-document", NameUa = "Освітні документи" },
            new DocumentType { Id = 14, NameDiia = "residence-permit-permanent", NameUa = "Е-посвідка на постійне проживання" },
            new DocumentType { Id = 15, NameDiia = "residence-permit-temporary", NameUa = "Е-посвідка на тимчасове проживання" }
        );
    }
}