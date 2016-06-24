using Microsoft.Owin;
using Owin;
using System.Globalization;
using System.Threading;

[assembly: OwinStartupAttribute(typeof(EasyBot.Startup))]
namespace EasyBot
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            EnsureValidCulture();
            app.MapSignalR();
        }

        private static void EnsureValidCulture()
        {
            // The CultureInfo may leak across app domains which may cause hangs. The most prominent
            // case in SignalR are MapSignalR hangs when creating Performance Counters (#3414).
            // See https://github.com/SignalR/SignalR/issues/3414#issuecomment-152733194 for more details.
            var culture = CultureInfo.CurrentCulture;
            while (!culture.Equals(CultureInfo.InvariantCulture))
            {
                culture = culture.Parent;
            }

            if (ReferenceEquals(culture, CultureInfo.InvariantCulture))
            {
                return;
            }

            var thread = Thread.CurrentThread;
            thread.CurrentCulture = CultureInfo.GetCultureInfo(thread.CurrentCulture.Name);
            thread.CurrentUICulture = CultureInfo.GetCultureInfo(thread.CurrentUICulture.Name);
        }
    }
}
