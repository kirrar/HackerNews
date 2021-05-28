using HackerNewsPortal.Models;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HackerNewsPortal.Contracts
{
    [DataContract]
    public class PaginationResponse
    {
        public PaginationResponse()
        {
            this.Stories = new List<Story>();
        }

        [DataMember]
        public int PageNumber { get; set; }

        [DataMember]
        public int PageSize { get; set; }

        [DataMember]
        public int TotalStories { get; set; }

        [DataMember]
        public List<Story> Stories { get; set; }
    }
}
