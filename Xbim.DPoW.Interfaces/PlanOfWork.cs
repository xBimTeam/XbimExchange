using System.Collections.Generic;
using Newtonsoft.Json;

namespace Xbim.DPoW.Interfaces
{
    public class PlanOfWork 
    {
        public List<Classification> ClassificationSystem { get; set; }
        public Project Project { get; set; }
        public Facility Facility { get; set; }
        public List<ProjectStage> ProjectStages { get; set; }
        public Contact Client { get; set; }
        public List<Contact> Contacts { get; set; }

        public PlanOfWork()
        {
            Contacts = new List<Contact>();
            ProjectStages = new List<ProjectStage>();
            ClassificationSystem = new List<Classification>();
        }

        public static PlanOfWork Open(string path) 
        {
            using (var file = System.IO.File.OpenRead(path))
            {
                var result = Open(file);
                file.Close();
                return result;
            }
        }

        public static PlanOfWork Open(System.IO.Stream stream)
        {
            var reader = new System.IO.StreamReader(stream);

            var data = reader.ReadToEnd();
            var settings = new JsonSerializerSettings()
            {
            };
            settings.Converters.Add(new Xbim.DPoW.Interfaces.Converters.DPoWObjectConverter());
            return JsonConvert.DeserializeObject<PlanOfWork>(data, settings);
            
        }

        public void Save(string path)
        {
            using (var file = System.IO.File.Create(path))
            {
                Save(file);
                file.Close();
            }
        }

        public void Save(System.IO.Stream stream)
        {
            var writer = new System.IO.StreamWriter(stream);
            var data = JsonConvert.SerializeObject(this);
            writer.Write(data);
        }
    }

}
