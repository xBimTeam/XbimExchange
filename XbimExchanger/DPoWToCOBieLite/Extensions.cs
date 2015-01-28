using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.COBieLite;

namespace XbimExchanger.DPoWToCOBieLite
{
    public static class Extensions
    {

        public static void Add(this AttributeCollectionType act, string name, string description, string value, string pset = null)
        {
            act.Add(new AttributeType()
                    {
                        AttributeName = name,
                        AttributeDescription = description,
                        propertySetName = pset,
                        AttributeValue = new AttributeValueType() { Item = new AttributeStringValueType() { StringValue = value } }
                    });
        }

        public static void Add(this AttributeCollectionType act, string name, string description, int value, string pset = null)
        {
            act.Add(new AttributeType()
            {
                AttributeName = name,
                AttributeDescription = description,
                propertySetName = pset,
                AttributeValue = new AttributeValueType() { Item = new AttributeIntegerValueType() { IntegerValue = value } }
            });
        }

        public static void Add(this AttributeCollectionType act, string name, string description, double value, string pset = null)
        {
            act.Add(new AttributeType()
            {
                AttributeName = name,
                AttributeDescription = description,
                propertySetName = pset,
                AttributeValue = new AttributeValueType() { Item = new AttributeDecimalValueType() { DecimalValue = value , DecimalValueSpecified = true} }
            });
        }

        public static void Add(this AttributeCollectionType act, string name, string description, bool value, string pset = null)
        {
            act.Add(new AttributeType()
            {
                AttributeName = name,
                AttributeDescription = description,
                propertySetName = pset,
                AttributeValue = new AttributeValueType() { Item = new BooleanValueType() { BooleanValue = value , BooleanValueSpecified = true} }
            });
        }

        public static void Add(this AttributeCollectionType act, string name, string description)
        {
            act.Add(new AttributeType()
            {
                AttributeName = name,
                AttributeDescription = description,
            });
        }


        public static void Add(this IssueCollectionType self, IssueType issue)
        {
            self.Add(new[] { issue });
        }

        public static void Add(this IssueCollectionType self, IEnumerable<IssueType> issues)
        {
            var exist = self.Issue;
            if (exist == null)
                self.Issue = issues.ToArray();
            else
            {
                var newIssues = exist.ToList();
                newIssues.AddRange(issues);
                self.Issue = newIssues.ToArray();
            }
        }

        public static void Add(this ZoneCollectionType self, ZoneType zone)
        {
            self.Add(new[] { zone });
        }

        public static void Add(this ZoneCollectionType self, IEnumerable<ZoneType> zones)
        {
            var exist = self.Zone;
            if (exist == null)
                self.Zone = zones.ToArray();
            else
            {
                var newZones = exist.ToList();
                newZones.AddRange(zones);
                self.Zone = newZones.ToArray();
            }
        }

        public static void Add(this AssetTypeCollectionType self, AssetTypeInfoType assetTypes)
        {
            self.Add(new[] { assetTypes });
        }

        public static void Add(this AssetTypeCollectionType self, IEnumerable<AssetTypeInfoType> assetTypes)
        {
            var exist = self.AssetType;
            if (exist == null)
                self.AssetType = assetTypes.ToArray();
            else
            {
                var newZones = exist.ToList();
                newZones.AddRange(assetTypes);
                self.AssetType = newZones.ToArray();
            }
        }
    }
}
