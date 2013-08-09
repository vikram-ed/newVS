using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScoringApp1.helperClasses
{
 
    public class ScoresSummary:helperClassInterface
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

        private String _scoreRange;
        public String ScoreRange
        {
            get
            {
                return _scoreRange;
            }
            set
            {
                _scoreRange = value;
            }
        }

        private Int32 _prevCount;
        public Int32 PrevCount
        {
            get
            {
                return _prevCount;
            }
            set
            {
                _prevCount = value;
            }
        }
        private Int32 _currentCount;
        public Int32 CurrentCount
        {
            get
            {
                return _currentCount;
            }
            set
            {
                _currentCount = value;
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
        private Int32 _absoluteDiffCount;
        public Int32 AbsoluteDiffCount
        {
            get
            {
                return _absoluteDiffCount;
            }
            set
            {
                _absoluteDiffCount = value;
            }
        }

    }
}