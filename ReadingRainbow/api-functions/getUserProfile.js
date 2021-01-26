import * as SecureStore from 'expo-secure-store';
import ConfigurationInfo from '../config.json'; 

export async function GetUserProfile(iUsername){
    const encodedUsername = encodeURIComponent(iUsername);

    const APIUserService = ConfigurationInfo.APIUserService;
    const fullurl =  APIUserService + `/api/person/Person/${encodedUsername}`;


    SecureStore.getItemAsync('jwt').then(async (token) => {
        const response = await fetch(fullurl,
        {
            headers: {
                            
                'Authorization': 'Bearer ' + token,
                'Content-Type': 'application/json; charset=utf-8',
            },
        });
        const jsonObj = await response.json();
        console.log('person response: ', jsonObj);
        let { Name: vName, Profile: vProfile, Portrait: vPortrait, Email: vEmail } = jsonObj;
        return ReturnProfile(vName, vProfile, vPortrait, vEmail);
    });
}

function ReturnProfile(iName, iProfile, iPortrait, iEmail) {
  console.log('parameters: ' + iName + iProfile + iPortrait + iEmail);
  var person = {
    UserName : ReturnData(iName),
    ProfileDescription: ReturnData(iProfile),
    PictureLink : ReturnData(iPortrait),
    Email : ReturnData(iEmail),
};

console.log('person: ', person);

   return person;
}

function ReturnData(data) {
  if (data == null) {
    return 'No Data Found';
  }
  else {
    return data;
  }
}