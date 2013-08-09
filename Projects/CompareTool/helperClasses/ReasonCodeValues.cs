using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScoringApp1.helperClasses
{
    public class ReasonCodeValues:helperClassInterface
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

        private String _fieldName;
        public String FieldName
        {
            get
            {
                return _fieldName;
            }
            set
            {
                _fieldName = value;
            }
        }

        private String _reasonCode;
        public String ReasonCode
        {
            get
            {
                return _reasonCode;
            }
            set
            {
                _reasonCode = value;
            }
        }

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
        private Int32 _nbrOfPrevAccts;
        public Int32 NbrOfPrevAccts
        {
            get
            {
                return _nbrOfPrevAccts;
            }
            set
            {
                _nbrOfPrevAccts = value;
            }
        }
        private Int32 _nbrOfCurrentAccts;
        public Int32 NbrOfCurrentAccts
        {
            get
            {
                return _nbrOfCurrentAccts;
            }
            set
            {
                _nbrOfCurrentAccts = value;
            }
        }

        private Int32 _diffValue;
        public Int32 DiffValue
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
        private Int32 _absoluteDiff;
        public Int32 AbsoluteDiff
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
        private Decimal _proportion;
        public Decimal Proportion
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