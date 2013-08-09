using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScoringApp1.helperClasses
{
    public class CompareIDFields
    {
        private Int32 _serial;
        public Int32 Serial
        {
            get
            {
                return _serial;
            }
            set
            {
                _serial = value;
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
        private String _dates;
        public String Dates
        {
            get
            {
                return _dates;
            }
            set
            {
                _dates = value;
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
        public String Environment
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

    }
}