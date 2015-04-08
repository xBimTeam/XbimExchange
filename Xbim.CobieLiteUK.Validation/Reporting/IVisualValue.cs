using Xbim.COBieLiteUK;

namespace Xbim.CobieLiteUK.Validation.Reporting
{
    public enum VisualAttentionStyle
    {
        None,
        Green,
        Amber,
        Red
    }

    interface IVisualValue
    {
        VisualAttentionStyle AttentionStyle { get; }
        AttributeValue VisualValue { get; }
    }
}
