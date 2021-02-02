import React, { useState } from 'react';
import { TouchableOpacity, ScrollView, Button, TextInput, View, Text } from 'react-native';
import { globalStyles } from '../styles/global';
import { GetBooks } from '../api-functions/getbooks';
import { useNavigation } from '@react-navigation/native';

export default function Search() {
    const { navigate } = useNavigation();
    const [ text, setText ] = useState('');
    const [ results, setResults ] = useState([]);
   
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
                {results.map(book => 
                    <TouchableOpacity onPress={() => navigate('Book', {
                        title: book.Title,
                        author: book.Author,
                        thumbnail: book.Thumbnail,
                        pubDate: book.PublishedDate,
                        pageCount: book.NumberPages,
                        description: book.Description,
                    })}>
                        <Text style={globalStyles.item} key={book.Index}> {book.Title} </Text>
                    </TouchableOpacity>
                )}   
            </ScrollView>
        </View>
    );
}
