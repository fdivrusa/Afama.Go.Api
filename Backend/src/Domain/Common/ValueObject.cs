namespace Afama.Go.Api.Domain.Common
{
    // Learn more: https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/microservice-ddd-cqrs-patterns/implement-value-objects
    public abstract class ValueObject
    {
        protected static bool EqualOperator(ValueObject? left, ValueObject? right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null || right is null) return false;
            return left.Equals(right);                      
        }

        protected static bool NotEqualOperator(ValueObject? left, ValueObject? right)
            => !EqualOperator(left, right);

        protected abstract IEnumerable<object?> GetEqualityComponents();

        public sealed override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (obj is null || obj.GetType() != GetType()) return false;

            var other = (ValueObject)obj;
            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        public sealed override int GetHashCode()
        {
            var hash = new HashCode();
            foreach (var component in GetEqualityComponents())
            {
                hash.Add(component);
            }
            return hash.ToHashCode();
        }
    }
}
