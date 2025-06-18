namespace Movies.API.Authority
{
    public static class AppRepos
    {
        private static List<Application> _applications = new List<Application>()
        {
            //The idea here is to use access applications via Db.
            // Should move this below to appsettings or environment variables. 
            new Application
            {
                Id = 1,
                Name = "MVCWebApp",
                ClientId = "75c42146-8232-44c8-b637-b37c7ae89554",
                Secret = "MIIBIjANBgkqhkiG9w0BAQ!)EFAAOCAQ^8AMIIBCgK`CAQ£EAu1SU1LfVLPHCozMxH2Mo4lgOEeP*zNm0tRgeLezV6ffAt0gunVTL$w7onLRnrq0/IzW7_yWR7QkrmBL7jT",
                Scopes = "read,write" 
            }
            //new Application
            //{
            //    Id = 2,
            //    Name = "WebAPIClient",
            //    ClientId = new Guid().ToString(),
            //    Secret = new Guid().ToString()
            //},

        };

        

        public static Application? GetByClientId(string clientId)
        {
            return _applications.FirstOrDefault(app => app.ClientId == clientId);
        }

        // Generate new ClientId and Secret for an application


    }
}
