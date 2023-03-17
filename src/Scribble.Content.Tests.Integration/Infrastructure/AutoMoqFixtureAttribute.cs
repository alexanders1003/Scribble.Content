using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using Scribble.Content.Models;

namespace Scribble.Content.Tests.Integration.Infrastructure;

public class AutoMoqFixtureAttribute : AutoDataAttribute
{
    public AutoMoqFixtureAttribute()
        :base(() =>
        {
            var fixture = new Fixture()
                .Customize(new AutoMoqCustomization());
            
            fixture.Register(() => new BlogEntity
            {
                Name = null!,
                Description = "Default Description",
                AuthorId = Guid.NewGuid(),
                Articles = new List<ArticleEntity>()
            });

            return fixture;
        })
    {
        
    }
}