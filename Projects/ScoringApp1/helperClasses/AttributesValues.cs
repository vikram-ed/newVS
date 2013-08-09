using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScoringApp1.helperClasses
{
    public class AttributesValues:helperClassInterface
    {

        //private String _userName;
        //public String UserName
        //{
        //    get
        //    {
        //        return _userName;
        //    }
        //    set
        //    {
        //        _userName = value;
        //    }
        //}

        //private String _compareID;
        //public String CompareID
        //{
        //    get
        //    {
        //        return _compareID;
        //    }
        //    set
        //    {
        //        _compareID = value;
        //    }
        //}
        private String _accountNbr;
        public String AccountNbr
        {
            get
            {
                return _accountNbr;
            }
            set
            {
                _accountNbr = value;
            }
        }
        private String _attributeName;
        public String AttributeName
        {
            get
            {
                return _attributeName;
            }
            set
            {
                _attributeName = value;
            }
        }

        private String _previousValue;
        public String PreviousValue
        {
            get
            {
                return _previousValue;
            }
            set
            {
                _previousValue = value;
            }
        }
        private String _currentValue;
        public String CurrentValue
        {
            get
            {
                return _currentValue;
            }
            set
            {
                _currentValue = value;
            }
        }
        private Decimal _diffValue;
        public Decimal DiffValue
        {
            get
            {
                return _diffValue;
            }
            set
            {
                _diffValue = value;
            }
        }
        private Decimal _absoluteDiff;
        public Decimal AbsoluteDiff
        {
            get
            {
                return _absoluteDiff;
            }
            set
            {
                _absoluteDiff = value;
            }
        }
        private decimal _proportion;
        public decimal Proportion
        {
            get
            {
                return _proportion;
            }
            set
            {
                _proportion = value;
            }
        }
        private Int32 _diffFlag;
        public Int32 DiffFlag
        {
            get
            {
                return _diffFlag;
            }
            set
            {
                _diffFlag = value;
            }
        }
        private String _indicator;
        public String Indicator
        {
            get
            {
                return _indicator;
            }
            set
            {
                _indicator = value;
            }
        }
        private String _model;
        public String Model
        {
            get
            {
                return _model;
            }
            set
            {
                _model = value;
            }
        }
        private String _version;
        public String Version
        {
            get
            {
                return _version;
            }
            set
            {
                _version = value;
            }
        }
        private String _mode;
        public String Mode
        {
            get
            {
                return _mode;
            }
            set
            {
                _mode = value;
            }
        }

        private String _env;
        public String Env
        {
            get
            {
                return _env;
            }
            set
            {
                _env = value;
            }
        }

        private String _restriction;
        public String Restriction
        {
            get
            {
                return _restriction;
            }
            set
            {
                _restriction = value;
            }
        }

        private String _prevRunDate;
        public String PrevRunDate
        {
            get
            {
                return _prevRunDate;
            }
            set
            {
                _prevRunDate = value;
            }
        }

        private String _currentRunDate;
        public String CurrentRunDate
        {
            get
            {
                return _currentRunDate;
            }
            set
            {
                _currentRunDate = value;
            }
        }

        private String _customer;
        public String Customer
        {
            get
            {
                return _customer;
            }
            set
            {
                _customer = value;
            }
        }

        private String _loadDate;
        public String LoadDate
        {
            get
            {
                return _loadDate;
            }
            set
            {
                _loadDate = value;
            }
        }

 

    }
}