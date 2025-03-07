﻿// --------------------------------------------------------------- 
// Copyright (c) Coalition of the Good-Hearted Engineers 
// ---------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using OpenAI.NET.Brokers.OpenAIs;
using OpenAI.NET.Models.Services.Foundations.Completions;
using OpenAI.NET.Models.Services.Foundations.ExternalCompletions;

namespace OpenAI.NET.Services.Foundations.Completions
{
    internal partial class CompletionService : ICompletionService
    {
        private readonly IOpenAIBroker openAiBroker;

        public CompletionService(IOpenAIBroker openAiBroker) =>
            this.openAiBroker = openAiBroker;

        public ValueTask<Completion> PromptCompletionAsync(Completion completion) =>
        TryCatch(async () =>
        {
            ValidateCompletion(completion);

            ExternalCompletionResponse externalCompletionResponse =
                await PostCompletionRequestAsync(completion);

            return ConvertToCompletion(completion, externalCompletionResponse);
        });

        private async Task<ExternalCompletionResponse> PostCompletionRequestAsync(Completion completion)
        {
            ExternalCompletionRequest externalCompletionRequest =
                ConvertToCompletionRequest(completion);

            ExternalCompletionResponse externalCompletionResponse =
                await this.openAiBroker.PostCompletionRequestAsync(externalCompletionRequest);

            return externalCompletionResponse;
        }

        private static ExternalCompletionRequest ConvertToCompletionRequest(Completion completion)
        {
            return new ExternalCompletionRequest
            {
                Model = completion.Request.Model,
                BestOf = completion.Request.BestOf,
                CompletionsPerPrompt = completion.Request.CompletionsPerPrompt,
                Echo = completion.Request.Echo,
                FrequencyPenalty = completion.Request.FrequencyPenalty,
                LogitBias = completion.Request.LogitBias,
                LogProbabilities = completion.Request.LogProbabilities,
                MaxTokens = completion.Request.MaxTokens,
                PresencePenalty = completion.Request.PresencePenalty,
                ProbabilityMass = completion.Request.ProbabilityMass,
                Prompt = completion.Request.Prompt,
                Stop = completion.Request.Stop,
                Stream = completion.Request.Stream,
                Suffix = completion.Request.Suffix,
                Temperature = completion.Request.Temperature,
                User = completion.Request.User
            };
        }

        private static Completion ConvertToCompletion(
            Completion completion,
            ExternalCompletionResponse externalCompletionResponse)
        {
            completion.Response = new CompletionResponse
            {
                Model = externalCompletionResponse.Model,

                Choices = externalCompletionResponse.Choices.Select(externalChoice => new Choice
                {
                    FinishReason = externalChoice.FinishReason,
                    Index = externalChoice.Index,
                    LogProbabilities = externalChoice.LogProbabilities,
                    Text = externalChoice.Text
                }).ToArray(),

                Created = externalCompletionResponse.Created,
                Id = externalCompletionResponse.Id,

                Usage = new Usage
                {
                    CompletionTokens = externalCompletionResponse.Usage.CompletionTokens,
                    PromptTokens = externalCompletionResponse.Usage.PromptTokens,
                    TotalTokens = externalCompletionResponse.Usage.TotalTokens
                },

                Object = externalCompletionResponse.Object
            };

            return completion;
        }
    }
}
