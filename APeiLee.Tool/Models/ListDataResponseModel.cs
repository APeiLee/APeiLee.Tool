using System;
using System.Collections.Generic;
using System.Text;

namespace APeiLee.Tool.Models
{
    public class ListDataResponseModel
    {
        public int Total { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public bool IsFirstPage { get; set; }

        public bool IsLastPage { get; set; }

        public dynamic? DataList { get; set; }

        public bool HasRecords { get; set; }

        public long RefreshTime { get; set; }
    }
}
