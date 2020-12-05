export async function GetUserProfile(iUsername) {

    const encodedUsername = encodeURIComponent(iUsername);

      const fullurl =  `http://10.0.2.2:5000/api/person/Library/${encodedUsername}`;
    try{
        const response = await fetch(fullurl,           
            {
                headers: {
                  'Content-Type': 'application/json; charset=utf-8'
                },
            });

            const json = await response.json();
            console.log('users library: ', json)
            return json.items.map((name, index) => ReturnBook(name, index));                    
    } catch (err) {
        console.error(err);
    } finally {
        console.log('All tasks complete');
    }
    return;
} 

function ReturnBook(name, index)
{
 var book = {
   Id : name.Id,
   BookInformationLink: name.BookInformationLink,
   Title : name.Title,
   //Authors : name.Authors,
   Thumbnail : name.Thumbnail, 
   SmallThumbnail : name.SmallThumbnail, 
   PublishDate : name.PublishDate,
   NumberPages : name.NumberPages,
   Description : name.Description,
   ISBN_10 : name.isbN_10, // ISBN_10 {get; set;}   
   ISBN_13: name.isbN_13, // ISBN_13 {get; set;} 
   // Categories: name.Genres

   }

   return book;
}