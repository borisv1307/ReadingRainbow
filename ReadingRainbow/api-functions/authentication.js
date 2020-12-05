import * as SecureStore from 'expo-secure-store';

export async function RetrieveToken(iUsername, iPassword) {

    console.log(JSON.stringify({ username: iUsername, password: iPassword })); //Test purposes only
    const encodedUsername = encodeURIComponent(iUsername);
    const encodedPassword = encodeURIComponent(iPassword);
    //localhost -> 10.0.2.2 for Megan
    const fullurl = `http://localhost:5000/api/token?username=${encodedUsername}&password=${encodedPassword}`;

    try{

        const response = await fetch(fullurl,           
            {
                mode: 'cors',
                headers: {
                  'Access-Control-Allow-Origin':'*',
                  'Content-Type': 'application/json;charset=utf-8'
                },
            });
        const token = await response.text();
        await SecureStore.setItemAsync('jwt', token);
    } catch (err) {
        console.error(err);
    } finally {
        console.log('All tasks complete');
    }
    return;
}