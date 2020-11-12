
using System.IO;
using System.Linq;
using System.Reflection;

namespace Common.Infrastructure.Mails.Helpers
{
    public static class MailHelper
    {
        public static string RenderText(string key, params object[] args)
        {
            string template = ReadResourceFile("template.txt");

            //Inject content in the template
            string text = ReadResourceFile($"{key}.txt");
            text = AddArgs(text, args);

            return template.Replace("{{include-body}}", text);
        }

        private static string AddArgs(string text, object[] args)
        {
            args = args.Where(x => x != null).ToArray();
            for (int i = 0; i < args.Length; i++)
            {
                text = text.Replace("{" + i + "}", args[i].ToString());
            }
            return text;
        }

        public static string RenderHtml(string key, params object[] args)
        {
            string html = ReadResourceFile("template.html");

            //Inject content in the template
            string text = ReadResourceFile($"{key}.html");
            text = AddArgs(text, args);

            return html.Replace("{{include-body}}", text);
        }

        private static string ReadResourceFile(string key)
        {
            var assembly = Assembly.GetAssembly(typeof(MailHelper));
            var resourceName = $"{assembly.GetName().Name}.Mails.Helpers.TemplateMail.{key}";
            Stream stream = assembly.GetManifestResourceStream(resourceName);
            return new StreamReader(stream).ReadToEnd();
        }
    }
}
