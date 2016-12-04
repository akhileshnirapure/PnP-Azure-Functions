using System;
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;

namespace Functions.Core.Commands.Handlers
{
    public class SiteCollectionCreationCommandHandler : ICommandHandler<CreateSiteCollectionCommand, string>
    {
        public string Execute(CreateSiteCollectionCommand command)
        {
            var tenantAdminUri = new Uri($"https://{command.TenantName}-admin.sharepoint.com");

            var tenantAdminRealm = TokenHelper.GetRealmFromTargetUrl(tenantAdminUri);

            // Creating new add-in only context for the operation
            string accessToken = TokenHelper.GetAppOnlyAccessToken(TokenHelper.SharePointPrincipal,
                                    tenantAdminUri.Authority,
                                    tenantAdminRealm)
                                 .AccessToken;

             
            
            using (var adminContext = TokenHelper.GetClientContextWithAccessToken(tenantAdminUri.ToString(), accessToken))
            {
                var tenant = new Tenant(adminContext);
                var properties = new SiteCreationProperties
                {
                    Url = command.Url,
                    Owner = command.PrimaryOwnerEmail,
                    Title = command.Title,
                    Template = command.Template,
                    StorageMaximumLevel = command.StorageMaximumLevel,
                    UserCodeMaximumLevel = command.UserCodeMaximumLevel
                };

                //start the SPO operation to create the site
                SpoOperation op = tenant.CreateSite(properties);
                adminContext.Load(tenant);
                adminContext.Load(op, i => i.IsComplete);
                adminContext.ExecuteQueryRetry();

                //check if site creation operation is complete
                while (!op.IsComplete)
                {
                    //wait 30seconds and try again
                    System.Threading.Thread.Sleep(30000);
                    op.RefreshLoad();
                    adminContext.ExecuteQueryRetry();
                }
            }

            return command.Url;

        }
    }
}