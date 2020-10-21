import React from 'react';
import { View, Text, Button, ScrollView, Image } from 'react-native';
import { globalStyles } from '../styles/global';

export default function Library() {
    return (
        <View style={globalStyles.container}>
            <Text style={globalStyles.titleText}>My Library</Text>
            <Button title="Find Books"/>
            <View style={globalStyles.box}>
            <Image
                style={globalStyles.thumbnail}
                source={require("../assets/unnamed.jpg")} />
            </View>
            <Button title="Back"/>
        </View>
    );
}