using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using Scribble.Content.UnitTests.Fixtures;

namespace Scribble.Content.UnitTests;

public class AutoMoqFixtureAttribute : AutoDataAttribute
{
    public AutoMoqFixtureAttribute()
        :base(() => new Fixture()
            .Customize(new AutoMoqCustomization())
            .Customize(new ApplicationDbContextCustomization()))
    {
        
    }
}