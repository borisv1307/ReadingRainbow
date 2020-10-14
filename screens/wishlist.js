import React from 'react';
import { ScrollView, Image, View, Text } from 'react-native';
import { globalStyles } from '../styles/global';

export default function Wishlist() {
    return (
        <View style={globalStyles.container}>
            <Text style={globalStyles.titleText}>Paige's Wishlist</Text>
            <View style={globalStyles.box}>
                <ScrollView>
                    <Image source={require('../assets/unnamed.jpg')} style={globalStyles.thumbnail} />
                    <Image source={require('../assets/unnamed.jpg')} style={globalStyles.thumbnail} />
                </ScrollView>
            </View>
        </View>
    );
}