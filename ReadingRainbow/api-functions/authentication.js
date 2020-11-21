import * as SecureStore from 'expo-secure-store';

export async function RetrieveToken(iUsername, iPassword) {

    console.log(JSON.stringify({ username: iUsername, password: iPassword })); //Test purposes only
    const encodedUsername = encodeURIComponent(iUsername);
    const encodedPassword = encodeURIComponent(iPassword);
    const fullurl = `https://localhost:5001/api/token?username=${encodedUsername}&password=${encodedPassword}`;
    try{

        // const response = await fetch(fullurl,           
        //     {
        //         mode: 'cors',
        //         headers: {
        //           'Access-Control-Allow-Origin':'*',
        //           'Content-Type': 'application/json;charset=utf-8'
        //         },
        //     });
        // const token = await response.json();
        SecureStore.setItemAsync('jwt', 'placeholder token'); //change back
    } catch (err) {
        console.error(err);
    } finally {
        console.log('All tasks complete');
    }
    return;
}