#r "Functions.Core.dll"


using System.Net;
using Functions.Core.Commands;
using Functions.Core.Commands.Handlers;


public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info($"C# HTTP trigger function processed a request. RequestUri={req.RequestUri}");

    var commandPayLoad = await req.Content.ReadAsStringAsync();

    log.Info($"Body data : {commandPayLoad}");

    var creationEnabled = bool.Parse(System.Environment.GetEnvironmentVariable("Enabled", EnvironmentVariableTarget.Process));

    log.Info($"Site Creation is : {creationEnabled}");

    CreateSiteCollectionCommand command = await req.Content.ReadAsAsync<CreateSiteCollectionCommand>();

    if (creationEnabled && command != null)
    {
        //TODO:Validate Command
        var handler = new SiteCollectionCreationCommandHandler();
        handler.Execute(command);
    }


    return command == null
       ? req.CreateResponse(HttpStatusCode.BadRequest, "Please provide a valid command in body")
       : req.CreateResponse(HttpStatusCode.OK, "Site Collection created : " + command.Url);

}
