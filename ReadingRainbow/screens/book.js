import React from 'react';
import { View, Text, Image, Button } from 'react-native';
import { globalStyles } from '../styles/global';
import { useNavigation } from '@react-navigation/native';

export default function Book({route}) {
    const { title, author, thumbnail, pubDate, pageCount, description } = route.params;
    const { navigate } = useNavigation();
    return (
        <View style={globalStyles.container}>
            <Text style={globalStyles.titleText}>{title}</Text>
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
            <View>
                <Button title='Wishlist' onPress={() => navigate('WishList')}/>
                <Button title='Add To Library' />
            </View>
        </View>
    );
}