using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentersAPI.Models.Response
{
    public class SearchResponse:BaseResponse
    {
        public List<SmallCenter> smallCenters { get; set; }
        public List<SmallCourse> smallCourses { get; set; }
    }
    public class SearchHistoryResponse : BaseResponse
    {
        public List<SearchResponse> SearchResponse { get; set; }
        public List<string> Recomended { get; set; }
    }
}