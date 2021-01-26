import * as SecureStore from 'expo-secure-store';
import ConfigurationInfo from '../config.json';

export async function GetUserLibrary(iUsername) {

    const encodedUsername = encodeURIComponent(iUsername);
    console.log('encoded username: ', encodedUsername);

    const APIUserService = ConfigurationInfo.APIUserService;
    const fullurl =  APIUserService + `/api/person/Library/${encodedUsername}`;

    SecureStore.getItemAsync('jwt').then(async (token) => {
        const response = await fetch(fullurl,
        {
            headers: {
                'Authorization': 'Bearer ' + token,
                'Content-Type': 'application/json; charset=utf-8',
            },
        });
        try {
            const library = await response.json();
            console.log('Library response ', json);
        } catch(e) {
            console.log(e);
        }
        return library;
    });
} 

function ReturnBook(name, index)
{
 var book = {
   Id : name.Id,
   BookInformationLink: name.BookInformationLink,
   Title : name.Title,
   //Authors : book.Authors,
   Thumbnail : name.Thumbnail, 
   //SmallThumbnail : book.SmallThumbnail, 
   PublishDate : name.PublishDate,
   //NumberPages : book.NumberPages,
   Description : name.Description,
   ISBN_10 : name.isbN_10, // ISBN_10 {get; set;}   
   ISBN_13: name.isbN_13, // ISBN_13 {get; set;} 
   // Categories: book.Genres

   }

   return name;
}