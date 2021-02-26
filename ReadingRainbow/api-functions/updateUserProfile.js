import * as SecureStore from 'expo-secure-store';
import ConfigurationInfo from '../config.json'; 

export async function UpdateUserProfile(iPersonObj) {
    const APIUserService = ConfigurationInfo.APIUserService;
    const fullurl =  APIUserService + `/api/person/UpdateProfile`;
    
    try {
        return SecureStore.getItemAsync('jwt').then(async (token) => {
            const response = await fetch(fullurl,
            {
                method: 'POST',
                headers: {
                                
                    'Authorization': 'Bearer ' + token,
                    'Content-Type': 'application/json; charset=utf-8',
                },
                body: JSON.stringify(iPersonObj)
            });
            const profile = await response.json();
            return ReturnProfile(profile);
        });
    } catch(e) {
        console.log(e);
    } finally {
        console.log('All tasks complete');
    }
}

function ReturnProfile(info) {
    var profile = {
        Email : CheckForNull(info.Email),
        Name : CheckForNull(info.Name),
        Portrait : CheckForNull(info.Portrait),
        Profile : CheckForNull(info.Profile),
    }
        return profile;
}

function CheckForNull(field) {
    if (field == null) {
      return 'No data found';
    } else {
      return field;
    }
}
