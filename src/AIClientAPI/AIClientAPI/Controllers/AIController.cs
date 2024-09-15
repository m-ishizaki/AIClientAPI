namespace RKSoftware.AIClientAPI.Controllers;

[Microsoft.AspNetCore.Mvc.ApiController]
[Microsoft.AspNetCore.Mvc.Route("[controller]")]
public class AIController : Microsoft.AspNetCore.Mvc.ControllerBase
{
    public record PostParameter(string UserMessage) { }
    public record Result(string AIMessage) { }

    // API Key 等。ここでは環境変数から取得。コードに書かなくてよいように
    // DeploymentName は環境変数にしなくてもよいとは思いますが、コピペで検証しやすいように環境変数にしています
    static readonly string openAIEndpoint = Environment.GetEnvironmentVariable("OpenAIEndpoint")!;
    static readonly string openAIAPIKey = Environment.GetEnvironmentVariable("OpenAIAPIKey")!;
    static readonly string openAIDeploymentName = Environment.GetEnvironmentVariable("OpenAIDeploymentName")!;
    // システムプロンプトの文言を用意しておきます
    static readonly string systemPrompt = "ユーザーからのメッセージを要約してください";
    // エンドポイント、キー、デプロイメント名を使って AI のクライアントを生成しておきます
    static readonly Azure.AI.OpenAI.AzureOpenAIClient client = new Azure.AI.OpenAI.AzureOpenAIClient(new Uri(openAIEndpoint), new Azure.AzureKeyCredential(openAIAPIKey));

    [Microsoft.AspNetCore.Mvc.HttpPost()]
    public async Task<Result> Post(PostParameter p)
    {
        // AI と会話する会話履歴オブジェクトを生成。システムプロンプトとユーザーからの質問を保持
        List<OpenAI.Chat.ChatMessage> chatHistory = new() { OpenAI.Chat.ChatMessage.CreateSystemMessage(systemPrompt), OpenAI.Chat.ChatMessage.CreateUserMessage(p.UserMessage) };
        // AI のクライアントを生成し回答を得る
        var aiMessage = (await client.GetChatClient(openAIDeploymentName).CompleteChatAsync(chatHistory)).Value.Content.Last().Text;
        return new Result(aiMessage);
    }
}

[Microsoft.AspNetCore.Mvc.ApiController]
[Microsoft.AspNetCore.Mvc.Route("[controller]")]
public class AIScriptController : Microsoft.AspNetCore.Mvc.ControllerBase
{
    [Microsoft.AspNetCore.Mvc.HttpGet()]
    public string Get() => """
        function addAIClientAPI(idButton, idTextarea) {
            document.getElementById(idButton).setAttribute('onclick', `aiClientAPI('${idTextarea}');return false;`);
        }
        function aiClientAPI(id) {
            alert('要約します');
            var elm = document.getElementById(id);
            fetch('./AI', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ userMessage: elm.value })
            })
            .then(response => response.json())
            .then(data => {
                elm.value = data.aiMessage;
                alert('要約しました');
            })
            .catch(error => {
                alert(error);
            });
        }
    """;
}
