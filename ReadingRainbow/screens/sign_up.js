import React from 'react';
import { Button, TouchableOpacity, TextInput, View, Text } from 'react-native';
import { globalStyles } from '../styles/global';
import { useNavigation } from '@react-navigation/native';
import { AuthContext } from '../components/context';
import SignIn from './sign_in';

export default function SignUp() {
    const { navigate } = useNavigation();

    return (
        <View style={globalStyles.container}>
            <Text>Create Your Profile! Your Profile is set to Private by default, only friends you accepted can see your profile.</Text>
            <TextInput
                style={globalStyles.input}
                keyboardType='default'
                placeholder='Profile Information'/>
            <TouchableOpacity style={globalStyles.smallButton}>
                <Text style={globalStyles.buttonText}>Upload a Profile Image!</Text>
            </TouchableOpacity>
            <Text>Tell us about your book preferences!</Text>
            <TextInput
                style={globalStyles.input}
                keyboardType='default'
                placeholder='Favorite Author?'/>
            <TextInput
                style={globalStyles.input}
                keyboardType='default'
                placeholder='Favorite Book?'/>
            <Text>Please Select Genres You Like!</Text>
            <Button title='Continue to Homepage!' onPress={() => navigate('Home')} />
        </View>
    );
}