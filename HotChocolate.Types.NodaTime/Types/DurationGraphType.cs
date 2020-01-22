using System;
using System.Globalization;
using HotChocolate.Language;
using NodaTime;
using NodaTime.Text;

namespace HotChocolate.Types.NodaTime
{
    public class DurationType : ScalarType
    {
        public DurationType()
            : base("Duration")
        {
        }

        public override Type ClrType { get; } = typeof(Duration);

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
                return DoParse(stringLiteral.Value);
            }

            if (literal is NullValueNode)
            {
                return null;
            }

            throw new ScalarSerializationException(
                "The Duration type can only parse string literals.");
        }

        public override IValueNode ParseValue(object? value)
        {
            if (value == null)
            {
                return new NullValueNode(null);
            }

            if (value is Duration duration)
            {
                var str = DoFormat(duration);
                return new StringValueNode(null, str, false);
            }

            throw new ScalarSerializationException(
                "The specified value has to be a string in order " +
                "to be parsed by the Duration type.");
        }

        public override object? Serialize(object? value)
        {
            if (value == null)
            {
                return null;
            }

            if (value is Duration duration)
            {
                return DoFormat(duration);
            }

            throw new ScalarSerializationException(
                "The specified value cannot be serialized by the Duration type.");
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
                value = DoParse(str);
                return true;
            }

            value = null;
            return false;
        }

        private static string DoFormat(Duration duration)
        {
            return DurationPattern.Roundtrip
                    .WithCulture(CultureInfo.InvariantCulture)
                    .Format(duration);
        }

        private static Duration DoParse(string str)
        {
            try
            {
                return DurationPattern.Roundtrip
                    .WithCulture(CultureInfo.InvariantCulture)
                    .Parse(str).GetValueOrThrow();
            }
            catch (Exception e)
            {
                throw new ScalarSerializationException("Unable to deserialize string to Duration", e);
            }
        }
    }
}