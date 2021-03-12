using System.Collections.Generic;

namespace ReadingRainbowAPI.Models
{
    public class BookList : Neo4jEntity
    {
        public BookList()
        {
            Label = "BookList";
        }
        public Book Book { get; set; }
    }
}

// namespace ReadingRainbowAPI.Models
// {
//     public class Books : IEnumerable<Book>
//     {
//         public List<Book> BookList {get; set; }

//         public IEnumerator<Book> GetEnumerator()
//         {
//             return BookList.GetEnumerator();
//         }

//         IEnumerator IEnumerable.GetEnumerator()
//         {
//             return BookList.GetEnumerator();
//         }
//     }
// }

// namespace ReadingRainbowAPI.Models
// {
//     public class Books : Neo4jEntity
//     {
//         public string Id {get; set;}
//         public string BookInformationLink {get; set;}
//         public string Title {get; set;}
//         public List<string> Authors = new List<string>();
//         public string Thumbnail {get; set;}
//         public string SmallThumbnail {get; set;}
//         public string PublishDate {get; set;}
//         public string NumberPages {get; set;}
//         public string Description {get; set;}
//         public string ISBN_10 {get; set;}      
//         public string ISBN_13 {get; set;}   
//         public List<Genre> Genres = new List<Genre>();
//     } 

//         private List<Books> _books = new List<Books>();
//         public IEnumerable<Books> BookList
//         {
//             get {return _books;}
//         }

//         public IEnumerable<Book> GetEnumerator()
//         {
//             return Book.GetEnumerator();
//         }
//         IEnumerator IEnumerable.GetEnumerator()
//         {
//             return Book.GetEnumerator();
//         }
// }