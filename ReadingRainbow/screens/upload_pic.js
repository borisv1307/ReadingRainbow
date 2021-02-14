import React, { useEffect, useState } from 'react';
import { View, Text, Image, ScrollView, ActivityIndicator, AsyncStorage, FlatList, TouchableOpacity } from 'react-native';
import { globalStyles } from '../styles/global';
import { GetUserProfile } from '../api-functions/getUserProfile';
import { useNavigation } from '@react-navigation/native';

export default function UploadPic() {
    const { navigate } = useNavigation();
    const [ proResults, setProResults ] = useState({});
    const defaultProfilePicture = "https://pixy.org/src/80/805032.png";

    useEffect(() => {
        AsyncStorage.getItem('username').then(user => {
            GetUserProfile(user).then(profile => setProResults(profile));
        });
    }, []);

    return (
        <View style={globalStyles.container}>
            <Text style={globalStyles.titleText}>Choose Your Profile Picture!</Text>
            <Button
                titleText='Upload My Own Picture'
                onPress={()=>UploadPicture()}/>
            <Button
                titleText='Use Default Picture'
                onPress={(defaultProfilePicture)=>setProResults({...data, profile_pic: defaultProfilePicture})}/>
        </View>
    );
}
