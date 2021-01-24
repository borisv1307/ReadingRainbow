import * as SecureStore from 'expo-secure-store';
export async function GetUserLibrary(iUsername) {

    const encodedUsername = encodeURIComponent(iUsername);
    console.log('encoded username: ', encodedUsername);

    const fullurl =  `http://10.0.2.2:5000/api/person/Library/${encodedUsername}`;
    try{
        return SecureStore.getItemAsync('jwt').then(async (token) => {
            const response = await fetch(fullurl,
            {
                headers: {
                    'Authorization': 'Bearer ' + token,
                    'Content-Type': 'application/json; charset=utf-8',
                },
            });
            const library = await response.json();
            return library;
        });
    } catch(e) {
        console.error(e);
    } finally {
        console.log('All tasks complete');
    }
} 
