using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TypeSenceApi.HraOpsApi.Model
{
    public enum APIStatus
    {
        processing = 102,
        success = 200,
        cancelled = 203,
        aborted = 406,
        error = 500,
        Notfound = 404
        
    }

    public class Webresponse<T> : WebresponseNoData
    {
        public T data { get; set; }
    }

    public class WebresponseNoData
    {
        //check with aditya integration api return status:true, that why APIStatus is commented
        public APIStatus status { get; set; }
        public string message { get; set; }
    }

    public class WebresponsePaging<T> : WebresponseNoData
    {
        public long totalrecords { get; set; }
        public long totalpage
        {
            get
            {
                if (this.pageSize < 1)
                    return this.totalrecords / 10;
                return this.totalrecords / this.pageSize;
            }
        }
        public int currentpage { get { return this.page; } }
        public int nextpage
        {
            get
            {
                if (this.currentpage >= this.totalpage)
                    return 1;
                return this.currentpage + 1;
            }
        }
        public int previouspage
        {
            get
            {
                if (this.currentpage - 1 < 0)
                    return 0;
                return this.currentpage - 1;
            }
        }
        public int page { get; set; } = 1;
        public long pageSize { get; set; } = 1;
        public T data { get; set; }
    }
}
