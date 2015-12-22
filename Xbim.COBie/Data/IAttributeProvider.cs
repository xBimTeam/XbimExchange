using Xbim.COBie.Rows;

namespace Xbim.COBie.Data
{
    /// <summary>
    /// Defines an interface used by COBieData classes, in order to allow population of the Attributes tab
    /// </summary>
    interface IAttributeProvider
    {
        void InitialiseAttributes(ref COBieSheet<COBieAttributeRow> attributeSheet);
    }
}
