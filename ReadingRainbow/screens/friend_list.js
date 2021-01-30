import React, {useState} from 'react';
import { ScrollView, FlatList, Button, TextInput, TouchableOpacity, View, Text, ActivityIndicator } from 'react-native';
import { globalStyles } from '../styles/global';
import { useNavigation } from '@react-navigation/native';

export default function FriendList() {
    const { navigate } = useNavigation('');
    const [ friendResults, setFriendResults ] = useState();

    // useEffect(() => {
    //     AsyncStorage.getItem('username').then(user => {
    //         GetFriendList(user).then(friend => setFriendResults(friend));
    //     });
    // }, []);

    return (
        <View style={globalStyles.container}>
            <Text style={globalStyles.titleText}>Friends List</Text>
            {(friendResults) ?
                <View>
                    {/* friend results displayed here */}
                </View>
            :
                <ActivityIndicator color="black"/>
            }
        </View>
        
    );
}