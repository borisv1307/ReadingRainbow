import React from 'react';
import { Image, View, Text, Flatlist } from 'react-native';
import { globalStyles } from '../styles/global';

export default function Profile() {
    return (
        <View style={globalStyles.container}>
            <Text style={globalStyles.titleText}>Paige's Profile</Text>
            <View style={globalStyles.profileInfo}>
                <Text>About me info!</Text>
                <Text>Paige's Wishlist!</Text>
                <Image
                    style={{
                        width: "100%",
                        height: "60%"
                    }}
                    source={require("../assets/unnamed.jpg")} />
                <Text>Paige's Library</Text>
                <Image
                style={{
                    width: "100%",
                    height: "60%"
                    }}
                source={require("../assets/unnamed.jpg")} />
                <Text>Paige is reading</Text>
                <Image
                style={{
                    width: "100%",
                    height: "60%"
                    }}
                source={require("../assets/unnamed.jpg")} />
                <Text>Paige Turner's Friends</Text>
            </View>
        </View>
    );
}
