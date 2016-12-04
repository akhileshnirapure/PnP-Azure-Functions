#r "Microsoft.IdentityModel.Extensions.dll"
#r "Functions.Core.dll"



using System.Net;
using Functions.Core;


public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info($"C# HTTP trigger function processed a request. RequestUri={req.RequestUri}");

    // parse query parameter
    string name = req.GetQueryNameValuePairs()
        .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
        .Value;

    // Get request body
    dynamic data = await req.Content.ReadAsAsync<object>();

    // Set name to query string or body data
    name = name ?? data?.name;

    if(name != null)
    {
        var tenantAdminUri = new Uri(String.Format("https://{0}-admin.sharepoint.com", name));

        var tenantAdminRealm = TokenHelper.GetRealmFromTargetUrl(tenantAdminUri);
        
        // Creating new add-in only context for the operation
        string accessToken = Functions.Core.TokenHelper.GetAppOnlyAccessToken(
                                Functions.Core.TokenHelper.SharePointPrincipal,
                                tenantAdminUri.Authority,
                                tenantAdminRealm)
                             .AccessToken;

        name = accessToken;
    }
    

    return name == null
        ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a valid tenant name on the query string or in the request body")
        : req.CreateResponse(HttpStatusCode.OK, "Hello " + name);
}