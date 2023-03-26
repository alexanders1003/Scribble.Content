using AutoFixture;
using AutoFixture.Kernel;
using Microsoft.EntityFrameworkCore;
using Scribble.Content.Infrastructure.Contexts;

namespace Scribble.Content.Tests.Fixtures;

public class ApplicationDbContextCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        var specimenFactory = new SpecimenFactory<ApplicationDbContext>(CreateDbContext);
        fixture.Customize<ApplicationDbContext>(
            composer => composer.FromFactory(specimenFactory)
        );
    }
    
    private static ApplicationDbContext CreateDbContext()
    {
        var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("Scribble.Content.UnitTests")
            .Options;
        var dbContext = new ApplicationDbContext(dbContextOptions);        
        return dbContext;
    }
}