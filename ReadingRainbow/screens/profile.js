import React, { useEffect, useState } from 'react';
import { View, Text, Button, ScrollView, ActivityIndicator, AsyncStorage } from 'react-native';
import { globalStyles } from '../styles/global';
import { GetUserProfile } from '../api-functions/getUserProfile';
import { GetUserLibrary } from '../api-functions/getUserLibrary';

export default function Profile() {
    const [ proResults, setProResults ] = useState({});
    const [ libResults, setLibResults ] = useState([]);
    
    useEffect(() => {
        AsyncStorage.getItem('username').then(user => {
            GetUserProfile(user).then(profile => setProResults(profile));
            GetUserLibrary(user).then(library => setLibResults(library));
        });
    }, []);

    return (
        <View style={globalStyles.container}>
            <Text style={globalStyles.titleText}>Paige's Profile</Text>
            
            { (proResults && libResults) ? 
                <ScrollView>
                    {libResults.map(book => <Text style={globalStyles.item} key={book.Id}> {book.Title} </Text>)}
                </ScrollView>
                : 
                <ActivityIndicator/>
            }
        </View>
    );
}
