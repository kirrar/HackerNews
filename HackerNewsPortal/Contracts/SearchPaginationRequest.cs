using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace HackerNewsPortal.Contracts
{
    [DataContract]
    public class SearchPaginationRequest
    {
        public SearchPaginationRequest()
        {
            this.PageNumber = 1;
            this.PageSize = 20;
        }

        public SearchPaginationRequest(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 10 ? 10 : pageSize;
        }

        [DataMember]
        public int PageNumber { get; set; }

        [DataMember]
        public int PageSize { get; set; }

        [DataMember]
        public string SearchTerm { get; set; }
    }
}
