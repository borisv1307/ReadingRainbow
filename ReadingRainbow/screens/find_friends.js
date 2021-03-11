import React, { useEffect, useState } from 'react';
import { TouchableOpacity, Button, AsyncStorage, View, Text, ActivityIndicator } from 'react-native';
import { globalStyles } from '../styles/global';
import { useNavigation } from '@react-navigation/native';
import { GetPeople } from '../api-functions/getPeople';
import { ScrollView } from 'react-native-gesture-handler';

export default function FindFriends() {
    const { navigate } = useNavigation('');
    const [ people, setPeopleResults ] = useState();
    const [ ID, setID ] = useState(0);

    useEffect(() => {
        GetPeople().then(r => setPeopleResults(r));
    }, []);

    return (
        <View style={globalStyles.container}>
            <Text style={globalStyles.titleText}>Find Friends</Text>
            {(people) ?
                <ScrollView> 
                    {people.map((person, index) => 
                        <TouchableOpacity
                            key={index}
                            onPress={() => navigate('FriendProfile', {
                                Email: person.Email,
                                Name: person.Name,
                                Portrait: person.Portrait,
                                Profile: person.Profile,
                            })}
                        > 
                            <Text style={globalStyles.item} key={person.Name}> {person.Name} </Text>
                        </TouchableOpacity>
                    )}
                </ScrollView>
            :
                <ActivityIndicator color="black"/>
            }
        </View>
        
    );
}