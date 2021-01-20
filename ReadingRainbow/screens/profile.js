import React, { useEffect, useState } from 'react';
import { View, Text, Button, ScrollView, ActivityIndicator } from 'react-native';
import { globalStyles } from '../styles/global';
import { GetUserProfile } from '../api-functions/getUserProfile';
import { GetUserLibrary } from '../api-functions/getUserLibrary';
import * as SecureStore from 'expo-secure-store';

export default function Profile() {
    const [ proResults, setProResults ] = useState([]);
    const [ libResults, setLibResults ] = useState([]);
    const [isLoading, setLoading] = useState(true);
    
    useEffect(() => {
        GetUserProfile('april2').then(profile => {
            setProResults(profile);
            console.log(proResults);
        });
        GetUserLibrary('april2').then(library => {
            setLibResults(library);
            console.log(libResults);
        });
        setLoading(false);
    }, [])

    return (
        <View style={globalStyles.container}>
            <Text style={globalStyles.titleText}>Paige's Profile</Text>
        </View>
    );
}
