using System;
using HotChocolate.Language;

namespace HotChocolate.Types.NodaTime
{
    public abstract class IntBaseType<TBase> : ScalarType
    {
        public IntBaseType(string name)
            : base(name)
        {
        }

        protected abstract int DoFormat(TBase baseValue);

        protected abstract TBase DoParse(int str);

        public override Type ClrType { get; } = typeof(TBase);

        public override bool IsInstanceOfType(IValueNode? literal)
        {
            if (literal == null)
            {
                throw new ArgumentNullException(nameof(literal));
            }

            return literal is IntValueNode
                || literal is NullValueNode;
        }

        public override object? ParseLiteral(IValueNode? literal)
        {
            if (literal == null)
            {
                throw new ArgumentNullException(nameof(literal));
            }

            if (literal is IntValueNode intLiteral)
            {
                try
                {
                    return DoParse(int.Parse(intLiteral.Value));
                }
                catch (Exception e)
                {
                    throw new ScalarSerializationException(
                        $"Unable to deserialize integer to {this.Name}", e);
                }
            }

            if (literal is NullValueNode)
            {
                return null;
            }

            throw new ScalarSerializationException(
                $"The {this.Name} type can only parse integer literals.");
        }

        public override IValueNode ParseValue(object? value)
        {
            if (value == null)
            {
                return new NullValueNode(null);
            }

            if (value is TBase baseValue)
            {
                try
                {
                    var integer = DoFormat(baseValue);
                    return new IntValueNode(integer);
                }
                catch (Exception) { }
            }

            throw new ScalarSerializationException(
                "The specified value has to be an integer in order " +
                $"to be parsed by the {this.Name} type.");
        }

        public override object? Serialize(object? value)
        {
            if (value == null)
            {
                return null;
            }

            if (value is TBase baseValue)
            {
                try
                {
                    return DoFormat(baseValue);
                }
                catch (Exception) { }
            }

            throw new ScalarSerializationException(
                $"The specified value cannot be serialized by the {this.Name} type.");
        }

        public override bool TryDeserialize(object? serialized, out object? value)
        {
            if (serialized is null)
            {
                value = null;
                return true;
            }

            if (serialized is int integer)
            {
                try
                {
                    value = DoParse(integer);
                    return true;
                }
                catch (Exception)
                {
                }
            }

            value = null;
            return false;
        }
    }
}