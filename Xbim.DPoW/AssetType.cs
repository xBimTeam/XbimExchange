namespace Xbim.DPoW
{
    /// <summary>
    /// This class represents asset types in DPoW
    /// </summary>
    public class AssetType:DPoWObject
    {
        /// <summary>
        /// Variant of this asset type. This is typicaly something like 'A', 'B', 'C' or some other 
        /// distinquisher between different types of the same type
        /// </summary>
        public string Variant { get; set; }
    }
}
