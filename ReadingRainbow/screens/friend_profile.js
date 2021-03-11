import React, { useEffect, useState } from 'react';
import { View, Text, Image, ScrollView, ActivityIndicator, AsyncStorage, FlatList, TouchableOpacity, Button } from 'react-native';
import { globalStyles } from '../styles/global';
import { GetUserProfile } from '../api-functions/getUserProfile';
import { GetUserLibrary } from '../api-functions/getUserLibrary';
import { useNavigation } from '@react-navigation/native';
import { addFriendRequest } from '../api-functions/addFriendRequest';

export default function FriendProfile({route}) {
    const { Email, Name, Portrait, Profile } = route.params;
    const [ proResults, setProResults ] = useState({});
    const [ libResults, setLibResults ] = useState([]);
    const { navigate } = useNavigation();
    
    useEffect(() => {
        AsyncStorage.getItem('username').then(user => {
            GetUserProfile(user).then(profile => setProResults(profile));
            GetUserLibrary(user).then(library => setLibResults(library));
        });
    }, []);

    async function FriendRequestHandler() {
        try {
            AsyncStorage.getItem('username').then(user => {
                addFriendRequest(user, Name);
            });
        } catch (e) {
            console.log(e);
        }
    };

    return (
        <View style={globalStyles.container}>
            <Text style={globalStyles.titleText}>{Name}'s Profile</Text>
            <ScrollView>
                <Image 
                    source={{uri: Portrait}}
                    style={{width: 220, height: 220}}/>
                <Text>Email: {Email}</Text>
                <Text>{Profile}</Text>
            </ScrollView>
            <Button
                title = 'Request Friend'
                onPress={() => FriendRequestHandler()}/>
        </View>
    );
}
