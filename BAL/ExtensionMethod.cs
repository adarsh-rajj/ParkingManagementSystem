using DAL.Context;
using Owin;

namespace BAL
{
    public static class ExtensionMethod
    {
        public static void ConfigureService(IAppBuilder app)
        {
            app.CreatePerOwinContext(ParkingDbContext.Create);
        }
    }
}
