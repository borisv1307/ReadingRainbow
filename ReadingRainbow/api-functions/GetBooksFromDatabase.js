
export async function GetBooksFromDatabase() {

    const fullurl = 'http://localhost:5000/api/book/FindfromNeo';

    // create full request URL by joining parts of the string with the base request
    try{

        const response = await fetch(fullurl,           
            {
                mode: 'cors',
                headers: {
                  'Access-Control-Allow-Origin':'*'
                }
            });
        const json = await response.json();
        console.log(json);
        return json.map((name, index) => ReturnBook(name, index));
    } catch (err) {
        console.error(err);
    } finally {
        console.log('All tasks complete'); //Feel free to change this
    }
 }

 function ReturnBook(name, index)
 {
  var book = {
    Index : index,
    //BookInformationLink: name.selfLink,
    Title : name.title,
    //Authors : name.volumeInfo.authors,
    Thumbnail : name.thumbnail, 
    SmallThumbnail : name.smallThumbnail, 
    PublishDate : name.publishDate,
    NumberPages : name.numberPages,
    Description : name.description,
    ISBN_10 : name.isbN_10, 
    ISBN_13: name.isbN_13,
    ISBN_Other: name.isbN_Other,
    // Categories: name.volumeInfo.categories
    }

    return book;
 }
