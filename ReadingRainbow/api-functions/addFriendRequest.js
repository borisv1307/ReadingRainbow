import * as SecureStore from 'expo-secure-store';
import ConfigurationInfo from '../config.json'; 

export async function addFriendRequest(iUser, iFriend) {
    const encodedUsername = encodeURIComponent(iUser);
    const encodedFriend = encodeURIComponent(iFriend);
    const APIUserService = ConfigurationInfo.APIUserService;
    const fullurl =  APIUserService + `/api/friend/RequestFriend?userName=${encodedUsername}&friendName=${encodedFriend}`;
    console.log(fullurl);
    
    try {
        return SecureStore.getItemAsync('jwt').then(async (token) => {
            const response = await fetch(fullurl,
            {
                method: 'POST',
                headers: {      
                    'Authorization': 'Bearer ' + token,
                    'Content-Type': 'application/json; charset=utf-8',
                }
            });
            const request_sent = await response.json();
            console.log('Request Sent: ', request_sent);
            return request_sent;
        });
    } catch(e) {
        console.log(e);
        throw error;
    } finally {
        console.log('All tasks complete');
    }
}

// function ReturnProfile(info) {
//     var profile = {
//         Email : CheckForNull(info.Email),
//         Name : CheckForNull(info.Name),
//         Portrait : CheckForNull(info.Portrait),
//         Profile : CheckForNull(info.Profile),
//     }
//         return profile;
// }

// function CheckForNull(field) {
//     if (field == null) {
//       return 'No data found';
//     } else {
//       return field;
//     }
// }
