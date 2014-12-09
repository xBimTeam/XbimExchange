using System;

namespace Xbim.COBie
{

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class COBieAttributes : Attribute
    {
        private readonly COBieAttributeState _state;
        private int _maxLength;
        private COBieAllowedType _allowedType;
        private readonly int _order;
        private string _columnName;
        private COBieKeyType _keyType;
        private string _referenceColumnName;
        private COBieCardinality _cardinality;

        public COBieAttributeState State
        {
            get { return _state; }
        }

        public string ColumnName
        {
            get { return _columnName; }
        }

        public int MaxLength
        {
            get { return _maxLength; }
        }

        public COBieAllowedType AllowedType
        {
            get { return _allowedType; }
        }

        public COBieKeyType KeyType
        {
            get { return _keyType; }
        }

        public int Order
        {
            get { return _order; }
        }

        
        public COBieAttributes(COBieAttributeState state)
        {
            _state = state;
        }

        public string ReferenceColumnName
        {
            get { return _referenceColumnName; }
        }

        public COBieCardinality Cardinality
        {
            get { return _cardinality; }
        }

        public bool AllowedMultipleValues
        {
            get { return Cardinality == COBieCardinality.ManyToMany; }
        }

        public COBieAttributes(int order, COBieKeyType keyType, string referenceColumnName, COBieAttributeState state, string columnName, int maxLength, COBieAllowedType allowedType, COBieCardinality cardinality = COBieCardinality.OneToMany)
        {
            _state = state;
            _maxLength = maxLength;
            _allowedType = allowedType;
            _order = order;
            _columnName = columnName;
            _keyType = keyType;
            _referenceColumnName = referenceColumnName;
            _cardinality = cardinality;
        }
    }
}
