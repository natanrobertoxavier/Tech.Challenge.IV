﻿using Newtonsoft.Json;
using System.ComponentModel;

namespace Region.Query.Communication.Request.Enum;

[JsonConverter(typeof(DescriptionEnumConverter))]
public enum RegionRequestEnum
{
    [Description("Norte")]
    Norte,

    [Description("Nordeste")]
    Nordeste,

    [Description("CentroOeste")]
    CentroOeste,

    [Description("Sudeste")]
    Sudeste,

    [Description("Sul")]
    Sul
}
