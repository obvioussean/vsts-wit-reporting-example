using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace VSTS.WIT.Reporting.Common
{
    public class Configuration : Dictionary<string, object>
    {
        private string path;

        public Configuration(string path): base(Deserialize(path))
        {
            this.path = path;
        }

        public void Save()
        {
            var json = JsonConvert.SerializeObject(this, Formatting.Indented);
            using (var streamWriter = new StreamWriter(path))
            {
                streamWriter.Write(json);
            }
        }

        private static Dictionary<string, object> Deserialize(string path)
        {
            if (File.Exists(path))
            {
                using (var streamReader = new StreamReader(path))
                {
                    var json = streamReader.ReadToEnd();
                    return JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                }
            }
            else
            {
                File.CreateText(path);

                return new Dictionary<string, object>();
            }
        }
    }
}
