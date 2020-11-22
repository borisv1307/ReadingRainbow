import * as SecureStore from 'expo-secure-store';

export async function RetrieveToken(iUsername, iPassword) {

    console.log(JSON.stringify({ username: iUsername, password: iPassword })); //Test purposes only
    const encodedUsername = encodeURIComponent(iUsername);
    const encodedPassword = encodeURIComponent(iPassword);

    console.log('APi username: ', encodedUsername);
    console.log('APi password: ', encodedPassword);

    //const fullurl = `https://localhost:5001/api/token?username=${encodedUsername}&password=${encodedPassword}`;
    //const fullurl = 'https://localhost:5001/api/token';
    //173.16.48.206
    const fullurl = '173.16.48.206:5001/api/token';
    console.log('full url: ', fullurl);
    try{
        const response = await fetch(fullurl,           
            {
                mode: 'cors',
                headers: {
                  'Access-Control-Allow-Origin':'*',
                  'Content-Type': 'text/plain; charset=utf-8'
                },
                params: {
                    'username': `${encodedUsername}`,
                    'password': `${encodedPassword}`,
                }
            });
        const token = await response.json();
        console.log('token from web request: ', token);
        await SecureStore.setItemAsync('jwt', token);
    } catch (err) {
        console.error(err);
    } finally {
        console.log('All tasks complete');
    }
    return;
} 