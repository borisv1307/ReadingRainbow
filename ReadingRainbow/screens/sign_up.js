import React from 'react';
import { Button, TextInput, View, Text, Alert } from 'react-native';
import { globalStyles } from '../styles/global';
import { AuthContext } from '../components/context';
import { ScrollView } from 'react-native-gesture-handler';
import { CreateAccount } from '../api-functions/authentication.js';
import * as Crypto from 'expo-crypto';

export default function SignUp({navigation}) {

    const [data, setData] = React.useState({
        email:'',
        username: '',
        password: '',
        password_confirm:'',
        check_textInputChange: false,
        isValidUser: true,
        isValidPassword: true,
    });

    const { signUp } = React.useContext(AuthContext);

    const signUpHandle = () => {
        console.log('------------------------------------')
        console.log('email: ', data.email);
        console.log('username: ', data.username);
        console.log('password: ', data.password);
        console.log('password_confirm: ', data.password_confirm);
        
        if (data.password !== data.password_confirm) {
            Alert.alert(
                "Invalid Data",
                "Passwords are different"
            );
            return;
        }

        (async () => {
            try {
                const digest = await Crypto.digestStringAsync(
                    Crypto.CryptoDigestAlgorithm.SHA256,
                    data.password
                );
                setData({
                    ...data,
                    password: digest
                });
            } catch (e) {
                console.log(e);
            }
        })().then(CreateAccount(data.username, data.email, data.password));
    }

    return (
        <View style={globalStyles.container}>
            <ScrollView>
                <Text>Email address:</Text>
                <TextInput
                    style={globalStyles.input}
                    placeholder='paige123@turner.com'
                    onChangeText={(text) => setData({...data, email: text})}/>
                <Text>Username:</Text>
                <TextInput
                    style={globalStyles.input}
                    placeholder='Books4Ever123'
                    onChangeText={(text) => setData({...data, username: text})}/>
                <Text>Password:</Text>
                <TextInput
                    style={globalStyles.input}
                    secureTextEntry={true}
                    onChangeText={(text) => setData({...data, password: text})}/>
                <Text>Confirm Password:</Text>
                <TextInput
                    style={globalStyles.input}
                    secureTextEntry={true}
                    onChangeText={(text) => setData({...data, password_confirm: text})}/>            
                <Button title='SUBMIT' onPress={() => {signUpHandle()}} />
            </ScrollView>
        </View>
    );
}