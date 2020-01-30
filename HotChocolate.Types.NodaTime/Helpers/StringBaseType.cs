using System;
using HotChocolate.Language;

namespace HotChocolate.Types.NodaTime
{
    public abstract class StringBaseType<TBase> : ScalarType
    {
        public StringBaseType(string name)
            : base(name)
        {
        }

        protected abstract string DoFormat(TBase baseValue);

        protected abstract TBase DoParse(string str);

        public override Type ClrType { get; } = typeof(TBase);

        public override bool IsInstanceOfType(IValueNode? literal)
        {
            if (literal == null)
            {
                throw new ArgumentNullException(nameof(literal));
            }

            return literal is StringValueNode
                || literal is NullValueNode;
        }

        public override object? ParseLiteral(IValueNode? literal)
        {
            if (literal == null)
            {
                throw new ArgumentNullException(nameof(literal));
            }

            if (literal is StringValueNode stringLiteral)
            {
                try
                {
                    return DoParse(stringLiteral.Value);
                }
                catch (Exception e)
                {
                    throw new ScalarSerializationException(
                        $"Unable to deserialize string to {this.Name}", e);
                }
            }

            if (literal is NullValueNode)
            {
                return null;
            }

            throw new ScalarSerializationException(
                $"The {this.Name} type can only parse string literals.");
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
                    var str = DoFormat(baseValue);
                    return new StringValueNode(null, str, false);
                }
                catch (Exception) { }
            }

            throw new ScalarSerializationException(
                "The specified value has to be a string in order " +
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

            if (serialized is string str)
            {
                try
                {
                    value = DoParse(str);
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