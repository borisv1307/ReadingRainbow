import React, { useState } from 'react';
import { Image, TouchableOpacity, ScrollView, Button, TextInput, View, Text } from 'react-native';
import { globalStyles } from '../styles/global';

export default function Search() {
    const [ text, setText ] = useState('');
    return (
        <View style={globalStyles.container}>
            <Text style={globalStyles.titleText}>Search for Books</Text>
            <TextInput style={globalStyles.input}
                keyboardType="default"
                placeholder="Enter Title, Author or ISBN"
                onChangeText={(text) => setText(text)} />
            <Button title='Search!' /*onPress={() => GetBooks(text)}*/ />
    <ScrollView>
        <Text>Search Results</Text>
    </ScrollView>
    </View>
    );
}