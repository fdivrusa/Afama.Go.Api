using Afama.Go.Api.Domain.Common;
using FluentAssertions;
using NUnit.Framework;

namespace Afama.Go.Api.Domain.UnitTests.Common;

public class BaseEntityTests
{
    private class TestEntity : BaseEntity { }

    private class TestDomainEvent : BaseEvent { }

    [Test]
    public void AddDomainEvent_ShouldAddEvent()
    {
        var entity = new TestEntity();
        var domainEvent = new TestDomainEvent();

        entity.AddDomainEvent(domainEvent);

        entity.DomainEvents.Should().ContainSingle().Which.Should().Be(domainEvent);
    }

    [Test]
    public void RemoveDomainEvent_ShouldRemoveEvent()
    {
        var entity = new TestEntity();
        var domainEvent = new TestDomainEvent();
        entity.AddDomainEvent(domainEvent);

        entity.RemoveDomainEvent(domainEvent);

        entity.DomainEvents.Should().BeEmpty();
    }

    [Test]
    public void ClearDomainEvents_ShouldClearEvents()
    {
        var entity = new TestEntity();
        entity.AddDomainEvent(new TestDomainEvent());
        entity.AddDomainEvent(new TestDomainEvent());

        entity.ClearDomainEvents();

        entity.DomainEvents.Should().BeEmpty();
    }
}
