using System;
using System.IO;
using System.Text;
using Xbim.COBieLiteUK;

namespace Xbim.COBie.EqCompare
{
    public class CompareLogger
    {
        /// <summary>
        /// Hold the TextWriter to log too
        /// </summary>
        private TextWriter logger
        { get; set; }

        /// <summary>
        /// Did we log anything
        /// </summary>
        public bool IsLogging
        {
            get
            {
                return (logger != null);
            }
        }

        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tw"></param>
        public CompareLogger(TextWriter tw)
        {
            logger = tw;
            
        }

        /// <summary>
        /// Write the text to the TextWriter
        /// </summary>
        /// <param name="logMe">string to write</param>
        public void WriteLine(string logMe)
        {
            if (logger != null)
            {
                logger.WriteLine(logMe);
            }          
        }

        public void WriteLine(Facility to, Facility from)
        {
            if (logger != null)
            {
                StringBuilder sb = new StringBuilder();
                logger.WriteLine();
                if (from != null)
                {
                    sb.Append("Facility(");
                    sb.Append(from.Name);
                    sb.Append(":");
                    sb.Append(from.ExternalId);
                    sb.Append(") ");
                }
                sb.Append(" =======================INTO====================>>>> ");
                if (to != null)
                {
                    sb.Append("Facility(");
                    sb.Append(to.Name);
                    sb.Append(":");
                    sb.Append(to.ExternalId);
                    sb.Append(") ");
                }

                

                
                logger.WriteLine(sb.ToString());
                logger.WriteLine();
            }
        }

        /// <summary>
        /// Write the text to the TextWriter
        /// </summary>
        /// <param name="logMe">string to write</param>
        public void WriteLine(CobieObject rootObj, CobieObject parentObj, Type mergeType, int duplicateNo, int mergedNo, int depthIndicator)
        {
            if (logger != null )
            {
                string mergeindicator = (mergedNo > 0) ? "*" : "";
                StringBuilder sb = new StringBuilder();
                if (mergedNo > 0)
                    sb.Append("*");
                else
                    sb.Append(" ");

                if (depthIndicator > 0)
                    sb.Append('\t', depthIndicator);
                
                if (rootObj != null && (depthIndicator == 0))
                {
                    sb.Append(rootObj.GetType().Name);
                    sb.Append("(");
                    sb.Append(rootObj.Name);
                    sb.Append(":");
                    sb.Append(rootObj.ExternalId);
                    sb.Append("): ");
                }

                if (parentObj != null) 
                {
                    sb.Append(parentObj.GetType().Name);
                    sb.Append("(");
                    sb.Append(parentObj.Name);
                    sb.Append(":");
                    sb.Append(parentObj.ExternalId);
                    sb.Append("): ");
                }
                
                sb.Append(mergeType.Name);
                sb.Append(" ");
                sb.Append(duplicateNo);
                sb.Append(" duplicate, ");
                sb.Append(mergedNo);
                sb.Append(" merged.");
                logger.WriteLine( sb.ToString());
            }
        }
    }
}
