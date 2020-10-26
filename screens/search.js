import React, { useState } from 'react';
import { ScrollView, TextInput, View, Text, TouchableOpacity, Image} from 'react-native';
import { globalStyles } from '../styles/global';
import { GetBooks } from '../api-functions/getbooks';
import { useNavigation } from '@react-navigation/native';

export default function Search() {
    const { navigate } = useNavigation();
    const [ text, setText ] = useState('');
    const [ results, setResults ] = useState([]);
    console.log(results);
    return (
        <View style={globalStyles.container}>
            <Text style={globalStyles.titleText}>Search for Books</Text>
            <View style={globalStyles.input}>
                <TextInput
                    autoFocus='true'
                    placeholder='Enter Title, Author or ISBN'
                    onChangeText={(text) => setText(text)} />
                <TouchableOpacity
                    disabled={text.length===0}
                    onPress={() => GetBooks(text).then(r=>setResults(r)) } >
                    <Image
                        style={globalStyles.icon}
                        source={require('../assets/490px-Magnifying_glass_icon.svg.png')} />
                </TouchableOpacity>
            </View>
            <ScrollView>
                {results.map(book => 
                    <TouchableOpacity
                        style={globalStyles.smallButton}
                        key={book.Index}
                        onPress={() => navigate('Book', {
                            id: book.Index,
                        })}>
                        <Text style={globalStyles.buttonText}> {book.Title} </Text>
                    </TouchableOpacity>
                )}
            </ScrollView>
        </View>
    );
}