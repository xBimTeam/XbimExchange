namespace Xsd2Code.Library.Helpers
{
    /// <summary>
    /// Result class represents result of execution
    /// <para><see cref="Success"/> flag represents result of the operation</para>
    /// <para><see cref="Messages"/> property contains a list of messages generated during execution</para>
    /// </summary>
    /// <remarks>
    /// Revision history:
    /// 
    ///     Created 2009-02-20 by Ruslan Urban
    /// 
    /// </remarks>
    public class Result
    {

        #region Result Default constructor

        /// <summary>
        /// Result class constructor
        /// </summary>
        public Result() : this(false)
        {}

        /// <summary>
        /// Result class constructor
        /// </summary>
        /// <param name="success">parameter value</param>
        public Result(bool success)
        {
            this.Success = success;
        }

        /// <summary>
        /// Result class constructor
        /// </summary>
        /// <param name="success">parameter value</param>
        /// <param name="message">parameter value</param>
        /// <param name="MessageType">parameter value</param>
        public Result(bool success, MessageType MessageType, string message) : this(success)
        {
            var messageItem = new Message(MessageType, message);
            this.Messages.Add(messageItem);
        }

        /// <summary>
        /// Result class constructor
        /// </summary>
        /// <param name="success">parameter value</param>
        /// <param name="messages">parameter value</param>
        public Result(bool success, MessageCollection messages) : this(success)
        {
            this.messages = messages;
        }

        #endregion

        #region Property : Success

        /// <summary>
        /// Success
        /// </summary>
        public bool Success { get; set; }

        #endregion

        #region Property : Messages

        /// <summary>
        /// Member field messages
        /// </summary>
        private MessageCollection messages;

        /// <summary>
        /// Messages
        /// </summary>
        public MessageCollection Messages
        {
            get
            {
                if (this.messages == null) 
                    this.messages = new MessageCollection();
                return this.messages;
            }
        }

        #endregion
    }

    /// <summary>
    /// Generic <see cref="Result"/> class represents result of execution 
    /// that returns an object of type <typeparamref name="TEntity"/> 
    /// in the property <see cref="Entity"/>
    /// </summary>
    /// <remarks>
    /// Revision history:
    /// 
    ///     Modified 2009-02-20 by Ruslan Urban
    /// 
    /// </remarks>
    /// <typeparam name="TEntity"></typeparam>
    public class Result<TEntity> : Result
    {
        ///<summary>
        /// Default constructor
        ///</summary>
        public Result()
        {}

        /// <summary>
        /// Result class constructor
        /// </summary>
        /// <param name="entity">parameter value</param>
        /// <param name="success">parameter value</param>
        public Result(TEntity entity, bool success)
            : this(entity, success, null)
        {}

        /// <summary>
        /// Result class constructor
        /// </summary>
        /// <param name="entity">parameter value</param>
        /// <param name="success">parameter value</param>
        /// <param name="messages">parameter value</param>
        public Result(TEntity entity, bool success, MessageCollection messages)
            : base(success, messages)
        {
            this.Entity = entity;
        }

        /// <summary>
        /// Result class constructor
        /// </summary>
        /// <param name="entity">parameter value</param>
        /// <param name="success">parameter value</param>
        /// <param name="message">parameter value</param>
        /// <param name="MessageType">parameter value</param>
        public Result(TEntity entity, bool success, string message, MessageType MessageType)
            : this(entity, success)
        {
            var messageItem = new Message(MessageType, message);
            this.Messages.Add(messageItem);
        }

        /// <summary>
        /// Resulting Entity
        /// </summary>
        public TEntity Entity { get; set; }
    }
}