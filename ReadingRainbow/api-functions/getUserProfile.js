import * as SecureStore from 'expo-secure-store';

export async function GetUserProfile(iUsername) {
    const encodedUsername = encodeURIComponent(iUsername);
    const fullurl =  `http://10.0.2.2:5000/api/person/Person/${encodedUsername}`;

    SecureStore.getItemAsync('jwt').then(async (token) => {
        const response = await fetch(fullurl,
        {
            headers: {
                            
                'Authorization': 'Bearer ' + token,
                'Content-Type': 'application/json; charset=utf-8',
            },
        });
        const profile = await response.json();
        console.log('person response: ', profile);
        return profile;
    });
}