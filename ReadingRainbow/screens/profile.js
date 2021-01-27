import React, { useEffect, useState } from 'react';
import { View, Text, Button, ScrollView, ActivityIndicator, AsyncStorage, FlatList, TouchableOpacity } from 'react-native';
import { globalStyles } from '../styles/global';
import { GetUserProfile } from '../api-functions/getUserProfile';
import { GetUserLibrary } from '../api-functions/getUserLibrary';
import { useNavigation } from '@react-navigation/native';

export default function Profile() {
    const { navigate } = useNavigation();
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
            <Text style={globalStyles.titleText}>Your Profile</Text>
            
            { (proResults && libResults) ? 
                <ScrollView>
                    {libResults.map(book => 
                        <TouchableOpacity key={book.Id} onPress={() => navigate('Book')}>
                            <Text style={globalStyles.item}> 
                                {book.Title} 
                            </Text>
                        </TouchableOpacity>
                    )}
                </ScrollView>
                : 
                <ActivityIndicator/>
            }
        </View>
    );
}
