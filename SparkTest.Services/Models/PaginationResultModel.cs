using System.Collections.Generic;

namespace SparkTest.Services.Models
{
    public class PaginationResponseModel<T>
    {
        public PaginationResponseModel()
        {
        }

        public PaginationResponseModel(List<T> data, long totalCount)
        {
            Data = data;
            TotalCount = totalCount;
        }

        public long TotalCount { get; set; }

        public List<T> Data { get; set; }
    }
}
