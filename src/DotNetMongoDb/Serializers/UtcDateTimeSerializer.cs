using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace DotNetMongoDb.Serializers;

public class UtcDateTimeSerializer : SerializerBase<DateTime>
{
    public override DateTime Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var type = context.Reader.GetCurrentBsonType();

        switch (type)
        {
            case BsonType.Array:
                context.Reader.ReadStartArray();
                var ticks = context.Reader.ReadInt64();
                var offsetTicks = context.Reader.ReadInt32();
                context.Reader.ReadEndArray();
                var offset = new TimeSpan(offsetTicks);
                var dateTimeOffset = new DateTimeOffset(ticks, offset);
                return dateTimeOffset.UtcDateTime;
            default:
                throw new NotSupportedException($"Cannot convert a {type} to a {nameof(DateTime)}");
        }
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DateTime value)
    {
        var utcDateTime = value.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(value, DateTimeKind.Utc)
            : value.ToUniversalTime();

        var dateTimeOffset = new DateTimeOffset(DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc), TimeSpan.Zero);
        BsonSerializer.Serialize(context.Writer, dateTimeOffset);
    }
}