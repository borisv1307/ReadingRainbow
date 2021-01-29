import React, { useEffect, useState } from 'react';
import { View, Text, Image, ScrollView, ActivityIndicator, AsyncStorage, FlatList, TouchableOpacity } from 'react-native';
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
            <Text style={globalStyles.profileInfo}>Your Library</Text>
            { (proResults && libResults) ? 
                <ScrollView>
                    <FlatList
                        horizontal = {true}
                        showsHorizontalScrollIndicator={true}
                        data={libResults}
                        keyExtractor={({id}, index) => id}
                        renderItem={({item}) => (
                            <TouchableOpacity onPress={() => navigate('Book', {
                                title: item.Title,
                                author: item.Author,
                                thumbnail: item.Thumbnail,
                                pubDate: item.PublishedDate,
                                pageCount: item.NumberPages,
                                description: item.Description,
                            })}>
                                <Image 
                                    source={{uri: item.Thumbnail}}
                                    style={{width: 128, height: 205}}
                                />
                            </TouchableOpacity>
                        )}>
                    </FlatList>
                </ScrollView>
                : 
                <ActivityIndicator/>
            }
        </View>
    );
}
