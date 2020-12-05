export async function GetUserProfile(iUsername) {

    const encodedUsername = encodeURIComponent(iUsername);
    var token =  await SecureStore.getItemAsync('jwt');

      const fullurl =  `http://10.0.2.2:5000/api/person/Person/${encodedUsername}`;
    try{
        const response = await fetch(fullurl,           
            {
                headers: {
                  'Content-Type': 'application/json; charset=utf-8',
                  'Authorization': 'Bearer ' + token
                },
            });

            const json = await response.json();
            console.log('person response: ', json)
            return json.items.map((person, index) => ReturnProfile(person, index));                    
    } catch (err) {
        console.error(err);
    } finally {
        console.log('All tasks complete');
    }
    return;
} 

function ReturnProfile(user, index)
{
 var Person = {
   UserName : user.Name,
   ProfileDescription: user.Profile,
   PictureLink : user.Portrait,
   Email : user.Email,
 };

   return Person;
}