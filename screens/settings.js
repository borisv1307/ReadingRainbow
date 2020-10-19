import React from 'react';
import { TouchableOpacity, View, Text} from 'react-native';
import { globalStyles } from '../styles/global';

export default function Settings() {
    return (
        <View style={globalStyles.container}>
            <TouchableOpacity style={globalStyles.smallButton}>
                <Text>Privacy</Text>
            </TouchableOpacity>
            <TouchableOpacity style={globalStyles.smallButton}>
                <Text>Change Password</Text>
            </TouchableOpacity>
            <TouchableOpacity style={globalStyles.smallButton}>
                <Text>Change Recommendations</Text>
            </TouchableOpacity>
            <TouchableOpacity style={globalStyles.warningButton}>
                <Text>Delete Account</Text>
            </TouchableOpacity>
      </View>
    );
}