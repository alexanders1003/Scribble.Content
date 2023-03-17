using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using Scribble.Content.Tests.Fixtures;

namespace Scribble.Content.Tests;

public class AutoMoqFixtureAttribute : AutoDataAttribute
{
    public AutoMoqFixtureAttribute()
        :base(() => new Fixture()
            .Customize(new AutoMoqCustomization())
            .Customize(new ApplicationDbContextCustomization()))
    {
        
    }
}