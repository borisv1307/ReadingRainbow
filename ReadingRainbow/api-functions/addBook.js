import * as SecureStore from 'expo-secure-store';
import ConfigurationInfo from '../config.json'; 

export async function AddBook(iUsername, iBookObj) {
    const encodedUsername = encodeURIComponent(iUsername);
    const APIUserService = ConfigurationInfo.APIUserService;
    const fullurl =  APIUserService + `/api/book/AddBookToLibrary/${encodedUsername}`;
    console.log(iBookObj);
    try {
        return SecureStore.getItemAsync('jwt').then(async (token) => {
            const response = await fetch(fullurl,
            {
                method: 'POST',
                headers: {
                                
                    'Authorization': 'Bearer ' + token,
                    'Content-Type': 'application/json; charset=utf-8',
                },
                body: JSON.stringify(iBookObj),
            });
            const message = await response.json();
            console.log(message);
            return message;
        });
    } catch(e) {
        console.log(e);
    } finally {
        console.log('All tasks complete');
    }
}
