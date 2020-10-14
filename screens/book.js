import React from 'react';
import { View, Text, Image, Button } from 'react-native';
import { globalStyles } from '../styles/global';
import { useNavigation } from '@react-navigation/native';

export default function Book() {
    const { navigate } = useNavigation();
    return (
        <View style={globalStyles.container}>
            <Text>Book Screen</Text>
            <Image
                style={globalStyles.thumbnail}
                source={require("../assets/unnamed.jpg")} />
            <Text style={globalStyles.paragraph}>
                Title:
            </Text>
            <Text>
                Author(s):
            </Text>
            <Text>
                Published:
            </Text>
            <Text>
                Description:
            </Text>
            <Text>
                Page Count:
            </Text>
            <Text>
                ISBN: (ISBN_13,ISBN_10,OTHER)
            </Text>
            <Button title='Click here for more information' /*onPress={bookInformationLink}*/ />
            <Text>
                Index : name.id,
            </Text>
            <Text>
                Categories: name.volumeInfo.categories
            </Text>
            <View>
                <Button title='Wishlist' onPress={() => navigate('WishList')}/>
                <Button title='Add To Library' />
            </View>
        </View>
    );
}