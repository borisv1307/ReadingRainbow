import React from 'react';
import { AsyncStorage, View, Text, Image, Button, ScrollView } from 'react-native';
import { globalStyles } from '../styles/global';
import { useNavigation } from '@react-navigation/native';
import { AddBook } from '../api-functions/addBook';

export default function Book({route}) {
    const { title, author, thumbnail, pubDate, pageCount, description } = route.params;
    const { navigate } = useNavigation();

    var bookDisplayed = {
        title: title,
        author: author,
        thumbnail: thumbnail,
        pubDate: pubDate,
        pageCount: pageCount,
        description: description,
    }

    async function addToLibraryHandle() {
        try {
            AsyncStorage.getItem('username').then(user => {
                AddBook(user, bookDisplayed);
            });
        } catch (e) {
            console.log(e);
        }
    };

    return (
        <View style={globalStyles.container}>
            <Text style={globalStyles.titleText}>{title}</Text>
            <ScrollView>
                <Image
                    style={globalStyles.thumbnail}
                    source={{uri: thumbnail}}/>
                <Text style={globalStyles.paragraph}>Title: {title}</Text>
                <Text>Author(s): {author}</Text>
                <Text>Published: {pubDate}</Text>
                <Text>Description: {description}</Text>
                <Text>Page Count: {pageCount}</Text>
                <Text>ISBN: </Text>
                <Button title='Click here for more information' /*onPress={bookInformationLink}*/ />
                <Text>Index:</Text>
                <Text>Categories: </Text>
            </ScrollView>
            <View>
                <Button title='Add To Wishlist'/>
                <Button title='Add To Library' onPress={() => addToLibraryHandle()}/>
            </View>
        </View>
    );
}