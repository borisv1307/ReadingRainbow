import React, { useEffect, useState } from 'react';
import { View, Text, Button, Image, ScrollView, ActivityIndicator, AsyncStorage, FlatList, TouchableOpacity } from 'react-native';
import { globalStyles } from '../styles/global';
import { GetUserProfile } from '../api-functions/getUserProfile';
import { useNavigation } from '@react-navigation/native';
import { UpdateUserProfile } from '../api-functions/updateUserProfile';

export default function UploadPic() {
    const { navigate } = useNavigation();
    const [ proResults, setProResults ] = useState({});
    const defaultProfilePicture = "https://pixy.org/src/80/805032.png";

    useEffect(() => {
        AsyncStorage.getItem('username').then(user => {
            GetUserProfile(user).then(profile => setProResults(profile));
        });
    }, []);

    const profilePicHandle = (image) => {
        setProResults(proResults => ({
            ...proResults,
            Portrait: image
        }));
        console.log('upload pic proresults', proResults);
        UpdateUserProfile(proResults);
    }

    return (
        <View style={globalStyles.container}>
            <Text style={globalStyles.titleText}>Choose Your Profile Picture!</Text>
            <Button
                title='Upload My Own Picture'/>
            <Button
                title='Use Default Picture'
                onPress={()=>profilePicHandle(defaultProfilePicture)}/>
        </View>
    );
}
