using System.Web.Http;
using Bmbsqd.JilMediaFormatter;
using Jil;

namespace Common
{
    public class FormatterConfig
    {
        private static readonly Options JilOptions = new Options(dateFormat: DateTimeFormat.ISO8601);

        /// <summary>
        ///     Configures formatters in <c>HttpConfiguration</c> to use Jil and
        ///     support JSON (de)serialization only.
        /// </summary>
        /// <param name="config">
        ///     The application <c>HttpConfiguration</c> object to configure
        ///     formatters on.
        /// </param>
        public static void Configure(HttpConfiguration config)
        {
            config.Formatters.Clear();
            config.Formatters.Add(new JilMediaTypeFormatter(JilOptions));
        }
    }
}