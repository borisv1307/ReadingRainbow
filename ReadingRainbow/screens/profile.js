import React, { useEffect, useState } from 'react';
import { View, Text, Button, ScrollView, ActivityIndicator } from 'react-native';
import { globalStyles } from '../styles/global';
import { GetUserProfile } from '../api-functions/getUserProfile';
import { GetUserLibrary } from '../api-functions/getUserLibrary';
import * as SecureStore from 'expo-secure-store';

export default function Profile() {
    const [ proresults, setProResults ] = useState(null);
    // const [ libresults, setLibResults ] = useState([]);
    
    useEffect(() => {
        async function checkData() {
            const data = await GetUserProfile('april2'); //will need to replace with props
            setProResults(data);
        }
        checkData();

    }, [])
    return (
        <View style={globalStyles.container}>
            <Text style={globalStyles.titleText}>Paige's Profile</Text>
            {proresults ? (
                <Text>{proresults}</Text> //still working to get these to display
            ):(
                <View>
                    <ActivityIndicator size="large" color="#99ff99" />
                </View>
            )}
        </View>
    );
}
