import React from 'react';
import { View, Text, Button, ScrollView, Image } from 'react-native';
import { globalStyles } from '../styles/global';
import {GetBooksFromDatabase} from '../api-functions/GetBooksFromDatabase';

export default function Library() {
    return (
        <View style={globalStyles.container}>
            <Text style={globalStyles.titleText}>My Library</Text>
            <Button
                disabled={text.length===0}
                title='Get My Library'
                onPress={() => GetBooks().then(r=>setResults(r)) } />
            <View style={globalStyles.box}>
            <ScrollView>
                {results.map(book => <Text style={globalStyles.item} key={book.Index}> {book.Title} </Text>)}
            </ScrollView>
            <Image
                style={globalStyles.thumbnail}
                source={require("../assets/unnamed.jpg")} />
            </View>
            <Button title="Back"/>
        </View>
    );
}