using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Xbim.DPoW
{
    public class PlanOfWork 
    {
        public List<Classification> ClassificationSystems { get; set; }
        public Project Project { get; set; }
        public Facility Facility { get; set; }
        public List<ProjectStage> ProjectStages { get; set; }
        public Guid ClientId { get; set; }
        public List<Contact> Contacts { get; set; }

        public Guid Id { get; set;}

        public PlanOfWork()
        {
            Contacts = new List<Contact>();
            ProjectStages = new List<ProjectStage>();
            ClassificationSystems = new List<Classification>();
            Id = Guid.NewGuid();
        }

        public static PlanOfWork OpenXml(Stream stream)
        {
            var serializer = new XmlSerializer(typeof (PlanOfWork));
            return (PlanOfWork)serializer.Deserialize(stream);
        }

        public static PlanOfWork OpenXml(string path)
        {
            using (var stream = File.OpenRead(path))
            {
                var result = OpenXml(stream);
                stream.Close();
                return result;
            }
        }

        public static PlanOfWork OpenJson(string path) 
        {
            using (var file = System.IO.File.OpenRead(path))
            {
                var result = OpenJson(file);
                file.Close();
                return result;
            }
        }

        public static PlanOfWork OpenJson(System.IO.Stream stream)
        {
            var reader = new System.IO.StreamReader(stream);

            var data = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<PlanOfWork>(data);
            
        }


        public void SaveJson(string path)
        {
            using (var file = File.Create(path))
            {
                SaveJson(file);
            }
        }

        public void SaveJson(Stream stream)
        {
            using (var writer = new StreamWriter(stream))
            {
                var data = JsonConvert.SerializeObject(this);
                writer.Write(data);
                writer.Close();
            }
        }

        public void SaveXml(Stream stream)
        {
            var serializer = new XmlSerializer(typeof (PlanOfWork));
            serializer.Serialize(stream, this);
        }

        public void SaveXml(string path)
        {
            using (var file = File.Create(path))
            {
                SaveXml(file);
                file.Close();
            }
        }
    }

}
