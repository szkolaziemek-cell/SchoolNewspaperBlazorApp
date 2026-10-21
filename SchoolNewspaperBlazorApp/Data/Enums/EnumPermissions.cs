using System.Collections.Generic;

namespace SchoolNewspaperBlazorApp.Data.Enums
{
    /// <summary>Lista uprawnień, które można przypisać użytkownikowi.</summary>
    public enum EnumPermissions
    {
        /// <summary>Zezwala na tworzenie artykułów.</summary>
        AddArticle,
        /// <summary>Zezwala na edycję artykułów.</summary>
        EditArticle,
        /// <summary>Zezwala na usuwanie artykułów.</summary>
        DeleteArticle,
        /// <summary>Zezwala na tworzenie kont użytkowników.</summary>
        AddUser,
        /// <summary>Zezwala na zarządzanie użytkownikami.</summary>
        ManageUser,
        /// <summary>Zezwala na dodawanie plików.</summary>
        AddFile,
        /// <summary>Pełne uprawnienia administracyjne.</summary>
        Master
    }
}