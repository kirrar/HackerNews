using System.Runtime.Serialization;

namespace HackerNewsPortal.Contracts
{
    [DataContract]
    public class PaginationRequest
    {
        public PaginationRequest()
        {
            this.PageNumber = 1;
            this.PageSize = 20;
        }

        public PaginationRequest(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 10 ? 10 : pageSize;
        }

        [DataMember]
        public int PageNumber { get; set; }

        [DataMember]
        public int PageSize { get; set; }
    }
}
