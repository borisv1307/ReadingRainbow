import React, { useEffect, useState } from 'react';
import { Button, AsyncStorage, View, Text, ActivityIndicator } from 'react-native';
import { globalStyles } from '../styles/global';
import { useNavigation } from '@react-navigation/native';
import { GetFriends } from '../api-functions/getFriends';

export default function FriendList() {
    const { navigate } = useNavigation('');
    const [ friendResults, setFriendResults ] = useState();

    useEffect(() => {
        AsyncStorage.getItem('username').then(user => {
            console.log('friend_list user: ', user);
            GetFriends().then(friend => setFriendResults(friend));
        }).then(
            console.log(people)
        );
    }, []);

    return (
        <View style={globalStyles.container}>
            <Text style={globalStyles.titleText}>Friends List</Text>
            {(friendResults) ?
                <View>
                    
                </View>
            :
                <ActivityIndicator color="black"/>
            }
            <Button
                title = 'AddFriend'/>
        </View>
        
    );
}