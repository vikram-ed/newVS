using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScoringApp1.helperClasses
{
    
    public class ScoresAttributesSummary:helperClassInterface
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

        private String _modelName;
        public String ModelName
        {
            get
            {
                return _modelName;
            }
            set
            {
                _modelName = value;
            }
        }

        private int _previousValue;
        public int PreviousValue
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

        private int _currentValue;
        public int CurrentValue
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
        private int _differenceValue;
        public int DifferenceValue
        {
            get
            {
                return _differenceValue;
            }
            set
            {
                _differenceValue = value;
            }
        }
        //private String _differenceValueRange;
        //public String DifferenceValueRange
        //{
        //    get
        //    {
        //        return _differenceValueRange;
        //    }
        //    set
        //    {
        //        _differenceValueRange = value;
        //    }
        //}
        private int _absoluteDiff;
        public int AbsoluteDiff
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
        private decimal _diffValuePercent;
        public decimal DiffValuePercent
        {
            get
            {
                return _diffValuePercent;
            }
            set
            {
                _diffValuePercent = value;
            }
        }

        private int _differenceFlag;
        public int DifferenceFlag
        {
            get
            {
                return _differenceFlag;
            }
            set
            {
                _differenceFlag = value;
            }
        }

        private String _prevRange;
        public String PrevRange
        {
            get
            {
                return _prevRange;
            }
            set
            {
                _prevRange = value;
            }
        }
        private String _currentRange;
        public String CurrentRange
        {
            get
            {
                return _currentRange;
            }
            set
            {
                _currentRange = value;
            }
        }
        private int _rangeDiffFlag;
        public int RangeDiffFlag
        {
            get
            {
                return _rangeDiffFlag;
            }
            set
            {
                _rangeDiffFlag = value;
            }
        }
        //private String _model;
        //public String Model
        //{
        //    get
        //    {
        //        return _model;
        //    }
        //    set
        //    {
        //        _model = value;
        //    }
        //}
        //private String _version;
        //public String Version
        //{
        //    get
        //    {
        //        return _version;
        //    }
        //    set
        //    {
        //        _version = value;
        //    }
        //}

        //private String _mode;
        //public String Mode
        //{
        //    get
        //    {
        //        return _mode;
        //    }
        //    set
        //    {
        //        _mode = value;
        //    }
        //}

        //private String _env;
        //public String Env
        //{
        //    get
        //    {
        //        return _env;
        //    }
        //    set
        //    {
        //        _env = value;
        //    }
        //}

        //private String _restriction;
        //public String Restriction
        //{
        //    get
        //    {
        //        return _restriction;
        //    }
        //    set
        //    {
        //        _restriction = value;
        //    }
        //}

        //private String _prevRunDate;
        //public String PrevRunDate
        //{
        //    get
        //    {
        //        return _prevRunDate;
        //    }
        //    set
        //    {
        //        _prevRunDate = value;
        //    }
        //}

        //private String _currentRunDate;
        //public String CurrentRunDate
        //{
        //    get
        //    {
        //        return _currentRunDate;
        //    }
        //    set
        //    {
        //        _currentRunDate = value;
        //    }
        //}

        //private String _customer;
        //public String Customer
        //{
        //    get
        //    {
        //        return _customer;
        //    }
        //    set
        //    {
        //        _customer = value;
        //    }
        //}

        //private String _loadDate;
        //public String LoadDate
        //{
        //    get
        //    {
        //        return _loadDate;
        //    }
        //    set
        //    {
        //        _loadDate = value;
        //    }
        //}


        private Int32 _attrChangeCount;
        public Int32 AttrChangeCount
        {
            get
            {
                return _attrChangeCount;
            }
            set
            {
                _attrChangeCount = value;
            }
        }

        

    }
}