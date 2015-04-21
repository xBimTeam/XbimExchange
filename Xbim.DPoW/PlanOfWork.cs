using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Xbim.DPoW
{
    /// <summary>
    /// Root object of this data schema
    /// </summary>
    public class PlanOfWork :DPoWAttributableObject
    {
        /// <summary>
        /// Classification systems defined in this plan of work
        /// </summary>
        public List<Classification> ClassificationSystems { get; set; }
        /// <summary>
        /// Project information invariant between project stages
        /// </summary>
        public Project Project { get; set; }
        /// <summary>
        /// Facility information invariant between project stages
        /// </summary>
        public Facility Facility { get; set; }
        /// <summary>
        /// Project stages representing time based development of requirements
        /// </summary>
        public List<ProjectStage> ProjectStages { get; set; }
        /// <summary>
        /// ID of client
        /// </summary>
        public Guid ClientContactId { get; set; }
        /// <summary>
        /// Client of this plan of work. Use 'ClientContactId' to set client. 
        /// </summary>
        [XmlIgnore][JsonIgnore]
        public Contact Client
        {
            get
            {
                return Contacts == null ? null : Contacts.FirstOrDefault(c => c.Id == ClientContactId);
            }
        }
        /// <summary>
        /// List of contacts for this plan of work. Contacts are referenced by their IDs on other places in the schema.
        /// </summary>
        public List<Contact> Contacts { get; set; }
        /// <summary>
        /// Unique ID
        /// </summary>
        public Guid Id { get; set;}
        /// <summary>
        /// Roles on this plan of work. Roles are used for example to assign responsibility when there is no specific person defined at particular point in time
        /// </summary>
        public List<Role> Roles { get; set; }

        /// <summary>
        /// The date when this plan of work fas created
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Initializes ID to new unique value
        /// </summary>
        public PlanOfWork()
        {
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// Creates Plan of work from serialized XML stream
        /// </summary>
        /// <param name="stream">input stream</param>
        /// <returns>Plan of work</returns>
        public static PlanOfWork OpenXml(Stream stream)
        {
            var serializer = new XmlSerializer(typeof (PlanOfWork));
            return (PlanOfWork)serializer.Deserialize(stream);
        }

        /// <summary>
        /// Creates Plan of work from serialized XML file
        /// </summary>
        /// <param name="path">Path to XML file</param>
        /// <returns>Plan of work</returns>
        public static PlanOfWork OpenXml(string path)
        {
            using (var stream = File.OpenRead(path))
            {
                var result = OpenXml(stream);
                stream.Close();
                return result;
            }
        }
        /// <summary>
        /// Creates plan of work from serialized JSON file
        /// </summary>
        /// <param name="path">Path to JSON file</param>
        /// <returns>Plan of work</returns>
        public static PlanOfWork OpenJson(string path) 
        {
            using (var file = System.IO.File.OpenRead(path))
            {
                var result = OpenJson(file);
                file.Close();
                return result;
            }
        }
        /// <summary>
        /// Creates plan of work from serialized JSON stream
        /// </summary>
        /// <param name="stream">stream</param>
        /// <returns>Plan of work</returns>
        public static PlanOfWork OpenJson(System.IO.Stream stream)
        {
            var reader = new System.IO.StreamReader(stream);

            var data = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<PlanOfWork>(data);
            
        }

        /// <summary>
        /// Saves Plan of work to file as JSON string
        /// </summary>
        /// <param name="path">Path to resulting file</param>
        public void SaveJson(string path)
        {
            using (var file = File.Create(path))
            {
                SaveJson(file);
            }
        }
        /// <summary>
        /// Saves Plan of work to stream as JSON string
        /// </summary>
        /// <param name="stream">output stream</param>
        public void SaveJson(Stream stream)
        {
            using (var writer = new StreamWriter(stream))
            {
                var data = JsonConvert.SerializeObject(this);
                writer.Write(data);
                writer.Close();
            }
        }
        /// <summary>
        /// Saves Plan of work to stream as XML
        /// </summary>
        /// <param name="stream">Output steam</param>
        public void SaveXml(Stream stream)
        {
            var serializer = new XmlSerializer(typeof (PlanOfWork));
            serializer.Serialize(stream, this);
        }
        /// <summary>
        /// Saves Plan of work to file as XML
        /// </summary>
        /// <param name="path">Path to file</param>
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
