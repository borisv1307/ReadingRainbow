import * as SecureStore from 'expo-secure-store';
import ConfigurationInfo from '../config.json'; 

export async function GetUserProfile(iUsername) {
    const encodedUsername = encodeURIComponent(iUsername);

    const APIUserService = ConfigurationInfo.APIUserService;
    const fullurl =  APIUserService + `/api/person/Person/${encodedUsername}`;

    try {
        return SecureStore.getItemAsync('jwt').then(async (token) => {
            const response = await fetch(fullurl,
            {
                headers: {
                                
                    'Authorization': 'Bearer ' + token,
                    'Content-Type': 'application/json; charset=utf-8',
                },
            });
            const profile = await response.json();
            return profile;
        });
    } catch(e) {
        console.log(e);
    } finally {
        console.log('All tasks complete');
    }
}
