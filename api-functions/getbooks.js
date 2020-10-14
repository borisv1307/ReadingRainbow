import {useState, useEffect} from 'react';

export function SearchBooksByTitle(searchTitle, startIndex = 0)
{
  if (searchTitle != "")
  {
    searchTitle = 'intitle:' + searchTitle 
  }

  return GetBooks(URLString(searchTitle),startIndex);
}

export function SearchBooksByAuthor(searchAuthor, startIndex = 0)
{
  if (searchAuthor != "")
  {
    searchAuthor = 'inauthor:' + searchAuthor  
  }

  return GetBooks(URLString(searchAuthor), startIndex);
}

export function SearchBooksByISBN(searchIsbn, startIndex = 0)
{
  if (searchIsbn != "")
  {
    searchIsbn = 'isbn:' + searchIsbn    
  }

  return GetBooks(searchIsbn, startIndex);
}

export function GetBooks(searchStr, startIndex = 0) {

    const [books, setBooks] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    const baseurl = 'https://www.googleapis.com/books/v1/volumes';
    const mykey = 'AIzaSyCY-Xy_wHFKlBpZfEHI1kthE-t9KeU0LHo';

    // create full request URL by joining parts of the string with the base request
    let fullurl = [baseurl, '?q=', searchStr, '&startIndex=', CheckInt(startIndex), '&key=', mykey].join('');

    useEffect (() =>  {fetch(fullurl)
      .then(response => {
            if (response.ok) 
              return response.json();
      })
      .then(responsejson => {
            setBooks (responsejson.items);
       })
      .catch(error => {
        console.error(error);
        setError(error);
      })
      .finally(() => {
        setLoading(false);
      });
    }, []);

    if (loading) return "Loading...";
    if (error) return "Oops!";
    if (books == null)
    {
      return "No books found";
    }
    else
    {
      return (books.map((name, index) => (
        ReturnBook(name, index)
    )));
    }
 }

 function ReturnBook(name, index)
 {
  var book = {
    Index : name.id,
    BookInformationLink: name.selfLink,
    Title : name.volumeInfo.title,
    Authors : name.volumeInfo.authors,
    Thumbnail : ReturnImage(name.volumeInfo.imageLinks, 'thumbnail'), 
    SmallThumbnail : ReturnImage(name.volumeInfo.imageLinks, 'smallThumbnail'), 
    PublishDate : name.volumeInfo.publishedDate,
    NumberPages : name.volumeInfo.pageCount,
    Description : name.volumeInfo.description,
    ISBN_10 : ReturnISBN(name.volumeInfo.industryIdentifiers, 'ISBN_10'), 
    ISBN_13: ReturnISBN(name.volumeInfo.industryIdentifiers, 'ISBN_13'),
    ISBN_Other: ReturnISBN(name.volumeInfo.industryIdentifiers, 'OTHER'),
    Categories: name.volumeInfo.categories
    }

    return book;
 }

 function ReturnImage(image, type)
 {
   if (image == null)
   {
     return "No Image Found";
   }
   else
   {
      if (type == "smallThumbnail")
        return image.smallThumbnail;
      else
        return image.thumbnail;
   }
 }

 function URLString(searchStr)
 {
    return searchStr.replace('','+');
 }

 function CheckInt(startIndex)
 {
    if (startIndex === parseInt(startIndex,10))
      return startIndex;
    else
      return 0;
 }

 function ReturnISBN(ISBNArray, ISBNtype)
 {
   if (ISBNArray == null)
  {
      return "No ISBN Found"
  }

  var ISBNId;

  ISBNArray.forEach(function(isbn) {
    if (isbn.type == ISBNtype)
    {
      ISBNId =  isbn.identifier;
      return;
    }
  });

  return ISBNId;

 }
