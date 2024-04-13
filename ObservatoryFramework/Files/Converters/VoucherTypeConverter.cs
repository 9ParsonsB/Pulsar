﻿using System.Text.Json;
using System.Text.Json.Serialization;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Converters;

class VoucherTypeConverter : JsonConverter<VoucherType>
{
    public override VoucherType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
            var voucher = reader.GetString();

            if (voucher.Length == 0)
                voucher = "None";

            var missionEffect = (VoucherType)Enum.Parse(typeof(VoucherType), voucher, true);

            return missionEffect;
        }

    public override void Write(Utf8JsonWriter writer, VoucherType value, JsonSerializerOptions options)
    {
            throw new NotImplementedException();
        }
}