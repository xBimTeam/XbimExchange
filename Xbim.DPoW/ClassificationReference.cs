using System;
namespace Xbim.DPoW
{
    public class ClassificationReference 
    {
        public string Sort { get; set; }
        public string ClassificationCode { get; set; }
        public string ClassificationDescription { get; set; }

        public Guid Id { get; set; }

        public ClassificationReference()
        {
            Id = Guid.NewGuid();
        }
    }
}
