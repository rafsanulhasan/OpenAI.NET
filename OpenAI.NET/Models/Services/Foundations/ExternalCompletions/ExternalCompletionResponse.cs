﻿// --------------------------------------------------------------- 
// Copyright (c) Coalition of the Good-Hearted Engineers 
// ---------------------------------------------------------------

using Newtonsoft.Json;

namespace OpenAI.NET.Models.Services.Foundations.ExternalCompletions
{
    public class ExternalCompletionResponse
    {
        [JsonProperty(propertyName: "id")]
        public string Id { get; set; }

        [JsonProperty(propertyName: "_object")]
        public string Object { get; set; }

        [JsonProperty(propertyName: "created")]
        public int Created { get; set; }

        [JsonProperty(propertyName: "model")]
        public string Model { get; set; }

        [JsonProperty(propertyName: "choices")]
        public ExternalChoice[] Choices { get; set; }

        [JsonProperty(propertyName: "usage")]
        public ExternalUsage Usage { get; set; }
    }
}
