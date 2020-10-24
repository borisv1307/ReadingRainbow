import React, { useState } from 'react';
import { TextInput, View, Text, TouchableOpacity, Button} from 'react-native';
import { globalStyles } from '../styles/global.js';
import { useNavigation } from '@react-navigation/native';

export default function SignIn() {
    const { navigate } = useNavigation();
    const [ text, setText ] = useState('');
    return (
        <View style={globalStyles.container}>
            <Text>
                Welcome to Reading Rainbow, an App for Literature Nerds to share
                Libraries, Browse Books, Find Friends, Recommend Books and More!
            </Text>
            <View>
                <TextInput
                    style={globalStyles.input}
                    placeholder='pageTurner@example.com'
                    onChangeText={(text) => setText(text)} />
                <TextInput
                    placeholder='password'
                    onChangeText={(text) => setText(text)}
                    secureTextEntry={true} />
                <Button
                    title="Sign In"
                    onPress={() => navigate("Home")} />
                <TouchableOpacity
                    style={globalStyles.smallButton}
                    onPress={() => navigate('SignUp')}>
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
