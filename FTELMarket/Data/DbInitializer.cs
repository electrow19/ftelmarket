using FTELMarket.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FTELMarket.Data;

public static class DbInitializer
{
    public static void Initialize(FTELMarketDbContext context)
    {
        context.Database.Migrate();

        //utilisateur par défaut
        if (!context.Users.Any())
        {
            context.Users.Add(new User 
            { 
                Id = 1, 
                Username = "AdminFTEL"
            });
        }

        //catégories par défaut
        if (!context.Categories.Any())
        {
            context.Categories.AddRange(
                new Category { Id = 1, Name = "PlayStation 5" },
                new Category { Id = 2, Name = "Nintendo Switch" },
                new Category { Id = 3, Name = "Xbox Series X" },
                new Category { Id = 4, Name = "PC" }
            );
        }

        context.SaveChanges();
    }
}