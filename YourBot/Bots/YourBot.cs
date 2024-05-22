// Generated with YourBot .NET Template version v4.22.0

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Azure.AI.OpenAI;
using Azure;

namespace YourBot.Bots
{
    public class YourBot : ActivityHandler
    {
        private string OpenAIKey { get; set; } = "16b4fa9c3cc943199f7f3ca2dcf1ff65";
        private string OpenAIEndpoint { get; set; } = "https://aoai-dev-jabb-aoaighcopilotws.openai.azure.com/";
        
        public async Task<string> MyAsyncMethod(string text)
        {
            //generar un AzureKeyCredential con la llave de OpenAI
            var credential = new AzureKeyCredential(OpenAIKey);
            //inicializar el cliente de OpenAIClient con el endpoint de OpenAI y el AzureKeyCredential
            var client = new OpenAIClient(new Uri(OpenAIEndpoint), credential);
            //Inicializar las opciones de ChatCompletionsOptions de forma inline indicando el parámetro de DeploymentName de tipo gpt-35-turbo y Messages recibido de la variable text de tipo ChatRequestUserMessage. Siguiendo el formato new Class { Property = Value }
            var options = new ChatCompletionsOptions { DeploymentName="gpt-35-turbo", Messages = { new ChatRequestUserMessage (text ) },};
            //Invocar el método GetChatCompletionsAsync y regresar la respuesta de la llamada
            var response = await client.GetChatCompletionsAsync(options);
            //regresar el texto de la respuesta de Choices
            return response.Value.Choices[0].Message.Content;
        }
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            //Declarar una variable de tipo string llamada replyText y asignarle el valor de la respuesta de GetOpenAIResponseAsync con el parámetro de texto de la actividad recibida
            var replyText = await MyAsyncMethod(turnContext.Activity.Text);

            await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Hello and welcome!";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }
    }
}
