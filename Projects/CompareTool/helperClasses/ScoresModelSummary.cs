using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScoringApp1.helperClasses
{
  
 
    public class ScoresModelSummary
    {

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

        private Int32 _total;
        public Int32 Total
        {
            get
            {
                return _total;
            }
            set
            {
                _total = value;
            }
        }

        private Int32 _nbrOfChanges;
        public Int32 NbrOfChanges
        {
            get
            {
                return _nbrOfChanges;
            }
            set
            {
                _nbrOfChanges = value;
            }
        }

        private Decimal _percentChange;
        public Decimal PercentChange
        {
            get
            {
                return _percentChange;
            }
            set
            {
                _percentChange = value;
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

 

    }
}