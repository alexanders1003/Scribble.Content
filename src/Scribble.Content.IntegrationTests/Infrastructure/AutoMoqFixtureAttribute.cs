using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using Scribble.Content.Models;

namespace Scribble.Content.IntegrationTests.Infrastructure;

public class AutoMoqFixtureAttribute : AutoDataAttribute
{
    public AutoMoqFixtureAttribute()
        :base(() =>
        {
            var fixture = new Fixture()
                .Customize(new AutoMoqCustomization());
            
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            fixture.Customize<BlogEntity>(composer => composer.WithAutoProperties());

            
            return fixture;
        })
    {
        
    }
}