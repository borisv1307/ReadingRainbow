import * as SecureStore from 'expo-secure-store';
import ConfigurationInfo from '../config.json'; 

export async function RetrieveToken(iUsername, iPassword) {

    console.log(JSON.stringify({ username: iUsername, password: iPassword })); //Test purposes only
    const encodedUsername = encodeURIComponent(iUsername);
    const encodedPassword = encodeURIComponent(iPassword);

    const APIUserService = ConfigurationInfo.APIUserService ; 
    const fullurl = APIUserService + `/api/token?username=${encodedUsername}&password=${encodedPassword}`;

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
        return SecureStore.setItemAsync('jwt', token);
    } catch (e) {
        console.error(e);
    } finally {
        console.log('All tasks complete');
    }
 }

export async function CreateAccount(iUsername, iEmail, iPassword) {

    const vBody = JSON.stringify({ Name: iUsername, Email: iEmail, HashedPassword: iPassword });
    const APIUserService = ConfigurationInfo.APIUserService ; 
    const fullurl = APIUserService + `/api/person/AddPerson`;
    console.log(vBody);

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
        const strResponse = await response.text();
        var account_created = (strResponse == 'true');
        console.log('resposnse.text', account_created);
        return account_created;
    } catch (e) {
        console.error(e);
    } finally {
        console.log('All tasks complete');
    }
    return;
}

export async function ReSendEmail(iUsername, iPassword) {

    console.log(JSON.stringify({ username: iUsername, password: iPassword })); //Test purposes only
    const encodedUsername = encodeURIComponent(iUsername);
    const encodedPassword = encodeURIComponent(iPassword);

    const APIUserService = ConfigurationInfo.APIUserService ; 
    const fullurl = APIUserService + `/api/token/ReSendEmail?username=${encodedUsername}&email=${encodedPassword}`;

    try{

        const response = await fetch(fullurl,           
            {
                mode: 'cors',
                headers: {
                  'Access-Control-Allow-Origin':'*',
                  'Content-Type': 'application/json;charset=utf-8'
                },
            });
        const oResponse = await response.text();
        console.log('oResponse: ', oResponse);
        return oResponse;
    } catch (e) {
        console.error(e);
    } finally {
        console.log('All tasks complete');
    }
 }
