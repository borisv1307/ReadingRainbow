import * as SecureStore from 'expo-secure-store';
import ConfigurationInfo from '../config.json'; 

export async function GetPeople() {
    const APIUserService = ConfigurationInfo.APIUserService;
    const fullurl =  APIUserService + `/api/person/People`;

    try {
        return SecureStore.getItemAsync('jwt').then(async (token) => {
            const response = await fetch(fullurl,
            {
                headers: {          
                    'Authorization': 'Bearer ' + token,
                    'Content-Type': 'application/json; charset=utf-8',
                },
            });
            const people = await response.json();
            return people.map(person => ReturnProfile(person));
        });
    } catch(e) {
        console.log(e);
    } finally {
        console.log('All tasks complete');
    }
}

function ReturnProfile(info) {
    console.log('made it: ', info);
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
      return 'No Data Available';
    } else {
      return field;
    }
}
