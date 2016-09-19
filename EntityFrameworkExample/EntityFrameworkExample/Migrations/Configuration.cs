using EntityFrameworkExample.Model;

namespace EntityFrameworkExample.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<EntityFrameworkExample.Northwind>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(EntityFrameworkExample.Northwind context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.Categories.AddOrUpdate(c => c.CategoryID,
                new Category {CategoryID = 1, CategoryName = "Beverages", Description = "Soft drinks, coffees, teas, beers, and ales" },
                new Category {CategoryID = 2, CategoryName = "Condiments", Description = "Sweet and savory sauces, relishes, spreads, and seasonings" },
                new Category {CategoryID = 3, CategoryName = "Confections", Description = "Desserts, candies, and sweet breads" },
                new Category {CategoryID = 4, CategoryName = "Dairy Products", Description = "Cheeses" });

            context.Regions.AddOrUpdate(r => r.RegionID,
                new Region {RegionID = 1, RegionDescription = "Eastern" },
                new Region {RegionID = 2, RegionDescription = "Western" },
                new Region {RegionID = 3, RegionDescription = "Northern" },
                new Region {RegionID = 4, RegionDescription = "Southern" });

            context.Territories.AddOrUpdate(t => t.TerritoryID,
                new Territory {TerritoryID = "01581", RegionID = 1, TerritoryDescription = "Westboro" },
                new Territory {TerritoryID = "01730", RegionID = 1, TerritoryDescription = "Bedford" },
                new Territory {TerritoryID = "01833", RegionID = 1, TerritoryDescription = "Georgetow" },
                new Territory {TerritoryID = "03049", RegionID = 3, TerritoryDescription = "Hollis" });
        }
    }
}
