using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScoringApp1.helperClasses
{
    public class RCSummary
    {
    
        private String _rvModel;
        public String RVModel
        {
            get
            {
                return _rvModel;
            }
            set
            {
                _rvModel = value;
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

    

    }
}