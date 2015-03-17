// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Message.cs" company="Xsd2Code">
//   N/A
// </copyright>
// <summary>
//   Message class represents a message generated at a point of execution
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Xsd2Code.Library.Helpers
{
    using System;
    using System.Threading;

    /// <summary>
    /// Message class represents a message generated at a point of execution
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Name of rule
        /// </summary>
        private string ruleNameField = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// Message constructor with initializers
        /// </summary>
        /// <param name="messageType">Message type</param>
        /// <param name="text">Message text</param>
        public Message(MessageType messageType, string text)
            : this(messageType, string.Empty, text)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// Message constructor with initializers
        /// </summary>
        /// <param name="messageType">Message type</param>
        /// <param name="ruleName">Name of business rule</param>
        /// <param name="text">Message text</param>
        public Message(MessageType messageType, string ruleName, string text)
            : this()
        {
            this.MessageType = messageType;
            this.ruleNameField = ruleName;
            this.Text = text ?? string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// Message constructor with initializers
        /// </summary>
        /// <param name="messageType">Message type</param>
        /// <param name="ruleName">Name of business rule</param>
        /// <param name="format">Message text format string</param>
        /// <param name="args">Format string arguments</param>
        public Message(MessageType messageType, string ruleName, string format, params object[] args)
            : this(messageType, ruleName, string.Format(Thread.CurrentThread.CurrentCulture, format, args))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// Message constructor with initializers
        /// </summary>
        /// <param name="messageType">Message type</param>
        /// <param name="format">Message text</param>
        /// <param name="args">Arguments list</param>
        public Message(MessageType messageType, string format, params object[] args)
            : this(messageType, string.Empty, string.Format(Thread.CurrentThread.CurrentCulture, format, args))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// Message class constructor
        /// </summary>
        /// <param name="text">parameter value</param>
        public Message(string text)
            : this(default(MessageType), text)
        {
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="Message"/> class from being created. 
        /// </summary>
        private Message()
        {
            this.MessageSubtype = MessageSubtype.Unspecified;
        }

        /// <summary>
        /// Gets the name of the rule.
        /// </summary>
        public string RuleName
        {
            get { return this.ruleNameField; }
        }

        /// <summary>
        /// Gets or sets the type of the message.
        /// </summary>
        public MessageType MessageType { get; set; }

        /// <summary>
        /// Gets or sets the message subtype.
        /// </summary>
        public MessageSubtype MessageSubtype { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Represents message object in a form of a string
        /// </summary>
        /// <returns>Formatted Message</returns>
        public override string ToString()
        {
            return string.Format(
                                 Thread.CurrentThread.CurrentCulture,
                                 "{1}: {2}{0}\tSubType: {3}{0}{0}\tRule: {4}",
                                 Environment.NewLine,
                                 this.MessageType,
                                 this.Text,
                                 this.MessageSubtype,
                                 this.RuleName);
        }
    }
}