using AdaptiveExpressions.Properties;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace ContriBotCustomAction
{
    //Custom Action For ContriBot 
    public class ContriBotCustomAction : Dialog
    {
        public ContriBotCustomAction([CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0) : base()
        {
            RegisterSourceLocation(sourceFilePath, sourceLineNumber);
        }

        [JsonProperty("$Kind")]
        public const string Kind = "ContriBotCustomAction";

        [JsonProperty("resultProperty")]
        public StringExpression ResultProperty { get; set; }

        public static readonly ConcurrentDictionary<string,ConversationReference> conDict= new 
            ConcurrentDictionary<string,ConversationReference>();

        private readonly ConcurrentDictionary<string, ConversationReference> _conversationReferences;
        public ContriBotCustomAction(ConcurrentDictionary<string, ConversationReference> conversationReferences)
        {
            _conversationReferences = conversationReferences;
        }


        public override Task<DialogTurnResult> BeginDialogAsync(DialogContext dc, object options = null,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var result = AddConversationReference(dc.Context.Activity);

            if (ResultProperty != null)
            {
                dc.State.SetValue(this.ResultProperty.GetValue(dc.State), result);
            }

            return dc.EndDialogAsync(result: result, cancellationToken: cancellationToken);
        }

        private string AddConversationReference(Activity activity)
        {
            var conversationReference = activity.GetConversationReference();

            var userInfo = conDict.AddOrUpdate(conversationReference.User.Id, conversationReference,
                (key, value) => conversationReference);

            if (userInfo != null)
                return userInfo.User.Name;
            else
                return "Invalid Information";
        }
    }
}
