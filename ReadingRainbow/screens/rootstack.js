import React from 'react';
import { createStackNavigator } from '@react-navigation/stack';
import SignIn from './sign_in';
import SignUp from './sign_up';

const RootStack = createStackNavigator();

const RootStackScreen = ({navigation}) => (
    <RootStack.Navigator headerMode='screen'>
        <RootStack.Screen name="SignInScreen" component={SignIn}/>
        <RootStack.Screen name="SignUpScreen" component={SignUp}/>
    </RootStack.Navigator>
);

export default RootStackScreen;