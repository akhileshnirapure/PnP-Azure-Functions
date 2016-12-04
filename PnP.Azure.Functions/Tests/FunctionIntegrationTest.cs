using System;
using Functions.Core.Commands;
using Functions.Core.Commands.Handlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Tests
{
    [TestClass]
    public class FunctionIntegrationTest
    {
        [TestMethod]
        public void CreateSiteCollectionTest()
        {

            const string jsonPayload = @"{
                                          'Title': 'Test1',
                                          'Url': 'https://{tenantname}.sharepoint.com/teams/team1',
                                          'Template': 'STS#0',
                                          'PrimaryOwnerEmail': 'admin@{tenantname}.onmicrosoft.com',
                                          'SecondaryOwnerEmail': 'none',
                                          'StorageMaximumLevel': 100,
                                          'UserCodeMaximumLevel': 100,
                                          'TenantName': 'iaeadev'
                                        }";
            
            var command = JsonConvert.DeserializeObject<CreateSiteCollectionCommand>(jsonPayload);
            var handler = new SiteCollectionCreationCommandHandler();
            handler.Execute(command);

        }
    }
}
