import React from 'react';
import { ScrollView, FlatList, Button, TextInput, TouchableOpacity, View, Text } from 'react-native';
import { globalStyles } from '../styles/global';
import { useNavigation } from '@react-navigation/native';

export default function FriendList() {
    const { navigate } = useNavigation('');
    return (
        <View style={globalStyles.container}>
          <Text style={globalStyles.titleText}>Friends List</Text>
          <TextInput style={globalStyles.input}
            keyboardType='default'
            placeholder='Enter Username'
            onChangeText={(val) => setName(val)} />
          <Button title='Search!' />
          <TouchableOpacity onPress={() => navigate('Profile')}>
            <FlatList
                data={[
                    {key: 'Yang Wen-Li'},
                    {key: 'Reinhard von Lohengramm'},
                    {key: 'Oskar von Reuenthal'},
                    {key: 'Julian Minci'},
                ]}
                renderItem={({item}) => <Text style={globalStyles.item}>{item.key}</Text>}
                />
            </TouchableOpacity>
        </View>
        
    );
}