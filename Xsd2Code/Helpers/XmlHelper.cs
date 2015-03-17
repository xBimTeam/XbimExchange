//-----------------------------------------------------------------------
// <copyright file="XmlHelper.cs" company="Xsd2Code">
//     copyright Pascal Cabanel.
// </copyright>
//-----------------------------------------------------------------------

namespace Xsd2Code.Library.Helpers
{
    /// <summary>
    /// Helper to find pseudo xml tag.
    /// </summary>
    public static class XmlHelper
    {
        /// <summary>
        /// Get value of pseudo xml tag
        /// </summary>
        /// <param name="xmlStream">xml data string</param>
        /// <param name="tag">Tag name in xml</param>
        /// <returns>return tag value</returns>
        public static string ExtractStrFromXML(this string xmlStream, string tag)
        {
            string upperData = xmlStream.ToUpper();
            tag = tag.ToUpper();
            int startpos = upperData.IndexOf("<" + tag + ">") + 2 + tag.Length;
            
            //Small Optimization as properties get longer; start searching from start position.
            int endpos = upperData.IndexOf("</" + tag + ">", startpos);
            int lenght = endpos - startpos;
            if (lenght > 0)
                return xmlStream.Substring(startpos, lenght);
            
            return string.Empty;
        }

        /// <summary>
        /// Insert tag in speudo xml string
        /// </summary>
        /// <param name="tag">tag name of pseudo xml</param>
        /// <param name="tagValue">value of tag</param>
        /// <returns>return pseudo xml string</returns>
        public static string InsertXMLFromStr(string tag, string tagValue)
        {
            return string.Format("<{0}>{1}</{0}>", tag, tagValue);
        }
    }
}