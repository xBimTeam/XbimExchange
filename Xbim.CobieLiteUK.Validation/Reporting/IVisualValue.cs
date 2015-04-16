using Xbim.COBieLiteUK;

namespace Xbim.CobieLiteUK.Validation.Reporting
{
    /// <summary>
    /// determines the appearence of information when attention needs to be drawn.
    /// </summary>
    public enum VisualAttentionStyle
    {
        /// <summary>
        /// Undetermined or does not apply
        /// </summary>
        None,
        /// <summary>
        /// Information is not of concern
        /// </summary>
        Green,
        /// <summary>
        /// Information can be problematic, awareness needed.
        /// </summary>
        Amber,
        /// <summary>
        /// Information has problems.
        /// </summary>
        Red
    }

    interface IVisualValue
    {
        VisualAttentionStyle AttentionStyle { get; }
        AttributeValue VisualValue { get; }
    }
}
