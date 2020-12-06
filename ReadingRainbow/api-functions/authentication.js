import * as SecureStore from 'expo-secure-store';
import { AsyncStorage } from 'react-native';

export async function RetrieveToken(iUsername, iPassword) {

    console.log(JSON.stringify({ username: iUsername, password: iPassword })); //Test purposes only
    const encodedUsername = encodeURIComponent(iUsername);
    const encodedPassword = encodeURIComponent(iPassword);
   
    const fullurl = `http://10.0.2.2:5000/api/token?username=${encodedUsername}&password=${encodedPassword}`;

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
    } catch (e) {
        console.error(e);
    } finally {
        console.log('All tasks complete');
    }
    return;
}

export async function CreateAccount(iUsername, iEmail, iPassword) {

    const vBody = JSON.stringify({ Name: iUsername, Email: iEmail, HashedPassword: iPassword });
    console.log(vBody);
    const fullurl = `http://10.0.2.2:5000/api/person/AddPerson`;

    try{

        const response = await fetch(fullurl,           
            {
                mode: 'cors',
                method: 'POST',
                headers: {
                  'Access-Control-Allow-Origin':'*',
                  'Accept': 'application/json',
                  'Content-Type': 'application/json;charset=utf-8'
                },
                body: vBody,
            });
        const account_activated = await response.text();
        await AsyncStorage.setItem('account_activated', account_activated);
        // await AsyncStorage.setItem('account_activated', 'true');
        console.log('account_activated: ', AsyncStorage.getItem(account_activated));
    } catch (e) {
        console.error(e);
    } finally {
        console.log('All tasks complete');
    }
    return;
}