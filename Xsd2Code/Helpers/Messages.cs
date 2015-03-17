#region Namespace references

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Text;
using Xsd2Code.Library.Helpers;

#endregion

namespace Xsd2Code.Library.Helpers
{
    /// <summary>
    /// <see cref="Message"/> collection
    /// </summary>
    /// <remarks>
    /// Revision history:
    ///     Created 2009-02-20 by Ruslan Urban
    /// </remarks>
    public class MessageCollection : Collection<Message>
    {
        /// <summary>
        /// Message collection class constructor
        /// </summary>
        /// <param name="messages"></param>
        public MessageCollection(params Message[] messages)
        {
            if (messages == null) return;
            foreach (var message in messages)
                this.Add(message);
        }

        /// <summary>
        /// Message collection class constructor
        /// </summary>
        /// <param name="messages"></param>
        public MessageCollection(IEnumerable<Message> messages)
        {
            if (messages == null) return;
            foreach (var message in messages)
                this.Add(message);
        }

        /// <summary>
        /// To string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var result = new StringBuilder();
            foreach (var message in this)
                result.AppendLine(message.ToString());
            return result.ToString();
        }


        /// <summary>
        /// Add a message
        /// </summary>
        /// <param name="messageType">parameter value</param>
        /// <param name="ruleName">parameter value</param>
        /// <param name="format">parameter value</param>
        /// <param name="args">parameter value</param>
        public void Add(MessageType messageType, string ruleName, string format, params object[] args)
        {
            this.Add(new Message(messageType, ruleName, format, args));
        }

        /// <summary>
        /// Add a message
        /// </summary>
        /// <param name="messageType">parameter value</param>
        /// <param name="format">parameter value</param>
        /// <param name="args">parameter value</param>
        public void Add(MessageType messageType, string format, params object[] args)
        {
            this.Add(new Message(messageType, format, args));
        }

        /// <summary>
        /// Add a message
        /// </summary>
        /// <param name="format">parameter value</param>
        /// <param name="args">parameter value</param>
        public void Add(string format, params object[] args)
        {
            this.Add(default(MessageType), format, args);
        }
    }
}