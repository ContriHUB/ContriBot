using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Builder;
using System.Net;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Collections.Concurrent;

namespace ContriBot.Controllers
{
    [Route("api/notify")]
    [ApiController]
    public class NotifyController : ControllerBase
    {
        private IBotFrameworkHttpAdapter _externAdapter;

        private readonly ConcurrentDictionary<string, ConversationReference> _conversationReferences;
        public NotifyController(IBotFrameworkHttpAdapter adapter, IConfiguration configuration, ConcurrentDictionary<string, ConversationReference> conversationReferences)
        {
            _externAdapter = adapter;
            _conversationReferences = conversationReferences;
        }

        public async Task<IActionResult> Get()
        {

            var _userReference = ContriBotCustomAction.ContriBotCustomAction.conDict;

            foreach (var conversationReference in _userReference.Values)
            {
                await ((BotAdapter)_externAdapter).ContinueConversationAsync(string.Empty, conversationReference,
                    ExternalCallback, default(CancellationToken));
            }

            var result = new ContentResult();
            result.StatusCode = (int)HttpStatusCode.OK;
            result.ContentType = "text/html";
            result.Content = "<html> Hi I sent the message to the users </html>";
            //will add json response 

            return result;

        }

        private async Task ExternalCallback(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            await turnContext.SendActivityAsync(MessageFactory.Text("Hi Proactive!!"), cancellationToken);
        }
    }
    
}
