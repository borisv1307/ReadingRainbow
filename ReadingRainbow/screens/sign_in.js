import React, { useState } from 'react';
import { TextInput, View, Text, TouchableOpacity, Button, Alert } from 'react-native';
import { globalStyles } from '../styles/global.js';
import { useNavigation } from '@react-navigation/native';
import { AuthContext } from '../components/context';
// import Users from '../model/users';
import * as Crypto from 'expo-crypto';
import { RetrieveToken } from '../api-functions/authentication.js';
import * as SecureStore from 'expo-secure-store'; 

const SignIn = ({navigate}) => {
    const [data, setData] = React.useState({
        username: '',
        password: '',
        check_textInputChange: false,
        isValidUser: true,
        isValidPassword: true,
    });

    const { signIn } = React.useContext(AuthContext);
    
    // const textInputChange = (val) => {
    //     if( val.trim().length >= 4 ) {
    //         setData({
    //             ...data,
    //             username: val,
    //             check_textInputChange: true,
    //             isValidUser: true
    //         });
    //     } else {
    //         setData({
    //             ...data,
    //             username: val,
    //             check_textInputChange: false,
    //             isValidUser: false
    //         });
    //     }
    // }

    // const handlePasswordChange = (val) => {
    //     if( val.trim().length >= 8 ) {
    //         setData({
    //             ...data,
    //             password: val,
    //             isValidPassword: true
    //         });
    //     } else {
    //         setData({
    //             ...data,
    //             password: val,
    //             isValidPassword: false
    //         });
    //     }
    // }

    // const handleValidUser = (val) => {
    //     if( val.trim().length >= 4 ) {
    //         setData({
    //             ...data,
    //             isValidUser: true
    //         });
    //     } else {
    //         setData({
    //             ...data,
    //             isValidUser: false
    //         });
    //     }
    // }

    // async function runCrypto(input) {
    //     data.password = await Crypto.digestStringAsync(
    //         Crypto.CryptoDigestAlgorithm.SHA256,
    //         input
    //     );
    // }

    const loginHandle = () => {
        console.log('------------------------------------')
        // if ( data.username.length == 0 || data.password.length == 0 ) {
        //     Alert.alert('Wrong Input!', 'Username or password field cannot be empty.', [
        //         {text: 'Okay'}
        //     ]);
        //     return;
        // }

        // if ( foundUser.length == 0 ) {
        //     Alert.alert('Invalid User!', 'Username or password is incorrect.', [
        //         {text: 'Okay'}
        //     ]);
        //     return;
        // }
        console.log('username: ', data.username);
        console.log('password: ', data.password);
        // const digest2 = '';
        // async function runCrypto() {
        //     const digest = await Crypto.digestStringAsync(
        //         Crypto.CryptoDigestAlgorithm.SHA256,
        //         data.password
        //     );
        //     console.log('Digest: ', digest);
        //     return digest;
        // }
        (async () => {
            try {
                const digest = await Crypto.digestStringAsync(
                    Crypto.CryptoDigestAlgorithm.SHA256,
                    data.password
                );
                setData({
                    ...data,
                    password:  data.password
                });
            } catch (e) {
                console.log(e);
            }
        })().then(    
            RetrieveToken(data.username, data.password).then(
                (async () => {
                    try {
                        const token = await SecureStore.getItemAsync('jwt');
                        console.log('value of token: ', token);
                        SecureStore.deleteItemAsync('jwt');
                    } catch (e) {
                        console.log(e);
                    }
                })()
            )
        );
        // signIn(returnedToken);
    }

    return (
        <View style={globalStyles.container}>
            <Text>
                Welcome to Reading Rainbow, an App for Literature Nerds to share
                Libraries, Browse Books, Find Friends, Recommend Books and More!
            </Text>
            <View>
                <TextInput
                    style={globalStyles.input}
                    placeholder='pageTurner123'
                    onChangeText={(text) => setData({...data, username: text})} 
                    // onEndEditing={(e)=>handleValidUser(e.nativeEvent.text)}
                />
                <TextInput
                    style={globalStyles.input}
                    placeholder='password'
                    onChangeText={(val) => setData({...data, password: val})}
                    secureTextEntry={true}
                    autoCapitalize='none'
                    // onChangeText={(val) => handlePasswordChange(val)}
                />
                <Button
                    title="Sign In"
                    onPress={() => {loginHandle()}} />
                <TouchableOpacity
                    style={globalStyles.smallButton}
                    onPress={() => navigate('SignUpScreen')}>
                    <Text style={globalStyles.buttonText}>Create Account</Text>
                </TouchableOpacity>
                <TouchableOpacity
                    style={globalStyles.smallButton}
                    onPress={() => navigate('ForgotPassword')} >
                    <Text style={globalStyles.buttonText}>Forgot Password</Text>
                </TouchableOpacity>
                <TouchableOpacity style={globalStyles.smallButton}>
                    <Text style={globalStyles.buttonText}>Sign In With Google</Text>
                </TouchableOpacity>
                <TouchableOpacity style={globalStyles.smallButton}>
                    <Text style={globalStyles.buttonText}>Sign In With Apple</Text>
                </TouchableOpacity>
                <TouchableOpacity style={globalStyles.smallButton}>
                    <Text style={globalStyles.buttonText}>Sign In With Facebook</Text>
                </TouchableOpacity>
            </View>
        </View>
    );
}

export default SignIn;