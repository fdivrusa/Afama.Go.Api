using Afama.Go.Api.Domain.Common;
using FluentAssertions;
using NUnit.Framework;

namespace Afama.Go.Api.Domain.UnitTests.ValueObjects;

public class TestValueObjectTests
{
    private class TestValueObject : ValueObject
    {
        public string Name { get; }
        public int Amount { get; }

        public TestValueObject(string name, int amount)
        {
            Name = name;
            Amount = amount;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
            yield return Amount;
        }

        public static bool operator ==(TestValueObject? left, TestValueObject? right) => EqualOperator(left!, right!);
        public static bool operator !=(TestValueObject? left, TestValueObject? right) => NotEqualOperator(left!, right!);

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode() => base.GetHashCode();
    }

    [Test]
    public void ValueObjects_WithSameValues_ShouldBeEqual()
    {
        var first = new TestValueObject("Item", 1);
        var second = new TestValueObject("Item", 1);

        first.Should().Be(second);
        (first == second).Should().BeTrue();
        first.GetHashCode().Should().Be(second.GetHashCode());
    }

    [Test]
    public void ValueObjects_WithDifferentValues_ShouldNotBeEqual()
    {
        var first = new TestValueObject("Item", 1);
        var second = new TestValueObject("Item", 2);

        first.Should().NotBe(second);
        (first != second).Should().BeTrue();
    }
}
