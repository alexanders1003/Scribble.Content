using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using Scribble.Content.Tests.Unit.Fixtures;

namespace Scribble.Content.Tests.Unit;

public class AutoMoqFixtureAttribute : AutoDataAttribute
{
    public AutoMoqFixtureAttribute()
        :base(() => new Fixture()
            .Customize(new AutoMoqCustomization())
            .Customize(new ApplicationDbContextCustomization()))
    {
        
    }
}