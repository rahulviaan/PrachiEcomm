using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GleamTech.DocumentUltimateExamples.AspNetCoreCS.Models
{
    public class TestHOurModels
    {

        public class Response<T>
        {
            public string Id { get; set; }
            public long? id { get; set; }
            public string Title { get; set; }
            public string Detail { get; set; }
            public int Status { get; set; }
            public T Data { get; set; }
        }


        public class MasterDatatModel
        {
            public List<PublisherDataModel> Publishers { get; set; }
            public List<BoardDataModel> Boards { get; set; }
            public List<SubjectDataModel> Subjects { get; set; }
            public List<SeriesDataModel> Series { get; set; }
            public List<ClassDataModel> Classes { get; set; }
            public List<BookDataModel> Books { get; set; }

        }

        public class BoardDataModel
        {
            public long Id { get; set; }
            public string GlobalId { get; set; }
            public string Title { get; set; }
            public int? MasterPublisherId { get; set; }
            public int? DisplayOrder { get; set; }
        }
        public class SubjectDataModel
        {
            public long Id { get; set; }
            public string GlobalId { get; set; }
            public string Title { get; set; }
            public int? MasterPublisherId { get; set; }
            public int? DisplayOrder { get; set; }

        }
        public class SeriesDataModel
        {
            public long Id { get; set; }
            public long MasterSubjectId { get; set; }
            public string GlobalId { get; set; }
            public string Title { get; set; }
            public int? MasterPublisherId { get; set; }
            public int? DisplayOrder { get; set; }
        }
        public class PublisherDataModel
        {
            public long Id { get; set; }
            public string GlobalId { get; set; }
            public int? DisplayOrder { get; set; }
            public string Title { get; set; }

        }
        public class ClassDataModel
        {
            public long Id { get; set; }
            public string GlobalId { get; set; }
            public string Title { get; set; }
            public int? MasterPublisherId { get; set; }
            public int? DisplayOrder { get; set; }

        }
        public class BookDataModel
        {
            public long Id { get; set; }
            public long? MasterSubjectId { get; set; }
            public long? MasterSeriesId { get; set; }
            public long? MasterBoardId { get; set; }
            public long? MasterClassId { get; set; }
            public long? MasterPublisherId { get; set; }
            public string GlobalId { get; set; }
            public string Title { get; set; }
            public string Author { get; set; }
            public int? DisplayOrder { get; set; }
            public string ISBN { get; set; }
            public string Edition { get; set; }
            public decimal? Price { get; set; }
            public decimal? Discount { get; set; }

        }
        public class SearchBookModel
        {
            public long? bookid { get; set; }
            public string bookname { get; set; }
            public string subjectname { get; set; }
            public string isbnname { get; set; }
            public string classname { get; set; }
        }

    }

}
