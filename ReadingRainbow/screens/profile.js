import React, { useEffect, useState } from 'react';
import { View, Text, Button, ScrollView } from 'react-native';
import { globalStyles } from '../styles/global';
import { GetUserProfile } from '../api-functions/getUserProfile';
import { GetUserLibrary } from '../api-functions/getUserLibrary';
import * as SecureStore from 'expo-secure-store';

export default function Profile() {
    const [ proresults, setProResults ] = useState('');
    // const [ libresults, setLibResults ] = useState([]);
    
    useEffect(() => {
        GetUserProfile('april2').then(r=>setProResults(r));
        // GetUserLibrary('april').then(s=>setLibResults(s));
    }, [])

    return (
        <View style={globalStyles.container}>
            <Text style={globalStyles.titleText}>Paige's Profile</Text>
            {/* <View>
                {proresults.map(user => <Text key={user.Index}> {user.UserName} </Text>)}
            </View> */}

            {/* <ScrollView>
                {libresults.map(book => <Text style={globalStyles.item} key={book.Index}> {book.Name} </Text>)}
            </ScrollView> */}
            {/* <View style={globalStyles.profileInfo}>
                <Text>About me info!</Text>
                <Button
                title='Profile info'
                onPress={() => GetUserLibrary('Frederica Greenhill').then(r=>setResults(r)) } />
            </View>
            <ScrollView>
                {results.map(book => <Text style={globalStyles.item} key={book.Index}> {book.Title} </Text>)}
            </ScrollView> */}
        </View>
    );
}
