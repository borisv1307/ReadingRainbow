import React, { useState } from 'react';
import { ScrollView, Button, TextInput, View, Text } from 'react-native';
import { globalStyles } from '../styles/global';
import { GetBooks } from '../api-functions/getbooks';

export default function Search() {
    const [ text, setText ] = useState('');
    const [ results, setResults ] = useState([]);
    console.log(results);
    return (
        <View style={globalStyles.container}>
            <Text style={globalStyles.titleText}>Search for Books</Text>
            <TextInput style={globalStyles.input}
                keyboardType="default"
                placeholder="Enter Title, Author or ISBN"
                onChangeText={(text) => setText(text)} />
            <Button
                disabled={text.length===0}
                title='Search!'
                onPress={() => GetBooks(text).then(r=>setResults(r)) } />
            <ScrollView>
                {results.map(book => <Text style={globalStyles.item} key={book.Id}> {book.Title} </Text>)}
            </ScrollView>
        </View>
    );
}
