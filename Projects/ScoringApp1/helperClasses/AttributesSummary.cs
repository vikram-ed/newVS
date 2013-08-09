using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScoringApp1.helperClasses
{
    public class AttributesSummary
    {
         
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

        private Int32 _diffCount;
        public Int32 DiffCount
        {
            get
            {
                return _diffCount;
            }
            set
            {
                _diffCount = value;
            }
        }

        private Decimal _diffCountPercent;
        public Decimal DiffCountPercent
        {
            get
            {
                return _diffCountPercent;
            }
            set
            {
                _diffCountPercent = value;
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

 


    }
}